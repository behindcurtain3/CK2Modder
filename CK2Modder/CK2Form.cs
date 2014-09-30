using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CK2Modder.GameData;
using CK2Modder.GameData.common;
using CK2Modder.GameData.history.characters;
using CK2Modder.Util;
using RavSoft;

namespace CK2Modder
{
    public partial class CK2Form : Form
    {
        public static readonly String SteamDirectory = "C:\\Program Files\\Steam\\steamapps\\common\\crusader kings ii";
        public static readonly String SteamDirectoryX86 = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\crusader kings ii";

        public static readonly String VanillaDynastyFile = "/common/dynasties/00_dynasties.txt";
        public static readonly String VanillaCulturesFile = "/common/cultures/00_cultures.txt";

        public static readonly String VanillaDynastiesPath = "/common/dynasties/";
        public static readonly String VanillaCharactersPath = "/history/characters/";
        public static readonly String VanillaCulturesPath = "/common/cultures/";

        // Display string
        public static readonly String DefaultWindowTitle = "CK2 Modder";
        public static readonly String DefaultFileListView = "View All Files";

        public String WorkingLocation { get; set; }
        public Mod CurrentMod { get; set; }
        public String CurrentMode { get; set; }

        public Culture SelectedCulture { get; set; }
        public TreeNode SelectedCultureNode { get; set; }
        

        #region Initialization

        public CK2Form(string filename)
        {
            Initialize();

            // Attempt to load the mod passed in
            if (File.Exists(filename))
            {
                SetCurrentMod(Mod.LoadFromFile(filename));
            }

        }

        public CK2Form()
        {
            Initialize();

            // Attempt to load the last mod
            if (File.Exists(UserPreferences.Default.LastMod))
            {
                SetCurrentMod(Mod.LoadFromFile(UserPreferences.Default.LastMod));
            }
        }

        public void Initialize()
        {
            // Setup the working location
            WorkingLocation = UserPreferences.Default.WorkingLocation;

            InitializeComponent();

            // Setup the cue's on textboxes
            CueProvider.SetCue(dataFilesFilter, "Filter Files");
            CueProvider.SetCue(dataFilter, "Filter Data");

            // Setup the data type selection
            selectDataType.SelectedIndex = 0;

            // show lines numbers
            dataTextEditor.Margins[0].Width = 20;
            dataTextEditor.ConfigurationManager.Language = "python";
            dataTextEditor.ConfigurationManager.Configure();
            dataTextEditor.Caret.HighlightCurrentLine = true;
            dataTextEditor.Caret.CurrentLineBackgroundColor = System.Drawing.Color.DarkGray;
            dataTextEditor.Caret.CurrentLineBackgroundAlpha = 64;

            if(!Directory.Exists(WorkingLocation))
            {
                if (!SelectWorkingLocation())
                {
                    MessageBox.Show("Please select an installation directory for Crusader Kings II.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }

            workingLocationStripStatusLabel.Text = String.Format("Working Location: {0}", WorkingLocation);

            dynastyBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dynastyBackgroundWorker_RunWorkerCompleted);
            cultureBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(cultureBackgroundWorker_RunWorkerCompleted);
            characterBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(characterBackgroundWorker_RunWorkerCompleted);
        }        

        #endregion

        #region Background Workers

        private void dynastyBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Dynasty> loadingDynasties = new List<Dynasty>();

            BackgroundWorker worker = sender as BackgroundWorker;
            StreamReader stream = e.Argument as StreamReader;

            List<String> lines = new List<string>();
            String currentLine;
            int bracketCounter = 0;
            while ((currentLine = stream.ReadLine()) != null)
            {
                // read through the lines of each dynasty adding them to the lines list
                // when the end of the dynasty is reached load the dynasty from the list
                // and reset the list for the next dynasty

                // skip processing any commented out lines or empty lines
                if (currentLine.StartsWith("#") || String.IsNullOrEmpty(currentLine))
                    continue;

                // check for brackets, they keep track of whether the entire character has been added yet
                if (currentLine.Contains("{"))
                    bracketCounter++;
                if (currentLine.Contains("}"))
                    bracketCounter--;

                // add the current line
                lines.Add(currentLine);

                // end of a character
                if (currentLine.Contains("}") && bracketCounter == 0)
                {
                    // attempt to the load the dynasty
                    Dynasty dynasty = Dynasty.Load(lines);

                    // if successful add the character to the character list
                    if (dynasty != null)
                    {
                        dynasty.BelongsTo = Path.GetFileNameWithoutExtension(CurrentMod.DynastyFilesToLoad.Peek());
                        loadingDynasties.Add(dynasty);
                    }

                    // reset the list
                    lines.Clear();
                }
            }
          
            // clsoe the stream
            stream.Close();

            // set the results
            e.Result = loadingDynasties;
        }

        void dynastyBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (CurrentMod == null)
                return;

            List<Dynasty> rows = e.Result as List<Dynasty>;

            foreach (Dynasty d in rows)
                if(!CurrentMod.Dynasties.Contains(d))
                    CurrentMod.Dynasties.Add(d);

            // Dequeue the current item
            CurrentMod.DynastyFilesToLoad.Dequeue();

            // Run the next file in the queue
            if (CurrentMod.DynastyFilesToLoad.Count > 0)
            {
                StreamReader reader = new StreamReader(CurrentMod.DynastyFilesToLoad.Peek(), Encoding.Default, true);
                dynastyBackgroundWorker.RunWorkerAsync(reader);
            }
            else
            {
                UpdateDataView(selectDataType.Text);
            }
        }
        
        private void cultureBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Store the ids
            List<Culture> loadingCultures = new List<Culture>();

            BackgroundWorker worker = sender as BackgroundWorker;
            StreamReader stream = e.Argument as StreamReader;

            List<String> lines = new List<string>();
            String currentLine;
            int bracketCounter = 0;
            while ((currentLine = stream.ReadLine()) != null)
            {
                // read through the lines of each culture adding them to the lines list
                // when the end of the culture is reached load the culture from the list
                // and reset the list for the next culture

                // skip processing any commented out lines or empty lines
                if (currentLine.StartsWith("#") || String.IsNullOrEmpty(currentLine))
                    continue;

                // check for brackets, they keep track of whether the entire character has been added yet
                if (currentLine.Contains("{"))
                    bracketCounter++;
                if (currentLine.Contains("}"))
                    bracketCounter--;

                // add the current line
                lines.Add(currentLine);

                // end of a character
                if (currentLine.Contains("}") && bracketCounter == 0)
                {
                    // attempt to the load the culture
                    Culture culture = Culture.Load(lines);

                    // if successful add the culture to the culture list
                    if (culture != null)
                    {
                        culture.BelongsTo = Path.GetFileNameWithoutExtension(CurrentMod.CultureFilesToLoad.Peek());
                        loadingCultures.Add(culture);
                    }

                    // reset the list
                    lines.Clear();
                }
            }
            
            // close the stream
            stream.Close();

            // store the results
            e.Result = loadingCultures;
        }

        void cultureBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (CurrentMod == null)
                return;

            List<Culture> rows = e.Result as List<Culture>;
            
            foreach (Culture c in rows)
            {
                CurrentMod.Cultures.Add(c);
            }

            // Dequeue the current item
            CurrentMod.CultureFilesToLoad.Dequeue();

            // Run the next file in the queue
            if (CurrentMod.CultureFilesToLoad.Count > 0)
            {
                StreamReader reader = new StreamReader(CurrentMod.CultureFilesToLoad.Peek(), Encoding.Default, true);
                cultureBackgroundWorker.RunWorkerAsync(reader);
            }
        }

        private void characterBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Character> loadingCharacters = new List<Character>();

            BackgroundWorker worker = sender as BackgroundWorker;
            StreamReader stream = e.Argument as StreamReader;

            List<String> lines = new List<string>();
            String currentLine;
            int bracketCounter = 0;
            while ((currentLine = stream.ReadLine()) != null)
            {
                // read through the lines of each character adding them to the lines list
                // when the end of the character is reached load the character from the list
                // and reset the list for the next character

                // skip processing any commented out lines or empty lines
                //if (currentLine.StartsWith("#") || String.IsNullOrEmpty(currentLine))
                //    continue;

                // check for brackets, they keep track of whether the entire character has been added yet
                if (currentLine.Contains("{"))
                    bracketCounter += StringExtensions.CountOccurences(currentLine, "{");
                if (currentLine.Contains("}"))
                    bracketCounter -= StringExtensions.CountOccurences(currentLine, "}");

                // add the current line
                lines.Add(currentLine);

                // end of a character
                if (currentLine.Contains("}") && bracketCounter == 0)
                {
                    // attempt to the load the character
                    Character c = Character.Load(lines);
                    
                    // if successful add the character to the character list
                    if (c != null)
                    {
                        c.BelongsTo = Path.GetFileNameWithoutExtension(CurrentMod.CharacterFilesToLoad.Peek());
                        loadingCharacters.Add(c);
                    }

                    // reset the list
                    lines.Clear();
                }
            }

            // close the file stream
            stream.Close();

            // set the result for this worker
            e.Result = loadingCharacters;
        }

        void characterBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (CurrentMod == null)
                return;

            List<Character> rows = e.Result as List<Character>;

            foreach (Character c in rows)
                if (!CurrentMod.Characters.Contains(c))
                    CurrentMod.Characters.Add(c);

            // Dequeue the current item
            CurrentMod.CharacterFilesToLoad.Dequeue();

            // Run the next file in the queue
            if (CurrentMod.CharacterFilesToLoad.Count > 0)
            {
                StreamReader reader = new StreamReader(CurrentMod.CharacterFilesToLoad.Peek(), Encoding.Default, true);
                characterBackgroundWorker.RunWorkerAsync(reader);
            }
            // if done update the character list box
            else
            {
                UpdateDataView(selectDataType.Text);
            }
        }

        #endregion

        #region Mod functions

        public void SetCurrentMod(Mod m)
        {
            if (m == null)
            {
                MessageBox.Show("Unable to load the specified mod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                modClosedPanel.Visible = true;
                return;
            }

            // Set the mod
            CurrentMod = m;

            // data tab
            dataFilesListBox.Items.Clear();
            dataFilesListBox.Items.Add(DefaultFileListView);
            dataFilesListBox.SelectedIndex = 0;

            // Load the dynasties
            String dynastiesFolder = CurrentMod.ModRootDirectory + VanillaDynastiesPath;
            if(Directory.Exists(dynastiesFolder))
            {
                // Go through each file and add it to the queue
                String[] files = Directory.GetFiles(dynastiesFolder);

                foreach (String filePath in files)
                {
                    String file = Path.GetFileNameWithoutExtension(filePath);

                    if (!CurrentMod.DynastyFiles.Contains(file))
                    {
                        CurrentMod.DynastyFiles.Add(file);
                        CurrentMod.DynastyFilesToLoad.Enqueue(filePath);
                    }
                }

                // start loading the files
                if (CurrentMod.DynastyFilesToLoad.Count > 0)
                {
                    StreamReader reader = new StreamReader(CurrentMod.DynastyFilesToLoad.Peek(), Encoding.Default, true);
                    dynastyBackgroundWorker.RunWorkerAsync(reader);
                }
            }

            // Load the cultures
            String culturesFolder = CurrentMod.ModRootDirectory + VanillaCulturesPath;
            if (Directory.Exists(culturesFolder))
            {
                // Go through each file and add it to the queue
                String[] files = Directory.GetFiles(culturesFolder);

                foreach (String filePath in files)
                {
                    String file = Path.GetFileNameWithoutExtension(filePath);

                    if (!CurrentMod.CultureFiles.Contains(file))
                    {
                        CurrentMod.CultureFiles.Add(file);
                        CurrentMod.CultureFilesToLoad.Enqueue(filePath);
                    }
                }

                // start loading the files
                if (CurrentMod.CultureFilesToLoad.Count > 0)
                {
                    StreamReader reader = new StreamReader(CurrentMod.CultureFilesToLoad.Peek(), Encoding.Default, true);
                    cultureBackgroundWorker.RunWorkerAsync(reader);
                }
            }

            // Load characters
            String charactersFolder = CurrentMod.ModRootDirectory + VanillaCharactersPath;
            if (Directory.Exists(charactersFolder))
            {
                // Go through each file and add it to the listview
                String[] files = Directory.GetFiles(charactersFolder);

                foreach (String filePath in files)
                {
                    String file = Path.GetFileNameWithoutExtension(filePath);

                    if (!CurrentMod.CharacterFiles.Contains(file))
                    {
                        CurrentMod.CharacterFiles.Add(file);
                        CurrentMod.CharacterFilesToLoad.Enqueue(filePath);
                    }
                }

                // Start loading character files
                if (CurrentMod.CharacterFilesToLoad.Count > 0)
                {
                    StreamReader reader = new StreamReader(CurrentMod.CharacterFilesToLoad.Peek(), Encoding.Default, true);
                    characterBackgroundWorker.RunWorkerAsync(reader);
                }
            }

            UserPreferences.Default.LastMod = CurrentMod.StorageLocation + "/" + CurrentMod.Name + ".mod";
            UserPreferences.Default.Save();

            saveToolStripMenuItem.Enabled = true;
            closeModToolStripMenuItem.Enabled = true;
            closeWithoutSavingToolStripMenuItem.Enabled = true;

            // Hide the panel
            modClosedPanel.Visible = false;

            // Update title
            this.Text = DefaultWindowTitle + " - " + CurrentMod.Name;
        }

        public void CloseMod()
        {
            // Menu options
            saveToolStripMenuItem.Enabled = false;
            closeModToolStripMenuItem.Enabled = false;
            closeWithoutSavingToolStripMenuItem.Enabled = false;

            CurrentMod = null;

            // reset the data tab
            dataFilesListBox.Items.Clear();
            dataListBox.Items.Clear();

            // reset editor
            dataTextEditor.DataBindings.Clear();
            dataTextEditor.Text = "";

            // reset the filters
            dataFilesFilter.Text = "";
            dataFilter.Text = "";

            UserPreferences.Default.LastMod = "";
            UserPreferences.Default.Save();

            // Show the panel
            modClosedPanel.Visible = true;

            this.Text = DefaultWindowTitle;
        }
               
        private bool SelectWorkingLocation()
        {
            folderBrowserDialog = new FolderBrowserDialog();
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.Description = "Select the main Crusader Kings II installation directory containing ck2.exe";
            folderBrowserDialog.SelectedPath = SteamDirectory;
            folderBrowserDialog.ShowNewFolderButton = false;

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(folderBrowserDialog.SelectedPath + "/CK2game.exe"))
                {
                    WorkingLocation = folderBrowserDialog.SelectedPath;
                    workingLocationStripStatusLabel.Text = String.Format("Working Location: {0}", WorkingLocation);
                    UserPreferences.Default.WorkingLocation = WorkingLocation;
                    UserPreferences.Default.Save();
                    return true;
                }
                else
                {
                    MessageBox.Show("Please select an installation directory for Crusader Kings II.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void SaveMod()
        {
            if (CurrentMod == null)
                return;

            // Write out the mod file
            StreamWriter stream = File.CreateText(CurrentMod.StorageLocation + "/" + CurrentMod.Name + ".mod");
            stream.Write(CurrentMod.Raw);
            stream.Close();

            // Make sure the mod directory exists
            if (!Directory.Exists(CurrentMod.ModRootDirectory))
            {
                Directory.CreateDirectory(CurrentMod.ModRootDirectory);
            }

            // Write out the dynasties
            if (CurrentMod.Dynasties.Count > 0)
            {
                if (!Directory.Exists(CurrentMod.ModRootDirectory + VanillaDynastiesPath))
                {
                    Directory.CreateDirectory(CurrentMod.ModRootDirectory + VanillaDynastiesPath);
                }

                foreach (String name in CurrentMod.DynastyFiles)
                {
                    stream = new StreamWriter(CurrentMod.ModRootDirectory + VanillaDynastiesPath + "/" + name + ".txt", false, Encoding.Default);

                    foreach (Dynasty d in CurrentMod.Dynasties)
                    {
                        if (d.BelongsTo.Equals(name))
                        {
                            stream.Write(d.ToString());
                        }
                    }

                    stream.Close();
                }
            }

            // Write out the cultures
            if (CurrentMod.Cultures.Count > 0)
            {   
                if (!Directory.Exists(CurrentMod.ModRootDirectory + VanillaCulturesPath))
                {
                    Directory.CreateDirectory(CurrentMod.ModRootDirectory + VanillaCulturesPath);
                }

                foreach (String name in CurrentMod.CultureFiles)
                {
                    stream = new StreamWriter(CurrentMod.ModRootDirectory + VanillaCulturesPath + "/" + name + ".txt", false, Encoding.Default);

                    foreach (Culture c in CurrentMod.Cultures)
                    {
                        if (c.BelongsTo.Equals(name))
                        {
                            stream.Write(c.ToString());
                        }
                    }

                    stream.Close();
                }
            }

            // Write out the characters
            if (CurrentMod.CharacterFiles.Count > 0)
            {
                if (!Directory.Exists(CurrentMod.ModRootDirectory + VanillaCharactersPath))
                {
                    Directory.CreateDirectory(CurrentMod.ModRootDirectory + VanillaCharactersPath);
                }

                foreach (String name in CurrentMod.CharacterFiles)
                {
                    stream = new StreamWriter(CurrentMod.ModRootDirectory + VanillaCharactersPath + "/" + name + ".txt", false, Encoding.Default);

                    foreach (Character c in CurrentMod.Characters)
                    {
                        if (c.BelongsTo.Equals(name))
                        {
                            stream.Write(c.ToString());
                        }
                    }

                    stream.Close();
                }
            }
        }

        private void PopulateDataListBox(List<String> list)
        {
            dataListBox.Items.AddRange(list.ToArray());
        }

        private void UpdateDataView(String mode)
        {

            // set the current mode
            CurrentMode = mode;

            // reset the lists
            dataFilesListBox.Items.Clear();
            dataListBox.Items.Clear();

            // reset the filters
            dataFilesFilter.Text = String.Empty;
            dataFilter.Text = String.Empty;

            // load the default views depending on the mode selected
            dataFilesListBox.Items.Add(DefaultFileListView);

            // store the data for the lists here
            List<String> files = new List<String>();
            List<String> data = new List<String>();

            switch (mode)
            {
                case "Mod Details":
                    UpdateTextEditor(CurrentMod);
                    HideSplitPanels();
                    break;
                case "Characters":
                    files.AddRange(CurrentMod.CharacterFiles);

                    foreach (Character c in CurrentMod.Characters)
                        data.Add(c.Display);

                    ShowSplitPanels();
                    break;
                case "Dynasties":
                    files.AddRange(CurrentMod.DynastyFiles);

                    foreach (Dynasty d in CurrentMod.Dynasties)
                        data.Add(d.Display);

                    ShowSplitPanels();
                    break;
                case "Cultures":
                    files.AddRange(CurrentMod.CultureFiles);

                    foreach (Culture c in CurrentMod.Cultures)
                        data.Add(c.Display);

                    ShowSplitPanels();
                    break;
            }

            // populate the lists
            dataFilesListBox.Items.AddRange(files.ToArray());            
            PopulateDataListBox(data);
        }

        private void UpdateDataListBox()
        {
            String filter = dataFilter.Text;
            String file = dataFilesListBox.SelectedItem as String;
            List<String> list = new List<String>();

            if (String.IsNullOrWhiteSpace(file))
                return;

            switch (CurrentMode)
            {
                case "Characters":
                    List<Character> characters = new List<Character>();
                    
                    // apply the files filter
                    if (!file.Equals(DefaultFileListView))
                    {
                        // Filter the files first
                        characters = CurrentMod.Characters.FindAll(delegate(Character c) { return c.BelongsTo.Equals(file); });
                    }
                    else
                    {
                        characters.AddRange(CurrentMod.Characters);
                    }

                    // check if the filter is an ID
                    int ID = Helpers.ParseInt(filter);
                    if (ID != -1)
                    {
                        characters = characters.FindAll(delegate(Character c) { return c.ID.ToString().Contains(ID.ToString()); });
                    }
                    // filter for names
                    else
                    {
                        // filter for the string passed in
                        if (!String.IsNullOrWhiteSpace(filter))
                        {
                            characters = characters.FindAll(delegate(Character c) { return c.Name.ToLower().Contains(filter); });
                        }
                    }

                    foreach (Character c in characters)
                        list.Add(c.Display);
                    break;

                case "Dynasties":
                    List<Dynasty> dynasties = new List<Dynasty>();
                    dynasties.AddRange(CurrentMod.Dynasties);

                    // apply the files filter
                    if (!file.Equals(DefaultFileListView))
                    {
                        // Filter the files first
                        dynasties = CurrentMod.Dynasties.FindAll(delegate(Dynasty d) { return d.BelongsTo.Equals(file); });
                    }

                    // check if the filter is an ID
                    ID = Helpers.ParseInt(filter);
                    if (ID != -1)
                    {
                        dynasties = dynasties.FindAll(delegate(Dynasty d) { return d.ID.ToString().Contains(ID.ToString()); });
                    }
                    // filter for names
                    else
                    {
                        // filter for the string passed in
                        if (!String.IsNullOrWhiteSpace(filter))
                        {
                            dynasties = dynasties.FindAll(delegate(Dynasty d) { return d.Name.ToLower().Contains(filter); });
                        }
                    }

                    foreach (Dynasty d in dynasties)
                        list.Add(d.Display);
                    break;

                case "Cultures":
                    foreach (Culture c in CurrentMod.Cultures)
                        list.Add(c.Display);
                    break;
            }

            // clear out the list box
            dataListBox.Items.Clear();

            // Populate it
            PopulateDataListBox(list);
        }

        private void UpdateFileListBox()
        {
            String filter = dataFilesFilter.Text;
            List<String> list = new List<String>();

            switch (selectDataType.Text)
            {
                case "Characters":
                    list.AddRange(CurrentMod.CharacterFiles);
                    break;
                case "Dynasties":
                    list.AddRange(CurrentMod.DynastyFiles);
                    break;
                case "Cultures":
                    list.AddRange(CurrentMod.CultureFiles);
                    break;
            }

            // if there is a filter, apply it
            if (!String.IsNullOrWhiteSpace(filter))
            {
                list = list.FindAll(delegate(String s) { return s.Contains(filter); });
            }

            dataFilesListBox.Items.Clear();
            dataFilesListBox.Items.Add(DefaultFileListView);
            dataFilesListBox.Items.AddRange(list.ToArray());
        }

        private void UpdateTextEditor(ModResource resource)
        {
            // always clear the bindings on the text editor
            dataTextEditor.DataBindings.Clear();

            // if an object was selected add the raw data binding
            if (resource != null)
            {
                dataTextEditor.DataBindings.Add("Text", resource, "Raw");
            }
            // otherwise clear the text
            else
            {
                dataTextEditor.Text = "";
            }
        }

        private void CreateNewMod()
        {
            newModDialog = new SaveFileDialog();
            newModDialog.InitialDirectory = WorkingLocation + "\\mod";
            newModDialog.AddExtension = true;
            newModDialog.DefaultExt = ".mod";
            newModDialog.FileName = "Mod Name";
            newModDialog.Filter = "Mod File (*.mod)|*.mod";
            newModDialog.Title = "Select a save location and name for the mod";

            if (newModDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (CurrentMod != null)
                    CloseMod();

                // TODO: delete and remake the mod directory if it already exits to give the new mod a fresh start
                Mod mod = new Mod(Path.GetFileNameWithoutExtension(newModDialog.FileName));
                SetCurrentMod(mod);
            }
        }

        private void LoadMod()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Mod File (*.mod)|*.mod";
            openFileDialog.InitialDirectory = WorkingLocation + "\\mod";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select a mod to open";

            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (CurrentMod != null)
                    CloseMod();

                SetCurrentMod(Mod.LoadFromFile(openFileDialog.FileName));
            }
        }

        private void ShowSplitPanels()
        {
            secondarySplitPanel.Panel1Collapsed = false;
            secondarySplitPanel.Panel1.Show();

            mainSplitPanel.Panel1Collapsed = false;
            mainSplitPanel.Panel1.Show();

            dataFilesListBox.SelectedIndex = 0;
            dataListBox.SelectedIndex = 0;
        }

        private void HideSplitPanels()
        {
            mainSplitPanel.Panel1Collapsed = true;
            mainSplitPanel.Panel1.Hide();

            secondarySplitPanel.Panel1Collapsed = true;
            secondarySplitPanel.Panel1.Hide();
        }

        #endregion

        #region Events

        private void selectDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateDataView(selectDataType.Text);
        }

        private void dataFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateDataListBox();
        }

        private void dataFilter_TextChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateDataListBox();
        }


        private void dataFilesFilter_TextChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateFileListBox();
        }

        private void dataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            String selected = dataListBox.SelectedItem as String;
            ModResource resource = null;

            if (selected == null)
                return;

            int ID = Helpers.ParseInt(selected);
            string name = null;

            if (ID == -1)
            {
                name = selected;
            }

            // show the data
            switch (CurrentMode)
            {
                case "Characters":
                    resource = CurrentMod.Characters.Find(c => c.ID == ID);
                    break;

                case "Dynasties":
                    resource = CurrentMod.Dynasties.Find(d => d.ID == ID);
                    break;

                case "Cultures":
                    resource = CurrentMod.Cultures.Find(c => c.Name.Equals(name));
                    break;
            }

            UpdateTextEditor(resource);
        }

        private void CK2Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Process shortcuts
            if (e.KeyCode == Keys.F1)
                selectDataType.SelectedIndex = 0;
            else if (e.KeyCode == Keys.F2)
                selectDataType.SelectedIndex = 1;
            else if (e.KeyCode == Keys.F3)
                selectDataType.SelectedIndex = 2;
            else if (e.KeyCode == Keys.F4)
                selectDataType.SelectedIndex = 3;
        }

        private void newModButton_Click(object sender, EventArgs e)
        {
            CreateNewMod();
        }

        private void loadModButton_Click(object sender, EventArgs e)
        {
            LoadMod();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMod();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMod();
        }

        private void newModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewMod();
        }

        private void closeModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMod();
            CloseMod();
        }

        private void closeWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseMod();
        }

        private void workingLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectWorkingLocation();
        }

        private void deleteSelectedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            String selectedFile = dataFilesListBox.SelectedItem as String;

            //if (selectedFile == null || selectedFile.Equals(DefaultCharacterListView))
            //    return;

            if(MessageBox.Show("Deleting the file will delete all the characters in the file. Do you wish to continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
            {
                String originalFile = WorkingLocation + "/" + CurrentMod.Path + VanillaCharactersPath + selectedFile + ".txt";

                if (File.Exists(originalFile))
                {
                    File.Delete(originalFile);
                }

                dataFilesListBox.Items.Remove(selectedFile);
                CurrentMod.CharacterFiles.Remove(selectedFile);

                foreach (Character c in CurrentMod.Characters.ToList())
                    if (c.BelongsTo.Equals(selectedFile))
                        CurrentMod.Characters.Remove(c);

                // Set the view to all characters
                dataFilesListBox.SelectedIndex = 0;
            }
        }

        private void editSelectedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String selected = dataFilesListBox.SelectedItem as String;

            //if (selected == null || selected.Equals(DefaultCharacterListView))
            //    return;

            CharacterFileForm fileForm = new CharacterFileForm(selected);

            if (fileForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // If the user hit ok without changing the name don't do anything
                if (selected.Equals(fileForm.FileName))
                    return;

                if(fileForm.FileName.Equals(""))
                {
                    MessageBox.Show("The file must have a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (CurrentMod.CharacterFiles.Contains(fileForm.FileName))
                {
                    MessageBox.Show("The name you entered already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    String originalFile = WorkingLocation + "/" + CurrentMod.Path + VanillaCharactersPath + selected + ".txt";
                    String newFile = WorkingLocation + "/" + CurrentMod.Path + VanillaCharactersPath + fileForm.FileName + ".txt";

                    // Rename the actual file
                    if(File.Exists(originalFile) && !File.Exists(newFile))
                    {
                        // Copy the file, then delete the original
                        File.Copy(originalFile, newFile);
                        File.Delete(originalFile);

                        // Update the UI
                        CurrentMod.CharacterFiles.Remove(selected);
                        CurrentMod.CharacterFiles.Add(fileForm.FileName);
                        dataFilesListBox.Items.Remove(selected);
                        dataFilesListBox.Items.Add(fileForm.FileName);

                        // Update the characters
                        foreach (Character c in CurrentMod.Characters)
                        {
                            if (c.BelongsTo.Equals(selected))
                                c.BelongsTo = fileForm.FileName;
                        }
                    }
                }
            }
        }

        private void characterFilesMenuAdd_Click(object sender, EventArgs e)
        {
            CharacterFileForm fileForm = new CharacterFileForm();

            if (fileForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (fileForm.FileName.Equals(""))
                {
                    MessageBox.Show("The file must have a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (CurrentMod.CharacterFiles.Contains(fileForm.FileName))
                {
                    MessageBox.Show("The name you entered already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    CurrentMod.CharacterFiles.Add(fileForm.FileName);
                    dataFilesListBox.Items.Add(fileForm.FileName);
                }
            }
        }

        #endregion

    }
}
