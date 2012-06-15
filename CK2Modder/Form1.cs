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
        public static readonly String VanillaCharactersPath = "/history/characters/";

        // Display string
        public static readonly String DefaultCultureRoot = "Culture Groups";
        public static readonly String DefaultCharacterListView = "View All Characters";

        public String WorkingLocation { get; set; }
        public Mod CurrentMod { get; set; }
        public Culture SelectedCulture { get; set; }
        public TreeNode SelectedCultureNode { get; set; }
        public Boolean IsCharacterGridListeningForRows { get; set; }

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
            characterGridView.CellDoubleClick += new DataGridViewCellEventHandler(characterGridView_CellDoubleClick);
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
                        insideLine = stream.ReadLine().Trim();

                        // Skip comment lines
                        if (insideLine.StartsWith("#"))
                            continue;

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

                        // Is female?
                        else if (insideLine.Contains("female"))
                        {
                            int start = insideLine.IndexOf('=') + 1;
                            String yesOrNo = insideLine.Substring(start, insideLine.Length - start);

                            if (yesOrNo.Trim().Equals("yes"))
                                c.Female = true;
                            else
                                c.Female = false;
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

                        // Nickname
                        else if (insideLine.StartsWith("give_nickname"))
                        {
                            c.Nickname = insideLine.Substring(insideLine.IndexOf("=") + 1).Trim();
                        }

                        // Stats
                        else if (insideLine.Contains("martial=") && !insideLine.Contains("trait"))
                        {
                            c.Martial = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }
                        else if (insideLine.Contains("diplomacy="))
                        {
                            c.Diplomacy = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }
                        else if (insideLine.Contains("intrigue="))
                        {
                            c.Intrigue = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }
                        else if (insideLine.Contains("stewardship="))
                        {
                            c.Stewardship = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }
                        else if (insideLine.Contains("learning="))
                        {
                            c.Learning = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }

                        // Traits
                        else if (insideLine.StartsWith("add_trait"))
                        {
                            if (insideLine.Contains('"'))
                            {
                                int start = insideLine.IndexOf('"') + 1;
                                int end = insideLine.IndexOf('"', start);
                                c.Traits.Add(insideLine.Substring(start, end - start));
                            }
                            else
                            {
                                c.Traits.Add(insideLine.Substring(insideLine.IndexOf("=") + 1).Trim());
                            }
                        }

                        // Mother & Father
                        else if (insideLine.Contains("father=") || insideLine.Contains("father ="))
                        {
                            c.Father = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }
                        else if (insideLine.Contains("mother=") || insideLine.Contains("mother ="))
                        {
                            c.Mother = Int32.Parse(Regex.Match(insideLine, @"\d+").Value);
                        }

                        // custom look
                        else if (insideLine.Contains("dna=") || insideLine.Contains("dna ="))
                        {
                            int start = insideLine.IndexOf('"') + 1;
                            int end = insideLine.IndexOf('"', start);
                            c.DNA = insideLine.Substring(start, end - start);
                        }
                        else if (insideLine.Contains("properties=") || insideLine.Contains("properties ="))
                        {
                            int start = insideLine.IndexOf('"') + 1;
                            int end = insideLine.IndexOf('"', start);
                            c.Properties = insideLine.Substring(start, end - start);
                        }

                        // life events
                        // some events are all on one line read them here
                        else if ((insideLine.Contains("={") || insideLine.Contains("= {")) && (insideLine.Contains("}")))
                        {
                            if(c.Events.Equals(""))
                                c.Events += insideLine;
                            else
                                c.Events += "\r\n" + insideLine;                           
                        }
                        // most are spread out over several lines
                        else if (insideLine.Contains("={") || insideLine.Contains("= {"))
                        {
                            String eventLine = insideLine;
                            Boolean finished = false;
                            do
                            {
                                if (c.Events.Equals(""))
                                    c.Events += eventLine;
                                else
                                {
                                    c.Events += "\r\n";

                                    if (!eventLine.Contains("{"))
                                    {
                                        eventLine = eventLine.Trim();

                                        if (!eventLine.Contains("}"))
                                            c.Events += "\t";
                                    }

                                    c.Events += eventLine;
                                }
                                
                                if (eventLine.Contains("}"))
                                    finished = true;
                                else
                                    eventLine = stream.ReadLine();
                            } while (!finished);
                            
                            closingBrackets++;                            
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
            characterGridView.AllowUserToAddRows = false; // Default to not allowing user to add rows

            // Load the dynasties
            String dynastyFile = WorkingLocation + "/" + CurrentMod.Path + "/common/dynasties.txt";
            if (File.Exists(dynastyFile))
            {
                StreamReader reader = new StreamReader(dynastyFile, Encoding.Default, true);
                dynastyBackgroundWorker.RunWorkerAsync(reader);
            }

            // Load the cultures
            String cultureFile = WorkingLocation + "/" + CurrentMod.Path + "/common/cultures.txt";
            if (File.Exists(cultureFile))
            {
                StreamReader reader = new StreamReader(cultureFile, Encoding.Default, true);
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

            CurrentMod.UpdateRawOutput();
            UserPreferences.Default.LastMod = WorkingLocation + "/mod/" + CurrentMod.Name + ".mod";
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

            // Reset the culture tree view
            cultureTreeView.Nodes.Clear();
            TreeNode root = new TreeNode(DefaultCultureRoot);
            root.ContextMenuStrip = cultureRootContextMenuStrip;
            cultureTreeView.Nodes.Add(root);
            root.Expand();

            // reset the characters tab
            characterFilesListBox.Items.Clear();

            characterGridView.RowsAdded -= characterGridView_RowsAdded;
            IsCharacterGridListeningForRows = false;
            characterGridView.DataSource = null;            

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

                    ShowCharacter(row.DataBoundItem as Character);
                });                
        }

        private void ShowCharacter(Character c)
        {
            if (c == null)
                return;

            // 
            // tabInfo
            // 
            TabPage tabInfo = new TabPage(c.Name);
            tabInfo.Location = new System.Drawing.Point(4, 22);
            tabInfo.Name = "tabCharacterInfo";
            tabInfo.Padding = new System.Windows.Forms.Padding(3);
            tabInfo.Size = new System.Drawing.Size(773, 484);
            tabInfo.TabIndex = this.tabControl.TabCount;
            tabInfo.Text = c.Name + " (" + c.ID + ")";
            tabInfo.BackColor = Color.Transparent;
            //
            // Character Editor
            //
            CharacterEditor editor = new CharacterEditor();
            editor.Dock = System.Windows.Forms.DockStyle.Fill;
            editor.Location = new System.Drawing.Point(3, 3);
            editor.Size = new System.Drawing.Size(767, 478);
            editor.TabIndex = 0;

            tabInfo.Controls.Add(editor);

            // Add data bindings
            editor.ID.DataBindings.Add("Text", c, "ID");
            editor.CharacterName.DataBindings.Add("Text", c, "Name");
            editor.Dynasty.DataBindings.Add("Text", c, "Dynasty");
            editor.Religion.DataBindings.Add("Text", c, "Religion");
            editor.Culture.DataBindings.Add("Text", c, "Culture");
            editor.Female.DataBindings.Add("Checked", c, "Female");
            editor.Nickname.DataBindings.Add("Text", c, "Nickname");

            editor.Father.DataBindings.Add("Text", c, "Father");
            editor.Mother.DataBindings.Add("Text", c, "Mother");

            editor.Traits.DataSource = c.Traits;
            editor.LifeEvents.DataBindings.Add("Text", c, "Events");

            editor.Martial.DataBindings.Add("Text", c, "Martial");
            editor.Diplomacy.DataBindings.Add("Text", c, "Diplomacy");
            editor.Intrigue.DataBindings.Add("Text", c, "Intrigue");
            editor.Stewardship.DataBindings.Add("Text", c, "Stewardship");
            editor.Learning.DataBindings.Add("Text", c, "Learning");

            editor.DNA.DataBindings.Add("Text", c, "DNA");
            editor.Properties.DataBindings.Add("Text", c, "Properties");

            // Add the tab and select it
            this.tabControl.TabPages.Add(tabInfo);
            this.tabControl.SelectedIndex = tabInfo.TabIndex;

            // Events
            EventHandler closeHandler = (s, e) => this.tabControl.TabPages.Remove(tabInfo);
            EventHandler closeHandler2 = (s, e) => this.tabControl.SelectedIndex = 2; // go to characters
            editor.CloseButton.Click += closeHandler;
            editor.CloseButton.Click += closeHandler2;

            editor.CharacterName.TextChanged += new EventHandler(delegate(object Sender, EventArgs e)
                {
                    tabInfo.Text = editor.CharacterName.Text + " (" + editor.ID.Text + ")";
                });
            editor.ID.TextChanged += new EventHandler(delegate(object Sender, EventArgs e)
                {
                    tabInfo.Text = editor.CharacterName.Text + " (" + editor.ID.Text + ")";
                });

            editor.Traits.KeyDown += new KeyEventHandler(delegate(object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                    {
                        // Delete the entry
                        c.Traits.RemoveAt(editor.Traits.SelectedIndex);
                    }
                });

            // Add a new trait to the list
            editor.AddTraitButton.Click += new EventHandler(delegate(object sender, EventArgs e)
                {
                    if (!editor.TraitComboBox.Text.Equals(""))
                    {
                        c.Traits.Add(editor.TraitComboBox.Text);
                        editor.TraitComboBox.Text = "";
                        editor.TraitComboBox.SelectedIndex = -1;
                    }
                });
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

            // Write out the characters
            if (CurrentMod.CharacterFiles.Count > 0)
            {
                if (!Directory.Exists(modPath + "/history/characters"))
                {
                    Directory.CreateDirectory(modPath + "/history/characters");
                }

                foreach (String name in CurrentMod.CharacterFiles)
                {
                    stream = new StreamWriter(modPath + "/history/characters/" + name + ".txt", false, Encoding.Default);
                    //stream = File.CreateText(modPath + "/history/characters/" + name + ".txt");

                    foreach (Character c in CurrentMod.Characters)
                    {
                        if (c.File.Equals(name))
                        {
                            stream.Write(c.ToString());
                        }
                    }

                    stream.Close();
                }
            }
        }

        #endregion

        #region Events

        private void characterFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMod == null)
                return;

            String selected = characterFilesListBox.SelectedItem as String;
            
            if (selected == null)
                return;

            // Remove the rows added event listener, we don't want this to fire in this scenario
            if (IsCharacterGridListeningForRows)
            {
                characterGridView.RowsAdded -= characterGridView_RowsAdded;
                IsCharacterGridListeningForRows = false;
            }

            if (selected.Equals(DefaultCharacterListView))
            {
                characterGridView.DataSource = CurrentMod.Characters;
                characterGridView.AllowUserToAddRows = false;                
                return;
            }

            BindingList<Character> filteredList = new BindingList<Character>(CurrentMod.Characters.Where(m => m.File.Equals(selected) == true).ToList());
            characterGridView.DataSource = filteredList;
            characterGridView.AllowUserToAddRows = true;

            // Add the rows added event listener
            if (!IsCharacterGridListeningForRows)
            {
                characterGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(characterGridView_RowsAdded);
                IsCharacterGridListeningForRows = true;
            }
            
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
            DataGridViewRow row = dynastyGridView.Rows[e.RowIndex];

            ShowDynasty((Dynasty)row.DataBoundItem);
        }

        void characterGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = characterGridView.Rows[e.RowIndex];

            ShowCharacter(row.DataBoundItem as Character);
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
                    if (c.File.Equals(selectedFile))
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
                            if (c.File.Equals(selected))
                                c.File = fileForm.FileName;
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

        private void characterGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for(int i = e.RowIndex - 1; i < e.RowIndex - 1 + e.RowCount; i++)
            {
                Character c = characterGridView.Rows[i].DataBoundItem as Character;

                if (c != null)
                {
                    // Set the character file to the currently selected file
                    c.File = characterFilesListBox.SelectedItem as String;

                    // Make sure the character is in the master list
                    if (!CurrentMod.Characters.Contains(c))
                        CurrentMod.Characters.Add(c);                    
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
