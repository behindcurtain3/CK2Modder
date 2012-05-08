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

namespace CK2Modder
{
    public partial class Form1 : Form
    {
        public static readonly String SteamDirectory = "C:\\Program Files\\Steam\\steamapps\\common\\crusader kings ii";
        public static readonly String DynastyFile = "/common/dynasties.txt";

        public String SelectedFolder { get; set; }

        public Form1()
        {
            InitializeComponent();

            toolStripProgressBar.Visible = false;

            dynastyBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(dynastyBackgroundWorker_ProgressChanged);
            dynastyBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dynastyBackgroundWorker_RunWorkerCompleted);
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
            List<String[]> rows = new List<string[]>();

            BackgroundWorker worker = sender as BackgroundWorker;
            StreamReader stream = e.Argument as StreamReader;

            String line;

            while ((line = stream.ReadLine()) != null)
            {
                // Start of dynasty
                if (line.EndsWith("= {") && !line.StartsWith("#"))
                {
                    String[] row = new String[3];
                    row[0] = Regex.Match(line, @"\d+").Value;
                    row[1] = "";
                    row[2] = "";

                    int openingBrackets = 1;
                    int closingBrackets = 0;

                    String insideLine;

                    do
                    {
                        insideLine = stream.ReadLine();

                        if (insideLine.Contains("{"))
                            openingBrackets++;
                        else if (insideLine.Contains("}"))
                            closingBrackets++;

                        if (insideLine.Contains("name"))
                        {
                            int start = insideLine.IndexOf('"') + 1;
                            int end = insideLine.IndexOf('"', start);
                            row[1] = insideLine.Substring(start, end - start);
                        }
                        if (insideLine.Contains("culture"))
                        {
                            // Cultures are added in two ways with "" or without
                            if (insideLine.Contains('"'))
                            {
                                int start = insideLine.IndexOf('"') + 1;
                                int end = insideLine.IndexOf('"', start);
                                row[2] = insideLine.Substring(start, end - start);
                            }
                            else
                            {
                                row[2] = insideLine.Substring(insideLine.IndexOf("=") + 1).Trim();
                            }
                        }

                    } while (closingBrackets < openingBrackets);


                    // Add the row to our list
                    rows.Add(row);

                    // Update the progress
                    progress += 10;
                    if(progress > 100)
                        progress = 100;

                    worker.ReportProgress(progress);
                }               
            }

            stream.Close();
            e.Result = rows;
        }

        void dynastyBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar.Visible = false;

            List<String[]> rows = e.Result as List<String[]>;
            foreach (string[] rowArray in rows)
            {
                dynastyGridView.Rows.Add(rowArray);
            }

        }

        void dynastyBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = e.ProgressPercentage;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
