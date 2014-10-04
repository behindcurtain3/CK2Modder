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
        public static readonly String VanillaDynastiesPath = "/common/dynasties/";
        public static readonly String VanillaCharactersPath = "/history/characters/";
        public static readonly String VanillaCulturesPath = "/common/cultures/";
        public static readonly String DefaultWindowTitle = "CK2 Modder";

        public Mod CurrentMod { get; set; }
        public String DataMode { get; set; }
        
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
            InitializeComponent();

            // do this here so in the design view it can stay at the back
            modClosedPanel.BringToFront();

            // Setup the cue's on textboxes
            CueProvider.SetCue(dataFilter, "Filter Data");

            // show lines numbers
            dataTextEditor.Margins[0].Width = 20;
            dataTextEditor.ConfigurationManager.Language = "python";
            dataTextEditor.ConfigurationManager.Configure();
            dataTextEditor.Caret.HighlightCurrentLine = true;
            dataTextEditor.Caret.CurrentLineBackgroundColor = System.Drawing.Color.DarkBlue;
            dataTextEditor.Caret.CurrentLineBackgroundAlpha = 64;
            dataTextEditor.MatchBraces = true;

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
                        dynasty.BelongsTo = Path.GetFileName(CurrentMod.DynastyFilesToLoad.Peek());
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
                        culture.BelongsTo = Path.GetFileName(CurrentMod.CultureFilesToLoad.Peek());
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
                        c.BelongsTo = Path.GetFileName(CurrentMod.CharacterFilesToLoad.Peek());
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

            // Load the dynasties
            String dynastiesFolder = CurrentMod.ModRootDirectory + VanillaDynastiesPath;
            if(Directory.Exists(dynastiesFolder))
            {
                // Go through each file and add it to the queue
                String[] files = Directory.GetFiles(dynastiesFolder);

                foreach (String filePath in files)
                {
                    String file = Path.GetFileName(filePath);

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
                    String file = Path.GetFileName(filePath);

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
                    String file = Path.GetFileName(filePath);

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
            UserPreferences.Default.WorkingLocation = CurrentMod.StorageLocation;
            UserPreferences.Default.Save();

            saveToolStripMenuItem.Enabled = true;
            closeModToolStripMenuItem.Enabled = true;
            closeWithoutSavingToolStripMenuItem.Enabled = true;
            
            // populate file tree
            PopulateTreeView();

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

            // reset file tree
            modFilesTree.Nodes.Clear();
            
            // reset editor
            dataTextEditor.DataBindings.Clear();
            dataTextEditor.Text = "";

            // reset the filter
            dataFilter.Text = "";

            // reset the data list
            dataListBox.Items.Clear();

            UserPreferences.Default.LastMod = "";
            UserPreferences.Default.Save();

            // Show the panel
            modClosedPanel.Visible = true;

            this.Text = DefaultWindowTitle;
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
                    stream = new StreamWriter(CurrentMod.ModRootDirectory + VanillaDynastiesPath + "/" + name, false, Encoding.Default);

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
                    stream = new StreamWriter(CurrentMod.ModRootDirectory + VanillaCulturesPath + "/" + name, false, Encoding.Default);

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
                    stream = new StreamWriter(CurrentMod.ModRootDirectory + VanillaCharactersPath + "/" + name, false, Encoding.Default);

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

            // write out any misc files that were loaded
            foreach (MiscFile file in CurrentMod.Files)
            {
                // open the stream
                stream = new StreamWriter(file.FileInfo.OpenWrite(), Encoding.Default);

                // write the data
                stream.Write(file.ToString());

                // close the stream
                stream.Close();
            }
        }

        private void PopulateDataListBox(List<String> list)
        {
            dataListBox.Items.AddRange(list.ToArray());
        }

        private void UpdateDataListBox(String mode, String fileFilter = null)
        {
            String filter = dataFilter.Text;
            List<String> list = new List<String>();

            switch (mode)
            {
                case "characters":
                    List<Character> characters = new List<Character>();
                    
                    // apply the files filter
                    if (!String.IsNullOrWhiteSpace(fileFilter))
                    {
                        // Filter the files first
                        characters = CurrentMod.Characters.FindAll(delegate(Character c) { return c.BelongsTo.Equals(fileFilter); });
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

                case "dynasties":
                    List<Dynasty> dynasties = new List<Dynasty>();
                    dynasties.AddRange(CurrentMod.Dynasties);

                    // apply the files filter
                    if (!String.IsNullOrWhiteSpace(fileFilter))
                    {
                        // Filter the files first
                        dynasties = CurrentMod.Dynasties.FindAll(delegate(Dynasty d) { return d.BelongsTo.Equals(fileFilter); });
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

                case "cultures":
                    List<Culture> cultures = new List<Culture>();
                    cultures.AddRange(CurrentMod.Cultures);

                    // apply the files filter
                    if (!String.IsNullOrWhiteSpace(fileFilter))
                    {
                        // Filter the files first
                        cultures = cultures.FindAll(delegate(Culture c) { return c.BelongsTo.Equals(fileFilter); });
                    }

                    // filter for the string passed in
                    if (!String.IsNullOrWhiteSpace(filter))
                    {
                        cultures = cultures.FindAll(delegate(Culture c) { return c.Name.ToLower().Contains(filter); });
                    }

                    foreach (Culture c in cultures)
                        list.Add(c.Display);
                    break;
            }

            // clear out the list box
            dataListBox.Items.Clear();

            // Populate it
            PopulateDataListBox(list);
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
            if (String.IsNullOrWhiteSpace(UserPreferences.Default.WorkingLocation))
            {
                newModDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                newModDialog.InitialDirectory = UserPreferences.Default.WorkingLocation;
            } 
            newModDialog.AddExtension = true;
            newModDialog.DefaultExt = ".mod";
            newModDialog.FileName = "Mod Name";
            newModDialog.Filter = "Mod File (*.mod)|*.mod";
            newModDialog.Title = "Select a save location and name for the mod";

            if (newModDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (CurrentMod != null)
                    CloseMod();

                // TODO: delete and remake the mod directory if it already exits to give the new mod a fresh start?
                Mod mod = new Mod(Path.GetFileNameWithoutExtension(newModDialog.FileName));
                
                // setup the initial mod data
                mod.StorageLocation = Path.GetDirectoryName(newModDialog.FileName);
                mod.ModRootDirectory = mod.StorageLocation + "/" + mod.Name;
                mod.Raw = "name = \"" + mod.Name + "\"" + System.Environment.NewLine;
                mod.Raw += "path = \"mod/" + mod.Name + "\"" + System.Environment.NewLine;

                // Set the new mod as current
                SetCurrentMod(mod);
            }
        }

        private void LoadMod()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Mod File (*.mod)|*.mod";
            if (String.IsNullOrWhiteSpace(UserPreferences.Default.WorkingLocation))
            {
                openFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                openFileDialog.InitialDirectory = UserPreferences.Default.WorkingLocation;
            }
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

            if(dataListBox.Items.Count > 0)
                dataListBox.SelectedIndex = 0;
        }

        private void HideSplitPanels()
        {   
            secondarySplitPanel.Panel1Collapsed = true;
            secondarySplitPanel.Panel1.Hide();
        }

        private void PopulateTreeView()
        {
            if (CurrentMod == null)
                return;

            // clear the tree
            modFilesTree.Nodes.Clear();

            // Add the mod file as the top entry
            TreeNode modFile = new TreeNode(CurrentMod.Name + ".mod");
            modFile.Tag = CurrentMod;
            modFile.ImageIndex = 1;
            modFile.SelectedImageIndex = 1;
            modFilesTree.Nodes.Add(modFile);

            // Loop through all the directories in the mod path
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(@CurrentMod.ModRootDirectory);
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                modFilesTree.Nodes.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageIndex = 0;
                //aNode.ContextMenuStrip = treeMenuStrip;
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);

                // Get any files
                FileInfo[] files = subDir.GetFiles("*.txt");
                foreach (FileInfo file in files)
                {
                    TreeNode fileNode = new TreeNode(file.Name, 0, 0);
                    fileNode.Tag = file;
                    fileNode.ImageIndex = 1;
                    fileNode.SelectedImageIndex = 1;
                    aNode.Nodes.Add(fileNode);
                }
            }
        }

        #endregion

        #region Events

        private void dataFilter_TextChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateDataListBox(modFilesTree.SelectedNode.Text);
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
            switch (DataMode)
            {
                case "characters":
                    resource = CurrentMod.Characters.Find(c => c.ID == ID);
                    break;

                case "dynasties":
                    resource = CurrentMod.Dynasties.Find(d => d.ID == ID);
                    break;

                case "cultures":
                    resource = CurrentMod.Cultures.Find(c => c.Name.Equals(name));
                    break;
            }

            UpdateTextEditor(resource);
        }


        private void modFilesTree_DoubleClick(object sender, EventArgs e)
        {
            // check if the selection is a directory
            if (modFilesTree.SelectedNode.Tag is DirectoryInfo)
            {
                // if a directory is selected, attempt to refresh the dataListBox with relevant information
                DirectoryInfo dir = modFilesTree.SelectedNode.Tag as DirectoryInfo;

                // pass the directory name
                UpdateDataListBox(dir.Name);
            }
            // check if the selection is a file
            else if (modFilesTree.SelectedNode.Tag is FileInfo)
            {
                FileInfo file = modFilesTree.SelectedNode.Tag as FileInfo;

                // try to retrieve the selected file
                MiscFile data = CurrentMod.Files.Find(delegate(MiscFile f) { return f.BelongsTo.Equals(file.Name); });

                // if it exists load it in the editor
                if (data != null)
                {
                    UpdateTextEditor(data);
                }
                // otherwise load the file
                else
                {
                    List<String> lines = new List<string>();
                    StreamReader reader = new StreamReader(file.FullName);
                    String line;
                    while((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    data = MiscFile.Load(lines);

                    if(data != null)
                    {
                        data.BelongsTo = file.Name;
                        data.FileInfo = file;

                        CurrentMod.Files.Add(data);
                        UpdateTextEditor(data);
                    }
                }
            }
            // otherwise the selection if the .mod file at the top
            else if (modFilesTree.SelectedNode.Tag is Mod)
            {
                // if a mod is selected open it in the text editor
                UpdateTextEditor(CurrentMod);
            }
        }


        private void modFilesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is DirectoryInfo)
            {
                // if a directory is selected, attempt to refresh the dataListBox with relevant information
                DirectoryInfo dir = e.Node.Tag as DirectoryInfo;

                // pass the directory name
                UpdateDataListBox(dir.Name);

                DataMode = dir.Name;
            }
            else if (e.Node.Tag is FileInfo)
            {
                FileInfo file = e.Node.Tag as FileInfo;
                String dir = file.Directory.Name;

                // update the list view
                UpdateDataListBox(dir, file.Name);

                DataMode = dir;
            }
            // otherwise the selection if the .mod file at the top
            else if (e.Node.Tag is Mod)
            {
                // if a mod is selected open it in the text editor
                UpdateTextEditor(CurrentMod);
            }
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


        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Add file to the selected folder
            DirectoryInfo directory = modFilesTree.SelectedNode.Tag as DirectoryInfo;

            String fileName = "New File.txt";

            MiscFile file = new MiscFile();
            file.BelongsTo = directory.FullName + "/" + fileName;

            // Write a blank file
            StreamWriter stream = File.CreateText(file.BelongsTo);
            stream.Close();

            PopulateTreeView();
        }

        /// <summary>
        /// Ensure whenever a user right-clicks on the treeview the context menu
        /// is able to use the node clicked on instead of the selected node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modFilesTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                modFilesTree.SelectedNode = modFilesTree.GetNodeAt(e.X, e.Y);

                if (modFilesTree.SelectedNode != null)
                {
                    treeMenuStrip.Show(modFilesTree, e.Location);
                }
            }
        }

        #endregion
        
    }
}
