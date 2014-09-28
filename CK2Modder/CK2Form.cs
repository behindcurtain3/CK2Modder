﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static readonly String DefaultCultureRoot = "Culture Groups";
        public static readonly String DefaultCharacterListView = "View All Characters";

        public String WorkingLocation { get; set; }
        public Mod CurrentMod { get; set; }
        public Culture SelectedCulture { get; set; }
        public TreeNode SelectedCultureNode { get; set; }
        public Boolean IsCharacterGridListeningForRows { get; set; }

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
            CueProvider.SetCue(characterFilesFilter, "Filter Character Files");
            CueProvider.SetCue(characterFilter, "Filter Characters");

            if(!Directory.Exists(WorkingLocation))
            {
                if (!SelectWorkingLocation())
                {
                    MessageBox.Show("Please select an installation directory for Crusader Kings II.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }

            workingLocationStripStatusLabel.Text = String.Format("Working Location: {0}", WorkingLocation);

            tabControl.Visible = false;
            toolStripProgressBar.Visible = false;

            dynastyBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(dynastyBackgroundWorker_ProgressChanged);
            dynastyBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dynastyBackgroundWorker_RunWorkerCompleted);
            
            cultureBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(cultureBackgroundWorker_RunWorkerCompleted);
            characterBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(characterBackgroundWorker_RunWorkerCompleted);
            
            // Setup dynastyGridView
            dynastyGridView.Visible = false;
            dynastyGridView.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "ID";
            idColumn.HeaderText = "ID";
            idColumn.MinimumWidth = 100;

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.MinimumWidth = 150;

            DataGridViewTextBoxColumn cultureColumn = new DataGridViewTextBoxColumn();
            cultureColumn.DataPropertyName = "Culture";
            cultureColumn.HeaderText = "Culture";
            cultureColumn.MinimumWidth = 150;

            dynastyGridView.Columns.Add(idColumn);
            dynastyGridView.Columns.Add(nameColumn);
            dynastyGridView.Columns.Add(cultureColumn);

            dynastyGridView.CellDoubleClick += new DataGridViewCellEventHandler(dynastyGridView_CellDoubleClick);
            cultureTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(cultureTreeView_NodeMouseClick);
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
                    Dynasty dynasty = DynastyLoader.Load(lines);

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
            toolStripProgressBar.Visible = false;

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

        void dynastyBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = e.ProgressPercentage;
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
                    Culture culture = CultureLoader.Load(lines);

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

            TreeNode root = cultureTreeView.Nodes[0];

            foreach (Culture c in rows)
            {
                TreeNode node = new TreeNode(c.Name);
                node.Tag = c;
                node.ContextMenuStrip = cultureSubContextMenuStrip;

                foreach (Culture sub in c.SubCultures)
                {
                    TreeNode subNode = new TreeNode(sub.Name);
                    subNode.Tag = sub;
                    subNode.ContextMenuStrip = cultureContextMenuStrip;
                    node.Nodes.Add(subNode);
                }

                root.Nodes.Add(node);

                CurrentMod.Cultures.Add(c);
            }

            // Make sure the new tree structure is shown
            root.Expand();

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
                    // attempt to the load the character
                    Character c = CharacterLoader.Load(lines);
                    
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
                UpdateCharacterListBox();
            }
        }

        #endregion

        #region Mod functions

        public void SetCurrentMod(Mod m)
        {
            if (m == null)
            {
                MessageBox.Show("Unable to load the specified mod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CurrentMod = m;

            tabControl.Visible = true;
            dynastyGridView.DataSource = CurrentMod.Dynasties;
            dynastyGridView.Visible = true;
            
            // Add data bindings
            textBoxModName.DataBindings.Add("Text", CurrentMod, "Name");
            userDirectoryTextBox.DataBindings.Add("Text", CurrentMod, "UserDirectory");
            textBoxDependencies.DataBindings.Add("Text", CurrentMod, "Dependencies");
            textBoxModRawOutput.DataBindings.Add("Text", CurrentMod, "RawOutput");
            buttonImportDynasties.DataBindings.Add("Enabled", CurrentMod, "AreDynastiesImported");
            buttonImportCultures.DataBindings.Add("Enabled", CurrentMod, "AreCulturesImported");
            buttonImportCharacters.DataBindings.Add("Enabled", CurrentMod, "AreCharactersImported");
            replacePathsListBox.DataSource = CurrentMod.ReplacePaths;

            // Setup the culture tree
            cultureTreeView.Nodes.Clear();
            TreeNode root = new TreeNode(DefaultCultureRoot);
            root.ContextMenuStrip = cultureRootContextMenuStrip;
            cultureTreeView.Nodes.Add(root);

            // character tab            
            characterFilesListBox.Items.Add(DefaultCharacterListView); // Add the default list value
            characterFilesListBox.SelectedIndex = 0;

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

                        // TODO: add file to list view
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

                        characterFilesListBox.Items.Add(file);
                    }
                }

                // Start loading character files
                if (CurrentMod.CharacterFilesToLoad.Count > 0)
                {
                    StreamReader reader = new StreamReader(CurrentMod.CharacterFilesToLoad.Peek(), Encoding.Default, true);
                    characterBackgroundWorker.RunWorkerAsync(reader);
                }
            }

            CurrentMod.UpdateRawOutput();
            UserPreferences.Default.LastMod = CurrentMod.StorageLocation + "/" + CurrentMod.Name + ".mod";
            UserPreferences.Default.Save();

            saveToolStripMenuItem.Enabled = true;
            closeModToolStripMenuItem.Enabled = true;
            closeWithoutSavingToolStripMenuItem.Enabled = true;
            IsCharacterGridListeningForRows = false;
        }

        public void CloseMod()
        {
            // Menu options
            saveToolStripMenuItem.Enabled = false;
            closeModToolStripMenuItem.Enabled = false;
            closeWithoutSavingToolStripMenuItem.Enabled = false;

            CurrentMod = null;

            // Hide the tabs
            this.tabControl.Visible = false;

            // Remove extra tabs
            while (tabControl.TabCount > 4)
            {
                tabControl.TabPages.RemoveAt(4);
            }

            dynastyGridView.DataSource = null;
            dynastyGridView.Visible = false;

            // Remove data bindings
            textBoxModName.DataBindings.Clear();
            textBoxDependencies.DataBindings.Clear();
            textBoxModRawOutput.DataBindings.Clear();
            buttonImportDynasties.DataBindings.Clear();
            buttonImportCultures.DataBindings.Clear();
            buttonImportCharacters.DataBindings.Clear();
            userDirectoryTextBox.DataBindings.Clear();

            // Reset the culture tree view
            cultureTreeView.Nodes.Clear();
            TreeNode root = new TreeNode(DefaultCultureRoot);
            root.ContextMenuStrip = cultureRootContextMenuStrip;
            cultureTreeView.Nodes.Add(root);
            root.Expand();

            // reset the characters tab
            characterFilesListBox.Items.Clear();
            characterListBox.Items.Clear();            

            cultureInformationGroupBox.Visible = false;
            cultureNamesGroupBox.Visible = false;

            UserPreferences.Default.LastMod = "";
            UserPreferences.Default.Save();
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

        private void ShowDynasty(Dynasty dynasty)
        {
            if (dynasty == null)
                return;
            
            // 
            // tabDynastyInfo
            // 
            TabPage tabDynastyInfo = new TabPage(dynasty.Name);
            tabDynastyInfo.Location = new System.Drawing.Point(4, 22);
            tabDynastyInfo.Name = "tabDynastyInfo";
            tabDynastyInfo.Padding = new System.Windows.Forms.Padding(3);
            tabDynastyInfo.Size = new System.Drawing.Size(773, 484);
            tabDynastyInfo.TabIndex = this.tabControl.TabCount;
            tabDynastyInfo.Text = dynasty.Name + " Dynasty";
            tabDynastyInfo.BackColor = Color.Transparent;

            DynastyEditor editor = new DynastyEditor();
            editor.Dock = System.Windows.Forms.DockStyle.Fill;
            editor.Location = new System.Drawing.Point(3, 3);
            editor.Size = new System.Drawing.Size(767, 478);
            editor.TabIndex = 0;

            tabDynastyInfo.Controls.Add(editor);

            editor.ID.DataBindings.Add("Text", dynasty, "ID");
            editor.DynastyName.DataBindings.Add("Text", dynasty, "Name");
            editor.Culture.DataBindings.Add("Text", dynasty, "Culture");
            editor.Characters.DataSource = CurrentMod.Characters.Where(c => c.Dynasty == dynasty.ID).ToList();

            this.tabControl.TabPages.Add(tabDynastyInfo);
            this.tabControl.SelectedIndex = tabDynastyInfo.TabIndex;

            EventHandler closeHandler = (s, e) => this.tabControl.TabPages.Remove(tabDynastyInfo);
            EventHandler closeHandler2 = (s, e) => this.tabControl.SelectedIndex = 1; // go to dynasties tab
            editor.CloseButton.Click += closeHandler;
            editor.CloseButton.Click += closeHandler2;

            editor.Characters.CellDoubleClick += new DataGridViewCellEventHandler(delegate(object sender, DataGridViewCellEventArgs e)
                {
                    DataGridViewRow row = editor.Characters.Rows[e.RowIndex];

                    //ShowCharacter(row.DataBoundItem as Character);
                });                
        }

        public void SaveMod()
        {
            if (CurrentMod == null)
                return;

            // Write out the mod file
            StreamWriter stream = File.CreateText(CurrentMod.StorageLocation + "/" + CurrentMod.Name + ".mod");
            stream.Write(CurrentMod.RawOutput);
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

        private void PopulateCharacterListBox(List<Character> characters)
        {
            foreach (Character c in characters)
            {
                characterListBox.Items.Add(c.InternalDisplay);
            }
        }

        private void UpdateCharacterListBox()
        {
            List<Character> filteredCharacters = new List<Character>();
            String filter = characterFilter.Text;
            String file = characterFilesListBox.SelectedItem as String;

            // apply the files filter
            if (!characterFilesListBox.SelectedItem.Equals(DefaultCharacterListView))
            {
                // Filter the files first
                filteredCharacters = CurrentMod.Characters.Where(c => c.BelongsTo.Equals(file)).ToList();
            }
            else
            {
                filteredCharacters.AddRange(CurrentMod.Characters);
            }

            // check if the filter is an ID
            int ID = Helpers.ParseInt(filter);
            if (ID != -1)
            {
                filteredCharacters = filteredCharacters.Where(c => c.ID.ToString().Contains(ID.ToString())).ToList();
            }
            // filter for names
            else
            {
                // filter for the string passed in
                if (!String.IsNullOrEmpty(filter) && !String.IsNullOrWhiteSpace(filter))
                {
                    filteredCharacters = filteredCharacters.Where(c => c.Name.ToLower().Contains(filter)).ToList();
                }
            }

            // Clear the list
            characterListBox.Items.Clear();

            // Populate it
            PopulateCharacterListBox(filteredCharacters);
        }

        #endregion

        #region Events

        private void characterFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateCharacterListBox();
        }

        private void characterListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            String selected = characterListBox.SelectedItem as String;

            if (selected == null)
                return;

            int ID = Helpers.ParseInt(selected);
            
            if (ID == -1)
                return;

            // show the character
            characterPropertyGrid.SelectedObject = CurrentMod.Characters.Find(c => c.ID == ID);
        }

        private void characterFilesFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void characterFilter_TextChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            UpdateCharacterListBox();
        }

        void cultureTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectedCultureNode = e.Node;

            // Don't do this on a right-click
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                return;

            String nodeName = e.Node.Text;

            if (nodeName.Equals(DefaultCultureRoot))
            {
                cultureInformationGroupBox.Visible = false;
                cultureNamesGroupBox.Visible = false;
                return;
            }

            Culture selectedCulture = CurrentMod.Cultures.Find(delegate(Culture c) { return c.Name.Equals(nodeName); });
            if (selectedCulture == null)
            {
                Culture searchForMe;
                // search the subcultures
                foreach (Culture culture in CurrentMod.Cultures)
                {
                    searchForMe = culture.SubCultures.Find(delegate(Culture c) { return c.Name.Equals(nodeName); });
                    if (searchForMe != null)
                    {
                        selectedCulture = searchForMe;
                        break;
                    }
                }
            }

            SelectedCulture = selectedCulture;

            if (selectedCulture != null)
            {
                cultureInformationGroupBox.Visible = true;
                cultureNamesGroupBox.Visible = true;

                // isCulture will be true if this is a culture, false if a culture group
                Boolean isCulture = (selectedCulture.SubCultures.Count == 0 && !SelectedCultureNode.Parent.Text.Equals(DefaultCultureRoot));

                cultureNameTextBox.Text = selectedCulture.Name;
                cultureGfxTextBox.Text = selectedCulture.Graphical_Culture;
                cultureColorTextBox.Text = selectedCulture.Color;
                cultureMaleNamesRichTextBox.Text = selectedCulture.MaleNames;
                cultureFemaleNamesRichTextBox.Text = selectedCulture.FemaleNames;
                cultureDynastyPrefixTextBox.Text = selectedCulture.DynastyPrefix;
                cultureBastardTextBox.Text = selectedCulture.BastardPrefix;
                cultureModifierTextBox.Text = selectedCulture.Modifier;
                cultureMalePatronymTextBox.Text = selectedCulture.MalePatronym;
                cultureFemalePatronymTextBox.Text = selectedCulture.FemalePatronym;
                cultureSuffixCheckBox.Checked = selectedCulture.IsSuffix;

                // Name chances
                culturePatGFTextBox.Text = selectedCulture.PaternalGrandFather.ToString();
                cultureMatGFTextBox.Text = selectedCulture.MaternalGrandFather.ToString();
                cultureFatherTextBox.Text = selectedCulture.Father.ToString();

                culturePatGMTextBox.Text = selectedCulture.PaternalGrandMother.ToString();
                cultureMatGMTextBox.Text = selectedCulture.MaternalGrandMother.ToString();
                cultureMotherTextBox.Text = selectedCulture.Mother.ToString();

                // enable or disable the appropriate fields
                cultureColorTextBox.Enabled = isCulture;
                cultureMaleNamesRichTextBox.Enabled = isCulture;
                cultureFemaleNamesRichTextBox.Enabled = isCulture;
                cultureDynastyPrefixTextBox.Enabled = isCulture;
                cultureBastardTextBox.Enabled = isCulture;
                cultureModifierTextBox.Enabled = isCulture;
                cultureMalePatronymTextBox.Enabled = isCulture;
                cultureFemalePatronymTextBox.Enabled = isCulture;
                cultureSuffixCheckBox.Enabled = isCulture;
                culturePatGFTextBox.Enabled = isCulture;
                culturePatGMTextBox.Enabled = isCulture;
                cultureMatGFTextBox.Enabled = isCulture;
                cultureMatGMTextBox.Enabled = isCulture;
                cultureFatherTextBox.Enabled = isCulture;
                cultureMotherTextBox.Enabled = isCulture;

                // data bindings
                cultureNameTextBox.DataBindings.Clear();
                cultureNameTextBox.DataBindings.Add("Text", selectedCulture, "Name");

                cultureGfxTextBox.DataBindings.Clear();
                cultureGfxTextBox.DataBindings.Add("Text", selectedCulture, "Graphical_Culture");

                cultureColorTextBox.DataBindings.Clear();
                cultureColorTextBox.DataBindings.Add("Text", selectedCulture, "Color");

                cultureMaleNamesRichTextBox.DataBindings.Clear();
                cultureMaleNamesRichTextBox.DataBindings.Add("Text", selectedCulture, "MaleNames");

                cultureFemaleNamesRichTextBox.DataBindings.Clear();
                cultureFemaleNamesRichTextBox.DataBindings.Add("Text", selectedCulture, "FemaleNames");

                cultureDynastyPrefixTextBox.DataBindings.Clear();
                cultureDynastyPrefixTextBox.DataBindings.Add("Text", selectedCulture, "DynastyPrefix");

                cultureBastardTextBox.DataBindings.Clear();
                cultureBastardTextBox.DataBindings.Add("Text", selectedCulture, "BastardPrefix");

                cultureMalePatronymTextBox.DataBindings.Clear();
                cultureMalePatronymTextBox.DataBindings.Add("Text", selectedCulture, "MalePatronym");

                cultureFemalePatronymTextBox.DataBindings.Clear();
                cultureFemalePatronymTextBox.DataBindings.Add("Text", selectedCulture, "FemalePatronym");

                cultureSuffixCheckBox.DataBindings.Clear();
                cultureSuffixCheckBox.DataBindings.Add("Checked", selectedCulture, "IsSuffix");

                culturePatGFTextBox.DataBindings.Clear();
                culturePatGFTextBox.DataBindings.Add("Text", selectedCulture, "PaternalGrandFather");

                cultureMatGFTextBox.DataBindings.Clear();
                cultureMatGFTextBox.DataBindings.Add("Text", selectedCulture, "MaternalGrandFather");

                cultureFatherTextBox.DataBindings.Clear();
                cultureFatherTextBox.DataBindings.Add("Text", selectedCulture, "Father");

                culturePatGMTextBox.DataBindings.Clear();
                culturePatGMTextBox.DataBindings.Add("Text", selectedCulture, "PaternalGrandMother");

                cultureMatGMTextBox.DataBindings.Clear();
                cultureMatGMTextBox.DataBindings.Add("Text", selectedCulture, "MaternalGrandMother");

                cultureMotherTextBox.DataBindings.Clear();
                cultureMotherTextBox.DataBindings.Add("Text", selectedCulture, "Mother");
            }
        }

        void dynastyGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dynastyGridView.Rows.Count)
                return;

            DataGridViewRow row = dynastyGridView.Rows[e.RowIndex];

            ShowDynasty(row.DataBoundItem as Dynasty);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMod();
        }

        private void modToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void closeModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMod();
            CloseMod();
        }

        private void closeWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseMod();
        }

        private void buttonImportDynasties_Click(object sender, EventArgs e)
        {
            ImportAllVanillaDynasties();
            CurrentMod.AreDynastiesImported = false;
        }

        private void workingLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectWorkingLocation();
        }

        private void buttonDynastyFilterByID_Click(object sender, EventArgs e)
        {
            BindingList<Dynasty> filteredList = new BindingList<Dynasty>();
            BindingList<Dynasty> idList = null;
            BindingList<Dynasty> nameList = null;
            BindingList<Dynasty> cultureList = null;
            if (!textBoxDynastyFilterByID.Text.Equals(""))
            {
                try
                {
                    int id = Int32.Parse(textBoxDynastyFilterByID.Text);
                    idList = new BindingList<Dynasty>(CurrentMod.Dynasties.Where(m => m.ID == id).ToList());
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("The ID entered must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (!textBoxDynastyFilterByName.Text.Equals(""))
            {
                nameList = new BindingList<Dynasty>(CurrentMod.Dynasties.Where(m => m.Name.ToLower().Contains(textBoxDynastyFilterByName.Text.ToLower()) == true).ToList());
            }
            if (!textBoxDynastyFilterByCulture.Text.Equals(""))
            {
                cultureList = new BindingList<Dynasty>(CurrentMod.Dynasties.Where(m => m.Culture.ToLower().Contains(textBoxDynastyFilterByCulture.Text.ToLower()) == true).ToList());
            }

            if (idList != null)
            {
                foreach (Dynasty d in idList)
                    filteredList.Add(d);
            }

            if (nameList != null)
            {
                foreach (Dynasty d in nameList)
                {
                    if (!filteredList.Contains(d))
                        filteredList.Add(d);
                }
            }

            if (cultureList != null)
            {
                foreach (Dynasty d in cultureList)
                {
                    if (!filteredList.Contains(d))
                        filteredList.Add(d);
                }
            }

            dynastyGridView.AllowUserToAddRows = false;
            dynastyGridView.AllowUserToDeleteRows = false;
            dynastyGridView.DataSource = filteredList;
        }

        private void buttonDynastyClearFilter_Click(object sender, EventArgs e)
        {
            textBoxDynastyFilterByID.Text = "";
            textBoxDynastyFilterByName.Text = "";
            textBoxDynastyFilterByCulture.Text = "";

            dynastyGridView.DataSource = CurrentMod.Dynasties;

            dynastyGridView.AllowUserToAddRows = true;
            dynastyGridView.AllowUserToDeleteRows = true;
        }

        private void textBoxDynastyFilterByID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
                buttonDynastyFilter.PerformClick();
        }

        private void textBoxDynastyFilterByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                buttonDynastyFilter.PerformClick();
        }

        private void textBoxDynastyFilterByCulture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                buttonDynastyFilter.PerformClick();
        }

        /// <summary>
        /// Removes a culture from the tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cultureToolStripMenuRemove_Click(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            if (SelectedCultureNode != null)
            {
                foreach (Culture group in CurrentMod.Cultures)
                {
                    int count = group.SubCultures.RemoveAll(delegate(Culture culture) { return culture.Name.Equals(SelectedCultureNode.Text); });

                    if (count > 0)
                    {
                        SelectedCultureNode.Remove();
                        
                        // Hide the editing panes
                        cultureInformationGroupBox.Visible = false;
                        cultureNamesGroupBox.Visible = false;                        
                    }
                }
            }
        }

        /// <summary>
        /// Removes a culture group from the tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cultureStripRemove_Click(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            if (SelectedCultureNode != null)
            {
                if (MessageBox.Show("This will also remove all the cultures attached to this culture group. Are you sure you wish to continue?",
                    "Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    int count = CurrentMod.Cultures.RemoveAll(delegate(Culture culture) { return culture.Name.Equals(SelectedCultureNode.Text); });

                    if (count > 0)
                    {
                        SelectedCultureNode.Remove();

                        // Hide the editing panes
                        cultureInformationGroupBox.Visible = false;
                        cultureNamesGroupBox.Visible = false;
                    }
                }                
            }
        }

        /// <summary>
        /// Add a new culture to a culture group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cultureStripAdd_Click(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            if (SelectedCultureNode != null)
            {
                Culture group = CurrentMod.Cultures.Find(delegate(Culture c) { return c.Name.Equals(SelectedCultureNode.Text); });

                if (group != null)
                {

                    Random rand = new Random(DateTime.Now.Millisecond);

                    Culture newCulture = new Culture();
                    newCulture.Name = "culture_" + rand.Next(10000, 99999);

                    TreeNode node = new TreeNode(newCulture.Name);
                    node.Tag = newCulture;
                    node.ContextMenuStrip = cultureContextMenuStrip;

                    group.SubCultures.Add(newCulture);
                    SelectedCultureNode.Nodes.Add(node);
                    SelectedCultureNode.Expand();
                }

            }
        }

        /// <summary>
        /// Add a new culture group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cultureGroupStripMenuAdd_Click(object sender, EventArgs e)
        {
            if (CurrentMod == null || SelectedCultureNode == null)
                return;            

            Random rand = new Random(DateTime.Now.Millisecond);

            Culture newCulture = new Culture();
            newCulture.Name = "culture_group_" + rand.Next(10000, 99999);

            TreeNode node = new TreeNode(newCulture.Name);
            node.Tag = newCulture;
            node.ContextMenuStrip = cultureSubContextMenuStrip;

            CurrentMod.Cultures.Add(newCulture);
            SelectedCultureNode.Nodes.Add(node);
            SelectedCultureNode.Expand();

        }

        private void buttonImportCultures_Click(object sender, EventArgs e)
        {
            ImportAllVanillaCultures();
            CurrentMod.AreCulturesImported = false;
        }

        private void buttonImportCharacters_Click(object sender, EventArgs e)
        {
            ImportAllVanillaCharacters();
            CurrentMod.AreCharactersImported = false;
        }        

        private void cultureNameTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox name = sender as TextBox;

            if (SelectedCultureNode.Text.Equals(DefaultCultureRoot))
                return;

            SelectedCultureNode.Text = name.Text;
        }

        private void deleteSelectedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            String selectedFile = characterFilesListBox.SelectedItem as String;

            if (selectedFile == null || selectedFile.Equals(DefaultCharacterListView))
                return;

            if(MessageBox.Show("Deleting the file will delete all the characters in the file. Do you wish to continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
            {
                String originalFile = WorkingLocation + "/" + CurrentMod.Path + VanillaCharactersPath + selectedFile + ".txt";

                if (File.Exists(originalFile))
                {
                    File.Delete(originalFile);
                }

                characterFilesListBox.Items.Remove(selectedFile);
                CurrentMod.CharacterFiles.Remove(selectedFile);

                foreach (Character c in CurrentMod.Characters.ToList())
                    if (c.BelongsTo.Equals(selectedFile))
                        CurrentMod.Characters.Remove(c);

                // Set the view to all characters
                characterFilesListBox.SelectedIndex = 0;
            }
        }

        private void editSelectedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String selected = characterFilesListBox.SelectedItem as String;

            if (selected == null || selected.Equals(DefaultCharacterListView))
                return;

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
                        characterFilesListBox.Items.Remove(selected);
                        characterFilesListBox.Items.Add(fileForm.FileName);

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
                    characterFilesListBox.Items.Add(fileForm.FileName);
                }
            }
        }

        private void addPathButton_Click(object sender, EventArgs e)
        {
            if(!replacePathsComboBox.Text.Equals(""))
            {
                CurrentMod.ReplacePaths.Add(replacePathsComboBox.Text);
                replacePathsComboBox.Text = "";
                CurrentMod.UpdateRawOutput();
            }
        }

        private void replacePathsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                CurrentMod.ReplacePaths.RemoveAt(replacePathsListBox.SelectedIndex);
                CurrentMod.UpdateRawOutput();
            }
        }

        private void replacePathsComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addPathButton.PerformClick();
        }

        #endregion

        #region Import functions

        private void ImportAllVanillaCultures()
        {
            String absolutePath = WorkingLocation + VanillaCulturesFile;

            if (File.Exists(absolutePath))
            {
                StreamReader reader = new StreamReader(absolutePath, Encoding.Default, true);

                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Maximum = 100;

                cultureBackgroundWorker.RunWorkerAsync(reader);
            }
        }

        private void ImportAllVanillaDynasties()
        {
            String absolutePath = WorkingLocation + VanillaDynastyFile;

            if (File.Exists(absolutePath))
            {
                StreamReader reader = new StreamReader(absolutePath, Encoding.Default, true);

                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Maximum = 100;

                dynastyBackgroundWorker.RunWorkerAsync(reader);
            }
        }

        private void ImportAllVanillaCharacters()
        {
            String absolutePath = WorkingLocation + VanillaCharactersPath;

            if (Directory.Exists(absolutePath))
            {
                foreach (String file in Directory.GetFiles(absolutePath))
                {
                    String fileName = Path.GetFileNameWithoutExtension(file);

                    if (!CurrentMod.CharacterFiles.Contains(fileName))
                    {
                        CurrentMod.CharacterFiles.Add(fileName);
                        CurrentMod.CharacterFilesToLoad.Enqueue(file);

                        characterFilesListBox.Items.Add(Path.GetFileNameWithoutExtension(fileName));
                    } 
                }

                if (!characterBackgroundWorker.IsBusy)
                {
                    StreamReader reader = new StreamReader(CurrentMod.CharacterFilesToLoad.Peek(), Encoding.Default, true);
                    characterBackgroundWorker.RunWorkerAsync(reader);
                }
            }
        }

        #endregion
       
    }
}
