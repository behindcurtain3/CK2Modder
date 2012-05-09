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

namespace CK2Modder
{
    public partial class Form1 : Form
    {
        public static readonly String SteamDirectory = "C:\\Program Files\\Steam\\steamapps\\common\\crusader kings ii";
        public static readonly String SteamDirectoryX86 = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\crusader kings ii";
        public static readonly String VanillaDynastyFile = "/common/dynasties.txt";

        public String WorkingLocation { get; set; }
        public Mod CurrentMod { get; set; }

        public Form1(string filename)
        {
            Initialize();

            // Attempt to load the mod passed in
            if (File.Exists(filename))
            {
                setCurrentMod(Mod.LoadFromFile(filename));
            }

        }

        public Form1()
        {
            Initialize();

            // Attempt to load the last mod
            if (File.Exists(UserPreferences.Default.LastMod))
            {
                setCurrentMod(Mod.LoadFromFile(UserPreferences.Default.LastMod));
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

                setCurrentMod(Mod.LoadFromFile(openFileDialog.FileName));
            }            
        }

        private void ImportAllVanillaDynasties()
        {
            String absolutePath = WorkingLocation + VanillaDynastyFile;

            if (File.Exists(absolutePath))
            {
                StreamReader stream = File.OpenText(absolutePath);
                

                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Maximum = 100;

                dynastyBackgroundWorker.RunWorkerAsync(stream);

                // After importing vanilla data we want to replace the path to the dynasties
                CurrentMod.ReplaceCommonPath = true;
            }
        }

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

        public void setCurrentMod(Mod m)
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
            textBoxDependencies.DataBindings.Add("Text", CurrentMod, "Dependencies");
            textBoxModRawOutput.DataBindings.Add("Text", CurrentMod, "RawOutput");
            buttonImportDynasties.DataBindings.Add("Enabled", CurrentMod, "NotReplaceCommonPath");

            // Load the dynasties
            String dynastyFile = WorkingLocation + "/" + CurrentMod.Path + "/common/dynasties.txt";
            if (File.Exists(dynastyFile))
            {
                StreamReader stream = File.OpenText(dynastyFile);
                dynastyBackgroundWorker.RunWorkerAsync(stream);
            }

            UserPreferences.Default.LastMod = WorkingLocation + "/mod/" + CurrentMod.Name + ".mod";
            UserPreferences.Default.Save();

            saveToolStripMenuItem.Enabled = true;
            closeModToolStripMenuItem.Enabled = true;
            closeWithoutSavingToolStripMenuItem.Enabled = true;
        }

        private void buttonImportDynasties_Click(object sender, EventArgs e)
        {
            ImportAllVanillaDynasties();
        }

        private void workingLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectWorkingLocation();
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
            buttonClose.Location = new Point(636, 6);
            buttonClose.Text = "Close";
            buttonClose.Size = new Size(129, 23);
            buttonClose.TabIndex = 4;
            // 
            // tabDynastyInfo
            // 
            TabPage tabDynastyInfo = new TabPage(dynasty.Name);
            tabDynastyInfo.Controls.Add(labelDynastyID);
            tabDynastyInfo.Controls.Add(textBoxDynastyID);
            tabDynastyInfo.Controls.Add(labelDynastyCharacters);
            tabDynastyInfo.Controls.Add(dataGridViewDynastyCharacters);
            tabDynastyInfo.Controls.Add(textBoxDynastyCulture);
            tabDynastyInfo.Controls.Add(labelDynastyCulture);
            tabDynastyInfo.Controls.Add(textBoxDynastyName);
            tabDynastyInfo.Controls.Add(labelDynastyName);
            tabDynastyInfo.Controls.Add(buttonClose);
            tabDynastyInfo.Location = new System.Drawing.Point(4, 22);
            tabDynastyInfo.Name = "tabDynastyInfo";
            tabDynastyInfo.Padding = new System.Windows.Forms.Padding(3);
            tabDynastyInfo.Size = new System.Drawing.Size(773, 484);
            tabDynastyInfo.TabIndex = this.tabControl.TabCount;
            tabDynastyInfo.Text = dynasty.Name + " Dynasty";
            tabDynastyInfo.BackColor = Color.Transparent;
            //tabDynastyInfo.ResumeLayout(false);
            //tabDynastyInfo.PerformLayout();

            this.tabControl.TabPages.Add(tabDynastyInfo);
            this.tabControl.SelectedIndex = tabDynastyInfo.TabIndex;

            EventHandler closeHandler = (s, e) => this.tabControl.TabPages.Remove(tabDynastyInfo);
            EventHandler closeHandler2 = (s, e) => this.tabControl.SelectedIndex = 1; // go to dynasties tab
            buttonClose.Click += closeHandler;
            buttonClose.Click += closeHandler2;
                
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

            /*
            StreamWriter stream = File.CreateText(WorkingLocation + "/dynasties.modder");
            

            foreach (Dynasty dynasty in dynasties)
                stream.Write(dynasty.ToString());
            */
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

            UserPreferences.Default.LastMod = "";
            UserPreferences.Default.Save();
        }        
    }
}
