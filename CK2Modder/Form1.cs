using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using CK2Modder.GameData.common;
using CK2Modder.GameData.history.characters;

namespace CK2Modder
{
    public partial class Form1 : Form
    {
        public static readonly String SteamDirectory = "C:\\Program Files\\Steam\\steamapps\\common\\crusader kings ii";
        public static readonly String SteamDirectoryX86 = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\crusader kings ii";

        public static readonly String VanillaDynastyFile = "/common/dynasties.txt";
        public static readonly String VanillaCulturesFile = "/common/cultures.txt";

        public String WorkingLocation { get; set; }
        public Mod CurrentMod { get; set; }
        public Culture SelectedCulture { get; set; }
        public TreeNode SelectedCultureNode { get; set; }

        #region Initialization

        public Form1(string filename)
        {
            Initialize();

            // Attempt to load the mod passed in
            if (File.Exists(filename))
            {
                SetCurrentMod(Mod.LoadFromFile(filename));
            }

        }

        public Form1()
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

            while (!Directory.Exists(WorkingLocation))
                SelectWorkingLocation();

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

            // Setup characterGridView
            characterGridView.AutoGenerateColumns = false;

            idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "ID";
            idColumn.HeaderText = "ID";
            idColumn.MinimumWidth = 50;

            nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.MinimumWidth = 150;

            cultureColumn = new DataGridViewTextBoxColumn();
            cultureColumn.DataPropertyName = "Culture";
            cultureColumn.HeaderText = "Culture";
            cultureColumn.MinimumWidth = 75;

            DataGridViewTextBoxColumn dynastyColumn = new DataGridViewTextBoxColumn();
            dynastyColumn.DataPropertyName = "Dynasty";
            dynastyColumn.HeaderText = "Dynasty";
            dynastyColumn.MinimumWidth = 50;

            DataGridViewTextBoxColumn religionColumn = new DataGridViewTextBoxColumn();
            religionColumn.DataPropertyName = "Religion";
            religionColumn.HeaderText = "Religion";
            religionColumn.MinimumWidth = 50;

            characterGridView.Columns.Add(idColumn);
            characterGridView.Columns.Add(nameColumn);
            characterGridView.Columns.Add(dynastyColumn);
            characterGridView.Columns.Add(religionColumn);
            characterGridView.Columns.Add(cultureColumn);

            dynastyGridView.CellDoubleClick += new DataGridViewCellEventHandler(dynastyGridView_CellDoubleClick);
            cultureTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(cultureTreeView_NodeMouseClick);
        }

        #endregion

        #region Background Workers

        private void dynastyBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            // Store the ids
            List<Dynasty> loadingDynasties = new List<Dynasty>();

            BackgroundWorker worker = sender as BackgroundWorker;
            StreamReader stream = e.Argument as StreamReader;

            String line;

            while ((line = stream.ReadLine()) != null)
            {
                // Start of dynasty
                if (line.EndsWith("= {") && !line.StartsWith("#"))
                {
                    Dynasty dynasty = new Dynasty();
                    dynasty.ID = Int32.Parse(Regex.Match(line, @"\d+").Value);

                    int openingBrackets = 1;
                    int closingBrackets = 0;
                    int colorsFound = 0;

                    String insideLine;

                    do
                    {
                        insideLine = stream.ReadLine();

                        if (insideLine.Contains("{"))
                            openingBrackets++;
                        else if (insideLine.Contains("}"))
                            closingBrackets++;

                        // Add the name
                        if (insideLine.Contains("name"))
                        {
                            int start = insideLine.IndexOf('"') + 1;
                            int end = insideLine.IndexOf('"', start);
                            dynasty.Name = insideLine.Substring(start, end - start);
                        }

                        // Add the culture
                        else if (insideLine.Contains("culture"))
                        {
                            // Cultures are added in two ways with "" or without
                            if (insideLine.Contains('"'))
                            {
                                int start = insideLine.IndexOf('"') + 1;
                                int end = insideLine.IndexOf('"', start);
                                dynasty.Culture = insideLine.Substring(start, end - start);
                            }
                            else
                            {
                                dynasty.Culture = insideLine.Substring(insideLine.IndexOf("=") + 1).Trim();
                            }
                        }

                        // Add the coat of arms
                        else if (insideLine.Contains("coat_of_arms"))
                        {
                            dynasty.COA = new CoatOfArms();                            
                        }

                        // COA template
                        else if (insideLine.Contains("template"))
                        {
                            dynasty.COA.Template = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }

                        // COA Texture_Internal
                        // We need to check for this before "texture" because we are using contains
                        else if (insideLine.Contains("texture_internal"))
                        {
                            dynasty.COA.Layer.Texture_Internal = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }

                        // COA Texture
                        else if (insideLine.Contains("texture"))
                        {
                            dynasty.COA.Layer.Texture = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }                       

                        // COA Emblem
                        else if (insideLine.Contains("emblem"))
                        {
                            dynasty.COA.Layer.Emblem = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }

                        // COA Color1
                        else if (insideLine.Contains("color"))
                        {
                            if(colorsFound == 0)
                                dynasty.COA.Layer.R = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                            else if (colorsFound == 1)
                                dynasty.COA.Layer.G = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                            else if (colorsFound == 2)
                                dynasty.COA.Layer.B = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);

                            colorsFound++;
                        }

                    } while (closingBrackets < openingBrackets);


                    // Add the row to our list
                    loadingDynasties.Add(dynasty);

                    // Update the progress
                    progress += 10;
                    if(progress > 100)
                        progress = 100;

                    worker.ReportProgress(progress);
                }               
            }

            stream.Close();
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

            String line;

            while ((line = stream.ReadLine()) != null)
            {
                // Start of dynasty
                if (line.EndsWith("= {") && !line.StartsWith("#"))
                {
                    Culture culture = new Culture();
                    culture.Name = line.Remove(line.IndexOf(" = {"));

                    int openingBrackets = 1;
                    int closingBrackets = 0;

                    String insideLine;

                    do
                    {
                        insideLine = stream.ReadLine();

                        if (insideLine.Contains("{"))
                            openingBrackets++;
                        if (insideLine.Contains("}"))
                            closingBrackets++;

                        // Add the name
                        if (insideLine.Contains("graphical_culture"))
                        {
                            culture.Graphical_Culture = insideLine.Substring(insideLine.IndexOf("=") + 1).Trim();
                        }

                        // Load subcultures
                        if (insideLine.Contains("= {") && !insideLine.StartsWith("#") && !insideLine.Contains("male_names") && !insideLine.Contains("female_names"))
                        {
                            Culture subCulture = new Culture();
                            subCulture.Name = insideLine.Remove(insideLine.IndexOf(" = {"));

                            int openBrackets = 1;
                            int closeBrackets = 0;
                            String subCultureLine;
                            do
                            {
                                subCultureLine = stream.ReadLine();

                                if (subCultureLine.Contains("{"))
                                    openBrackets++;
                                if (subCultureLine.Contains("}"))
                                    closeBrackets++;

                                // Add the name
                                if (subCultureLine.Contains("graphical_culture"))
                                {
                                    subCulture.Graphical_Culture = subCultureLine.Substring(subCultureLine.IndexOf("=") + 1).Trim();
                                }
                                else if (subCultureLine.Contains("color"))
                                {
                                    int start = subCultureLine.IndexOf("{") + 2;
                                    int end = subCultureLine.IndexOf("}") - 1;
                                    int length = end - start;

                                    subCulture.Color = subCultureLine.Substring(start, length);
                                }

                                // Add the 2nd condition otherwise this would pick up both "male_names" and "female_names"
                                else if (subCultureLine.Contains("male_names") && !subCultureLine.Contains("fe"))
                                {
                                    String nameLine = stream.ReadLine();
                                    while (!nameLine.Contains("}"))
                                    {
                                        if (!nameLine.Trim().Equals(""))
                                        {
                                            if (subCulture.MaleNames.Equals(""))
                                                subCulture.MaleNames += nameLine.Trim();
                                            else
                                                subCulture.MaleNames += "\r\n" + nameLine.Trim();
                                        }
                                        nameLine = stream.ReadLine();
                                    }
                                    closeBrackets++;
                                }
                                else if (subCultureLine.Contains("female_names"))
                                {
                                    String nameLine = stream.ReadLine();
                                    while (!nameLine.Contains("}"))
                                    {
                                        if (!nameLine.Trim().Equals(""))
                                        {
                                            if (subCulture.FemaleNames.Equals(""))
                                                subCulture.FemaleNames += nameLine.Trim();
                                            else
                                                subCulture.FemaleNames += "\r\n" + nameLine.Trim();
                                        }
                                        nameLine = stream.ReadLine();
                                    }
                                    closeBrackets++;
                                }
                                else if (subCultureLine.Contains("from_dynasty_prefix"))
                                {
                                    int start = subCultureLine.IndexOf('"') + 1;
                                    int end = subCultureLine.IndexOf('"', start);
                                    subCulture.DynastyPrefix = subCultureLine.Substring(start, end - start);
                                }
                                else if (subCultureLine.Contains("bastard_dynasty_prefix"))
                                {
                                    int start = subCultureLine.IndexOf('"') + 1;
                                    int end = subCultureLine.IndexOf('"', start);
                                    subCulture.BastardPrefix = subCultureLine.Substring(start, end - start);
                                }
                                else if (subCultureLine.Contains("male_patronym") && !subCultureLine.Contains("female_patronym"))
                                {
                                    int start = subCultureLine.IndexOf('"') + 1;
                                    int end = subCultureLine.IndexOf('"', start);
                                    subCulture.MalePatronym = subCultureLine.Substring(start, end - start);
                                }
                                else if (subCultureLine.Contains("female_patronym"))
                                {
                                    int start = subCultureLine.IndexOf('"') + 1;
                                    int end = subCultureLine.IndexOf('"', start);
                                    subCulture.FemalePatronym = subCultureLine.Substring(start, end - start);
                                }
                                else if (subCultureLine.Contains("prefix = "))
                                {
                                    int start = subCultureLine.IndexOf('=') + 1;
                                    String yesOrNo = subCultureLine.Substring(start, subCultureLine.Length - start);

                                    if(yesOrNo.Trim().Equals("yes"))
                                        subCulture.IsSuffix = true;
                                    else
                                        subCulture.IsSuffix = false;
                                }
                                else if (subCultureLine.Contains("pat_grf_name_chance"))
                                {
                                    subCulture.PaternalGrandFather = Int32.Parse(Regex.Match(subCultureLine, @"\d+").Value);
                                }
                                else if (subCultureLine.Contains("mat_grf_name_chance"))
                                {
                                    subCulture.MaternalGrandFather = Int32.Parse(Regex.Match(subCultureLine, @"\d+").Value);
                                }
                                else if (subCultureLine.Contains("father_name_chance"))
                                {
                                    subCulture.Father = Int32.Parse(Regex.Match(subCultureLine, @"\d+").Value);
                                }
                                else if (subCultureLine.Contains("pat_grm_name_chance"))
                                {
                                    subCulture.PaternalGrandMother = Int32.Parse(Regex.Match(subCultureLine, @"\d+").Value);
                                }
                                else if (subCultureLine.Contains("mat_grm_name_chance"))
                                {
                                    subCulture.MaternalGrandMother = Int32.Parse(Regex.Match(subCultureLine, @"\d+").Value);
                                }
                                else if (subCultureLine.Contains("mother_name_chance"))
                                {
                                    subCulture.Mother = Int32.Parse(Regex.Match(subCultureLine, @"\d+").Value);
                                }
                                else if (subCultureLine.Contains("modifier"))
                                {
                                    int start = subCultureLine.IndexOf('=') + 2;
                                    int end = subCultureLine.Length - start;
                                    subCulture.Modifier = subCultureLine.Substring(start, end);
                                }

                            } while (closeBrackets < openBrackets);

                            culture.SubCultures.Add(subCulture);
                            closingBrackets++;
                        }
                        
                    } while (closingBrackets < openingBrackets);

                    // Add the row to our list
                    loadingCultures.Add(culture);
                }
            }

            stream.Close();
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

            root.Expand();
        }

        private void characterBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Character> loadingCharacters = new List<Character>();

            BackgroundWorker worker = sender as BackgroundWorker;
            StreamReader stream = e.Argument as StreamReader;

            String line;

            while ((line = stream.ReadLine()) != null)
            {
                // Start of character
                if ((line.Contains("= {") || line.Contains("={")) && !line.StartsWith("#"))
                {
                    Character c = new Character();
                    c.ID = Int32.Parse(Regex.Match(line, @"\d+").Value);
                    c.File = Path.GetFileNameWithoutExtension(CurrentMod.CharacterFilesToLoad.Peek());

                    int openingBrackets = 1;
                    int closingBrackets = 0;

                    String insideLine;
                    do
                    {
                        insideLine = stream.ReadLine();

                        if (insideLine.Contains("{"))
                            openingBrackets++;
                        if (insideLine.Contains("}"))
                            closingBrackets++;

                        // Add the name
                        if (insideLine.Contains("name") && !insideLine.Contains("nickname"))
                        {
                            int start = insideLine.IndexOf('"') + 1;
                            int end = insideLine.IndexOf('"', start);
                            c.Name = insideLine.Substring(start, end - start);
                        }

                        // Add the culture
                        else if (insideLine.Contains("culture"))
                        {
                            // Cultures are added in two ways with "" or without
                            if (insideLine.Contains('"'))
                            {
                                int start = insideLine.IndexOf('"') + 1;
                                int end = insideLine.IndexOf('"', start);
                                c.Culture = insideLine.Substring(start, end - start);
                            }
                            else
                            {
                                c.Culture = insideLine.Substring(insideLine.IndexOf("=") + 1).Trim();
                            }
                        }

                        // Religion
                        else if (insideLine.Contains("religion"))
                        {
                            if (insideLine.Contains('"'))
                            {
                                int start = insideLine.IndexOf('"') + 1;
                                int end = insideLine.IndexOf('"', start);
                                c.Religion = insideLine.Substring(start, end - start);
                            }
                            else
                            {
                                c.Religion = insideLine.Substring(insideLine.IndexOf("=") + 1).Trim();
                            }
                        }

                        // Add dynasty
                        else if (insideLine.Contains("dynasty"))
                        {
                            c.Dynasty = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }

                    } while (closingBrackets < openingBrackets);


                    // Add the row to our list
                    loadingCharacters.Add(c);
                }
            }

            stream.Close();
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
                return;
            }

            CurrentMod = m;

            tabControl.Visible = true;
            dynastyGridView.DataSource = CurrentMod.Dynasties;
            dynastyGridView.Visible = true;

            characterGridView.DataSource = CurrentMod.Characters;


            // Add data bindings
            textBoxModName.DataBindings.Add("Text", CurrentMod, "Name");
            textBoxDependencies.DataBindings.Add("Text", CurrentMod, "Dependencies");
            textBoxModRawOutput.DataBindings.Add("Text", CurrentMod, "RawOutput");
            buttonImportDynasties.DataBindings.Add("Enabled", CurrentMod, "AreDynastiesImported");
            buttonImportCultures.DataBindings.Add("Enabled", CurrentMod, "AreCulturesImported");

            // Setup the culture tree
            cultureTreeView.Nodes.Clear();
            TreeNode root = new TreeNode("Culture Groups");
            root.ContextMenuStrip = cultureRootContextMenuStrip;
            cultureTreeView.Nodes.Add(root);

            // Load the dynasties
            String dynastyFile = WorkingLocation + "/" + CurrentMod.Path + "/common/dynasties.txt";
            if (File.Exists(dynastyFile))
            {
                StreamReader reader = new StreamReader(dynastyFile, true);
                dynastyBackgroundWorker.RunWorkerAsync(reader);
            }

            // Load the cultures
            String cultureFile = WorkingLocation + "/" + CurrentMod.Path + "/common/cultures.txt";
            if (File.Exists(cultureFile))
            {
                StreamReader reader = new StreamReader(cultureFile, true);
                cultureBackgroundWorker.RunWorkerAsync(reader);
            }

            // Load characters
            String charactersFolder = WorkingLocation + "/" + CurrentMod.Path + "/history/characters/";
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

                        characterFilesListBox.Items.Add(Path.GetFileNameWithoutExtension(file));
                    }                    
                }

                // Start loading character files
                if (CurrentMod.CharacterFilesToLoad.Count > 0)
                {
                    StreamReader reader = new StreamReader(CurrentMod.CharacterFilesToLoad.Peek(), Encoding.Default, true);
                    characterBackgroundWorker.RunWorkerAsync(reader);
                }
            }
            
            UserPreferences.Default.LastMod = WorkingLocation + "/mod/" + CurrentMod.Name + ".mod";
            UserPreferences.Default.Save();

            saveToolStripMenuItem.Enabled = true;
            closeModToolStripMenuItem.Enabled = true;
            closeWithoutSavingToolStripMenuItem.Enabled = true;
        }

        public void CloseMod()
        {
            saveToolStripMenuItem.Enabled = false;
            closeModToolStripMenuItem.Enabled = false;
            closeWithoutSavingToolStripMenuItem.Enabled = false;

            CurrentMod = null;
            this.tabControl.Visible = false;

            dynastyGridView.DataSource = null;
            dynastyGridView.Visible = false;

            // Add data bindings
            textBoxModName.DataBindings.Clear();
            textBoxDependencies.DataBindings.Clear();
            textBoxModRawOutput.DataBindings.Clear();
            buttonImportDynasties.DataBindings.Clear();
            buttonImportCultures.DataBindings.Clear();

            // Reset the culture tree view
            cultureTreeView.Nodes.Clear();
            TreeNode root = new TreeNode("Culture Groups");
            root.ContextMenuStrip = cultureRootContextMenuStrip;
            cultureTreeView.Nodes.Add(root);
            root.Expand();

            // reset the characters tab
            characterFilesListBox.Items.Clear();
            characterFilesListBox.Items.Add("View All Characters"); // Add the default list value

            cultureInformationGroupBox.Visible = false;
            cultureNamesGroupBox.Visible = false;

            UserPreferences.Default.LastMod = "";
            UserPreferences.Default.Save();
        }
               
        private void SelectWorkingLocation()
        {
            folderBrowserDialog = new FolderBrowserDialog();
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.Description = "Select the main Crusader Kings II installation directory containing ck2.exe";
            folderBrowserDialog.SelectedPath = SteamDirectory;
            folderBrowserDialog.ShowNewFolderButton = false;

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(folderBrowserDialog.SelectedPath + "/ck2.exe"))
                {
                    WorkingLocation = folderBrowserDialog.SelectedPath;
                    workingLocationStripStatusLabel.Text = String.Format("Working Location: {0}", WorkingLocation);
                    UserPreferences.Default.WorkingLocation = WorkingLocation;
                    UserPreferences.Default.Save();
                }
                else
                {
                    MessageBox.Show("Please select an installation directory for Crusader Kings II.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowDynasty(Dynasty dynasty)
        {
            if (dynasty == null)
                return;

            // SETUP THE UI            
            // 
            // labelDynastyID
            // 
            Label labelDynastyID = new Label();
            labelDynastyID.AutoSize = true;
            labelDynastyID.Location = new System.Drawing.Point(7, 7);
            labelDynastyID.Name = "labelDynastyID";
            labelDynastyID.Size = new System.Drawing.Size(79, 13);
            labelDynastyID.TabIndex = 0;
            labelDynastyID.Text = "Dynasty ID:";
            // 
            // textBoxDynastyID
            // 
            TextBox textBoxDynastyID = new TextBox();
            textBoxDynastyID.Location = new System.Drawing.Point(10, 23);
            textBoxDynastyID.Name = "textBoxDynastyID";
            textBoxDynastyID.Size = new System.Drawing.Size(279, 20);
            textBoxDynastyID.TabIndex = 1;
            textBoxDynastyID.Text = dynasty.ID.ToString();
            textBoxDynastyID.DataBindings.Add("Text", dynasty, "ID");
            // 
            // labelDynastyName
            // 
            Label labelDynastyName = new Label();
            labelDynastyName.AutoSize = true;
            labelDynastyName.Location = new System.Drawing.Point(7, 58);
            labelDynastyName.Name = "labelDynastyName";
            labelDynastyName.Size = new System.Drawing.Size(79, 13);
            labelDynastyName.TabIndex = 0;
            labelDynastyName.Text = "Dynasty Name:";
            // 
            // textBoxDynastyName
            // 
            TextBox textBoxDynastyName = new TextBox();
            textBoxDynastyName.Location = new System.Drawing.Point(10, 74);
            textBoxDynastyName.Name = "textBoxDynastyName";
            textBoxDynastyName.Size = new System.Drawing.Size(279, 20);
            textBoxDynastyName.TabIndex = 1;
            textBoxDynastyName.Text = dynasty.Name;
            textBoxDynastyName.DataBindings.Add("Text", dynasty, "Name");
            // 
            // labelDynastyCulture
            // 
            Label labelDynastyCulture = new Label();
            labelDynastyCulture.AutoSize = true;
            labelDynastyCulture.Location = new System.Drawing.Point(7, 109);
            labelDynastyCulture.Name = "labelDynastyCulture";
            labelDynastyCulture.Size = new System.Drawing.Size(43, 13);
            labelDynastyCulture.TabIndex = 2;
            labelDynastyCulture.Text = "Culture:";
            // 
            // textBoxDynastyCulture
            // 
            TextBox textBoxDynastyCulture = new TextBox();
            textBoxDynastyCulture.Location = new System.Drawing.Point(10, 125);
            textBoxDynastyCulture.Name = "textBoxDynastyCulture";
            textBoxDynastyCulture.Size = new System.Drawing.Size(279, 20);
            textBoxDynastyCulture.TabIndex = 3;
            textBoxDynastyCulture.Text = dynasty.Culture;
            textBoxDynastyCulture.DataBindings.Add("Text", dynasty, "Culture");
            // 
            // dataGridViewDynastyCharacters
            // 
            DataGridView dataGridViewDynastyCharacters = new DataGridView();
            dataGridViewDynastyCharacters.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            dataGridViewDynastyCharacters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDynastyCharacters.Location = new System.Drawing.Point(10, 265);
            dataGridViewDynastyCharacters.Name = "dataGridViewDynastyCharacters";
            dataGridViewDynastyCharacters.Size = new System.Drawing.Size(755, 213);
            dataGridViewDynastyCharacters.TabIndex = 5;
            // 
            // labelDynastyCharacters
            // 
            Label labelDynastyCharacters = new Label();
            labelDynastyCharacters.AutoSize = true;
            labelDynastyCharacters.Location = new System.Drawing.Point(7, 249);
            labelDynastyCharacters.Name = "labelDynastyCharacters";
            labelDynastyCharacters.Size = new System.Drawing.Size(99, 13);
            labelDynastyCharacters.Text = "Dynasty Characters";
            //
            // buttonClose
            //
            Button buttonClose = new Button();
            buttonClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonClose.Location = new Point(636, 6);
            buttonClose.Text = "Close";
            buttonClose.Size = new Size(129, 23);
            buttonClose.TabIndex = 4;
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

            tabDynastyInfo.Controls.Add(labelDynastyID);
            tabDynastyInfo.Controls.Add(textBoxDynastyID);
            tabDynastyInfo.Controls.Add(labelDynastyCharacters);            
            tabDynastyInfo.Controls.Add(textBoxDynastyCulture);
            tabDynastyInfo.Controls.Add(labelDynastyCulture);
            tabDynastyInfo.Controls.Add(textBoxDynastyName);
            tabDynastyInfo.Controls.Add(labelDynastyName);
            tabDynastyInfo.Controls.Add(buttonClose);            
            tabDynastyInfo.Controls.Add(dataGridViewDynastyCharacters);

            this.tabControl.TabPages.Add(tabDynastyInfo);
            this.tabControl.SelectedIndex = tabDynastyInfo.TabIndex;

            EventHandler closeHandler = (s, e) => this.tabControl.TabPages.Remove(tabDynastyInfo);
            EventHandler closeHandler2 = (s, e) => this.tabControl.SelectedIndex = 1; // go to dynasties tab
            buttonClose.Click += closeHandler;
            buttonClose.Click += closeHandler2;
                
        }

        public void SaveMod()
        {
            if (CurrentMod == null)
                return;

            String modPath = WorkingLocation + "/" + CurrentMod.Path;

            // Write out the mod file
            StreamWriter stream = File.CreateText(WorkingLocation + "/mod/" + CurrentMod.Name + ".mod");
            stream.Write(CurrentMod.RawOutput);
            stream.Close();

            // Make sure the mod directory exists
            if (!Directory.Exists(modPath))
            {
                Directory.CreateDirectory(modPath);
            }

            // Write out the dynasties
            if (CurrentMod.Dynasties.Count > 0)
            {
                if (!Directory.Exists(modPath + "/common"))
                {
                    Directory.CreateDirectory(modPath + "/common");
                }

                stream = File.CreateText(modPath + "/common/dynasties.txt");

                foreach (Dynasty dynasty in CurrentMod.Dynasties)
                    stream.Write(dynasty.ToString());

                stream.Close();
            }

            // Write out the cultures
            if (CurrentMod.Cultures.Count > 0)
            {
                if (!Directory.Exists(modPath + "/common"))
                {
                    Directory.CreateDirectory(modPath + "/common");
                }

                stream = File.CreateText(modPath + "/common/cultures.txt");

                foreach (Culture culture in CurrentMod.Cultures)
                    stream.Write(culture.ToString());

                stream.Close();
            }

            /*
            StreamWriter stream = File.CreateText(WorkingLocation + "/dynasties.modder");
            

            foreach (Dynasty dynasty in dynasties)
                stream.Write(dynasty.ToString());
            */
        }

        #endregion

        #region Events

        private void characterFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            String selected = characterFilesListBox.SelectedItem as String;

            if (selected.Equals("View All Characters"))
            {
                characterGridView.DataSource = CurrentMod.Characters;
                return;
            }

            BindingList<Character> filteredList = new BindingList<Character>(CurrentMod.Characters.Where(m => m.File.ToLower().Equals(selected) == true).ToList());
            characterGridView.DataSource = filteredList;
            
        }

        void cultureTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectedCultureNode = e.Node;

            // Don't do this on a right-click
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                return;

            String nodeName = e.Node.Text;

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
                Boolean isCulture = (selectedCulture.SubCultures.Count == 0 && !SelectedCultureNode.Parent.Text.Equals("Culture Groups"));

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
            DataGridViewRow row = dynastyGridView.Rows[e.RowIndex];

            ShowDynasty((Dynasty)row.DataBoundItem);
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
                System.Console.WriteLine(openFileDialog.FileName);

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
            NewModForm newMod = new NewModForm(this);
            newMod.ShowDialog(this);
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
                int count = CurrentMod.Cultures.RemoveAll(delegate(Culture culture) { return culture.Name.Equals(SelectedCultureNode.Text); });

                if (count > 0)
                {
                    SelectedCultureNode.Remove();
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

                // After importing vanilla data we want to replace the path to the dynasties
                CurrentMod.ReplaceCommonPath = true;
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

                // After importing vanilla data we want to replace the path to the dynasties
                CurrentMod.ReplaceCommonPath = true;
            }
        }

        #endregion

        

    }
}
