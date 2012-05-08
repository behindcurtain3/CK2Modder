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
        public static readonly String DynastyFile = "/common/dynasties.txt";

        public String SelectedFolder { get; set; }

        public BindingList<Dynasty> dynasties;

        public Form1()
        {
            InitializeComponent();

            dynasties = new BindingList<Dynasty>();

            toolStripProgressBar.Visible = false;

            dynastyBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(dynastyBackgroundWorker_ProgressChanged);
            dynastyBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dynastyBackgroundWorker_RunWorkerCompleted);

            dynastyGridView.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "ID";
            idColumn.HeaderText = "ID";

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";

            DataGridViewTextBoxColumn cultureColumn = new DataGridViewTextBoxColumn();
            cultureColumn.DataPropertyName = "Culture";
            cultureColumn.HeaderText = "Culture";

            dynastyGridView.Columns.Add(idColumn);
            dynastyGridView.Columns.Add(nameColumn);
            dynastyGridView.Columns.Add(cultureColumn);

            dynastyGridView.DataSource = dynasties;
            dynastyGridView.CellDoubleClick += new DataGridViewCellEventHandler(dynastyGridView_CellDoubleClick);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("sv");
        }

        void dynastyGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dynastyGridView.Rows[e.RowIndex];            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.Description = "Select the main Crusader Kings II installation directory.";
            folderBrowserDialog.SelectedPath = SteamDirectory;
            folderBrowserDialog.ShowNewFolderButton = false;

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectedFolder = folderBrowserDialog.SelectedPath;

                if (!SelectedFolder.Equals(""))
                {
                    workingLocationStripStatusLabel.Text = String.Format("Working Location: {0}", SelectedFolder);
                    LoadDynastiesInfo();
                }
            }
        }

        private void LoadDynastiesInfo()
        {
            String absolutePath = SelectedFolder + DynastyFile;

            if (File.Exists(absolutePath))
            {
                StreamReader stream = File.OpenText(absolutePath);
                

                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Maximum = 100;

                dynastyBackgroundWorker.RunWorkerAsync(stream);
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

            List<Dynasty> rows = e.Result as List<Dynasty>;

            foreach (Dynasty d in rows)
                dynasties.Add(d);

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
            StreamWriter stream = File.CreateText(SelectedFolder + "/dynasties.modder");
            

            foreach (Dynasty dynasty in dynasties)
                stream.Write(dynasty.ToString());

            stream.Close();
        }
    }
}
