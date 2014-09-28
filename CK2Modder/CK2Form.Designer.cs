using System;
using System.Drawing;
using RavSoft;

namespace CK2Modder
{
    partial class CK2Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Culture Groups");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWithoutSavingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workingLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.workingLocationStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabModProperties = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.userDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.addPathButton = new System.Windows.Forms.Button();
            this.replacePathsComboBox = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.replacePathsListBox = new System.Windows.Forms.ListBox();
            this.textBoxDependencies = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxModName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonImportCultures = new System.Windows.Forms.Button();
            this.buttonImportCharacters = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonImportDynasties = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxModRawOutput = new System.Windows.Forms.TextBox();
            this.culturesTabPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cultureTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cultureInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.cultureSuffixCheckBox = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cultureFemalePatronymTextBox = new System.Windows.Forms.TextBox();
            this.cultureMalePatronymTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cultureBastardTextBox = new System.Windows.Forms.TextBox();
            this.cultureModifierTextBox = new System.Windows.Forms.TextBox();
            this.cultureDynastyPrefixTextBox = new System.Windows.Forms.TextBox();
            this.cultureColorTextBox = new System.Windows.Forms.TextBox();
            this.cultureGfxTextBox = new System.Windows.Forms.TextBox();
            this.cultureNameTextBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cultureMotherTextBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cultureMatGMTextBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.culturePatGMTextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cultureFatherTextBox = new System.Windows.Forms.TextBox();
            this.cultureMatGFTextBox = new System.Windows.Forms.TextBox();
            this.culturePatGFTextBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cultureNamesGroupBox = new System.Windows.Forms.GroupBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.cultureMaleNamesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.cultureFemaleNamesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.tabDynasties = new System.Windows.Forms.TabPage();
            this.textBoxDynastyFilterByCulture = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDynastyFilterByName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonDynastyClearFilter = new System.Windows.Forms.Button();
            this.buttonDynastyFilter = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDynastyFilterByID = new System.Windows.Forms.TextBox();
            this.dynastyGridView = new System.Windows.Forms.DataGridView();
            this.tabCharacters = new System.Windows.Forms.TabPage();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.characterFilesFilter = new System.Windows.Forms.TextBox();
            this.characterFilesListBox = new System.Windows.Forms.ListBox();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.characterListBox = new System.Windows.Forms.ListBox();
            this.characterPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.characterFilesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.characterFilesMenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynastyBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cultureBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.cultureSubContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cultureStripAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cultureStripRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cultureRootContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cultureGroupStripMenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cultureContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cultureToolStripMenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.characterBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.newModDialog = new System.Windows.Forms.SaveFileDialog();
            this.characterFilter = new System.Windows.Forms.TextBox();
            this.mainMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabModProperties.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.culturesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.cultureInformationGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.cultureNamesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabDynasties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dynastyGridView)).BeginInit();
            this.tabCharacters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.characterFilesContextMenuStrip.SuspendLayout();
            this.cultureSubContextMenuStrip.SuspendLayout();
            this.cultureRootContextMenuStrip.SuspendLayout();
            this.cultureContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeModToolStripMenuItem,
            this.closeWithoutSavingToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // modToolStripMenuItem
            // 
            this.modToolStripMenuItem.Name = "modToolStripMenuItem";
            this.modToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.modToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.modToolStripMenuItem.Text = "Mod";
            this.modToolStripMenuItem.Click += new System.EventHandler(this.modToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openToolStripMenuItem.Text = "Load...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // closeModToolStripMenuItem
            // 
            this.closeModToolStripMenuItem.Enabled = false;
            this.closeModToolStripMenuItem.Name = "closeModToolStripMenuItem";
            this.closeModToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeModToolStripMenuItem.Text = "Save and Close";
            this.closeModToolStripMenuItem.Click += new System.EventHandler(this.closeModToolStripMenuItem_Click);
            // 
            // closeWithoutSavingToolStripMenuItem
            // 
            this.closeWithoutSavingToolStripMenuItem.Enabled = false;
            this.closeWithoutSavingToolStripMenuItem.Name = "closeWithoutSavingToolStripMenuItem";
            this.closeWithoutSavingToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeWithoutSavingToolStripMenuItem.Text = "Close Without Saving";
            this.closeWithoutSavingToolStripMenuItem.Click += new System.EventHandler(this.closeWithoutSavingToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workingLocationToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // workingLocationToolStripMenuItem
            // 
            this.workingLocationToolStripMenuItem.Name = "workingLocationToolStripMenuItem";
            this.workingLocationToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.workingLocationToolStripMenuItem.Text = "Working Location...";
            this.workingLocationToolStripMenuItem.Click += new System.EventHandler(this.workingLocationToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.workingLocationStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // workingLocationStripStatusLabel
            // 
            this.workingLocationStripStatusLabel.Name = "workingLocationStripStatusLabel";
            this.workingLocationStripStatusLabel.Size = new System.Drawing.Size(104, 17);
            this.workingLocationStripStatusLabel.Text = "Working Location:";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabModProperties);
            this.tabControl.Controls.Add(this.culturesTabPage);
            this.tabControl.Controls.Add(this.tabDynasties);
            this.tabControl.Controls.Add(this.tabCharacters);
            this.tabControl.Location = new System.Drawing.Point(3, 27);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(781, 510);
            this.tabControl.TabIndex = 0;
            this.tabControl.Visible = false;
            // 
            // tabModProperties
            // 
            this.tabModProperties.BackColor = System.Drawing.Color.Transparent;
            this.tabModProperties.Controls.Add(this.groupBox4);
            this.tabModProperties.Controls.Add(this.groupBox1);
            this.tabModProperties.Controls.Add(this.label2);
            this.tabModProperties.Controls.Add(this.textBoxModRawOutput);
            this.tabModProperties.Location = new System.Drawing.Point(4, 22);
            this.tabModProperties.Name = "tabModProperties";
            this.tabModProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabModProperties.Size = new System.Drawing.Size(773, 484);
            this.tabModProperties.TabIndex = 0;
            this.tabModProperties.Text = "Mod";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.userDirectoryTextBox);
            this.groupBox4.Controls.Add(this.addPathButton);
            this.groupBox4.Controls.Add(this.replacePathsComboBox);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.replacePathsListBox);
            this.groupBox4.Controls.Add(this.textBoxDependencies);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.textBoxModName);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(3, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(401, 241);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mod Information";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "User Directory:";
            // 
            // userDirectoryTextBox
            // 
            this.userDirectoryTextBox.Location = new System.Drawing.Point(105, 53);
            this.userDirectoryTextBox.Name = "userDirectoryTextBox";
            this.userDirectoryTextBox.Size = new System.Drawing.Size(249, 20);
            this.userDirectoryTextBox.TabIndex = 13;
            // 
            // addPathButton
            // 
            this.addPathButton.Location = new System.Drawing.Point(279, 208);
            this.addPathButton.Name = "addPathButton";
            this.addPathButton.Size = new System.Drawing.Size(75, 23);
            this.addPathButton.TabIndex = 11;
            this.addPathButton.Text = "Add";
            this.addPathButton.UseVisualStyleBackColor = true;
            this.addPathButton.Click += new System.EventHandler(this.addPathButton_Click);
            // 
            // replacePathsComboBox
            // 
            this.replacePathsComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.replacePathsComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.replacePathsComboBox.FormattingEnabled = true;
            this.replacePathsComboBox.Items.AddRange(new object[] {
            "common",
            "common/event_modifiers",
            "common/traits",
            "decisions",
            "events",
            "history/characters",
            "history/diplomacy",
            "history/provinces",
            "history/titles",
            "history/wars"});
            this.replacePathsComboBox.Location = new System.Drawing.Point(105, 208);
            this.replacePathsComboBox.Name = "replacePathsComboBox";
            this.replacePathsComboBox.Size = new System.Drawing.Size(168, 21);
            this.replacePathsComboBox.TabIndex = 10;
            this.replacePathsComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.replacePathsComboBox_KeyDown);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(20, 133);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(80, 13);
            this.label23.TabIndex = 9;
            this.label23.Text = "Replace Paths:";
            // 
            // replacePathsListBox
            // 
            this.replacePathsListBox.FormattingEnabled = true;
            this.replacePathsListBox.Location = new System.Drawing.Point(105, 133);
            this.replacePathsListBox.Name = "replacePathsListBox";
            this.replacePathsListBox.Size = new System.Drawing.Size(249, 69);
            this.replacePathsListBox.TabIndex = 8;
            this.replacePathsListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.replacePathsListBox_KeyDown);
            // 
            // textBoxDependencies
            // 
            this.textBoxDependencies.Location = new System.Drawing.Point(105, 79);
            this.textBoxDependencies.Name = "textBoxDependencies";
            this.textBoxDependencies.Size = new System.Drawing.Size(249, 20);
            this.textBoxDependencies.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // textBoxModName
            // 
            this.textBoxModName.Location = new System.Drawing.Point(105, 27);
            this.textBoxModName.Name = "textBoxModName";
            this.textBoxModName.Size = new System.Drawing.Size(249, 20);
            this.textBoxModName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(102, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ex: \"someothermod\", \"myawesomemod\"";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Dependencies:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.buttonImportCultures);
            this.groupBox1.Controls.Add(this.buttonImportCharacters);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.buttonImportDynasties);
            this.groupBox1.Location = new System.Drawing.Point(9, 254);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 197);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import Vanilla Data";
            // 
            // buttonImportCultures
            // 
            this.buttonImportCultures.Location = new System.Drawing.Point(261, 65);
            this.buttonImportCultures.Name = "buttonImportCultures";
            this.buttonImportCultures.Size = new System.Drawing.Size(128, 23);
            this.buttonImportCultures.TabIndex = 7;
            this.buttonImportCultures.Text = "Import Cultures";
            this.buttonImportCultures.UseVisualStyleBackColor = true;
            this.buttonImportCultures.Click += new System.EventHandler(this.buttonImportCultures_Click);
            // 
            // buttonImportCharacters
            // 
            this.buttonImportCharacters.Enabled = false;
            this.buttonImportCharacters.Location = new System.Drawing.Point(132, 65);
            this.buttonImportCharacters.Name = "buttonImportCharacters";
            this.buttonImportCharacters.Size = new System.Drawing.Size(123, 23);
            this.buttonImportCharacters.TabIndex = 6;
            this.buttonImportCharacters.Text = "Import Characters";
            this.buttonImportCharacters.UseVisualStyleBackColor = true;
            this.buttonImportCharacters.Click += new System.EventHandler(this.buttonImportCharacters_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(6, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(383, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Import All";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // buttonImportDynasties
            // 
            this.buttonImportDynasties.Location = new System.Drawing.Point(9, 65);
            this.buttonImportDynasties.Name = "buttonImportDynasties";
            this.buttonImportDynasties.Size = new System.Drawing.Size(117, 23);
            this.buttonImportDynasties.TabIndex = 4;
            this.buttonImportDynasties.Text = "Import Dynasties";
            this.buttonImportDynasties.UseVisualStyleBackColor = true;
            this.buttonImportDynasties.Click += new System.EventHandler(this.buttonImportDynasties_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(435, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Raw Output:";
            // 
            // textBoxModRawOutput
            // 
            this.textBoxModRawOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxModRawOutput.Location = new System.Drawing.Point(435, 23);
            this.textBoxModRawOutput.Multiline = true;
            this.textBoxModRawOutput.Name = "textBoxModRawOutput";
            this.textBoxModRawOutput.ReadOnly = true;
            this.textBoxModRawOutput.Size = new System.Drawing.Size(332, 455);
            this.textBoxModRawOutput.TabIndex = 2;
            // 
            // culturesTabPage
            // 
            this.culturesTabPage.BackColor = System.Drawing.Color.Transparent;
            this.culturesTabPage.Controls.Add(this.splitContainer1);
            this.culturesTabPage.Location = new System.Drawing.Point(4, 22);
            this.culturesTabPage.Name = "culturesTabPage";
            this.culturesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.culturesTabPage.Size = new System.Drawing.Size(773, 484);
            this.culturesTabPage.TabIndex = 4;
            this.culturesTabPage.Text = "Cultures";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cultureTreeView);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel1MinSize = 125;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(761, 472);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 0;
            // 
            // cultureTreeView
            // 
            this.cultureTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cultureTreeView.Location = new System.Drawing.Point(0, 0);
            this.cultureTreeView.Name = "cultureTreeView";
            treeNode5.Name = "Culture Groups";
            treeNode5.Text = "Culture Groups";
            this.cultureTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.cultureTreeView.ShowRootLines = false;
            this.cultureTreeView.Size = new System.Drawing.Size(150, 472);
            this.cultureTreeView.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cultureInformationGroupBox);
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Panel1MinSize = 250;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.cultureNamesGroupBox);
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Size = new System.Drawing.Size(611, 481);
            this.splitContainer2.SplitterDistance = 251;
            this.splitContainer2.TabIndex = 0;
            // 
            // cultureInformationGroupBox
            // 
            this.cultureInformationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cultureInformationGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.cultureInformationGroupBox.Controls.Add(this.cultureSuffixCheckBox);
            this.cultureInformationGroupBox.Controls.Add(this.label13);
            this.cultureInformationGroupBox.Controls.Add(this.cultureFemalePatronymTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.cultureMalePatronymTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.label11);
            this.cultureInformationGroupBox.Controls.Add(this.cultureBastardTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.cultureModifierTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.cultureDynastyPrefixTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.cultureColorTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.cultureGfxTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.cultureNameTextBox);
            this.cultureInformationGroupBox.Controls.Add(this.label22);
            this.cultureInformationGroupBox.Controls.Add(this.label15);
            this.cultureInformationGroupBox.Controls.Add(this.groupBox3);
            this.cultureInformationGroupBox.Controls.Add(this.groupBox2);
            this.cultureInformationGroupBox.Controls.Add(this.label14);
            this.cultureInformationGroupBox.Controls.Add(this.label12);
            this.cultureInformationGroupBox.Controls.Add(this.label10);
            this.cultureInformationGroupBox.Controls.Add(this.label9);
            this.cultureInformationGroupBox.Location = new System.Drawing.Point(3, 3);
            this.cultureInformationGroupBox.Name = "cultureInformationGroupBox";
            this.cultureInformationGroupBox.Size = new System.Drawing.Size(556, 236);
            this.cultureInformationGroupBox.TabIndex = 0;
            this.cultureInformationGroupBox.TabStop = false;
            this.cultureInformationGroupBox.Text = "Culture Information";
            this.cultureInformationGroupBox.Visible = false;
            // 
            // cultureSuffixCheckBox
            // 
            this.cultureSuffixCheckBox.AutoSize = true;
            this.cultureSuffixCheckBox.Location = new System.Drawing.Point(248, 173);
            this.cultureSuffixCheckBox.Name = "cultureSuffixCheckBox";
            this.cultureSuffixCheckBox.Size = new System.Drawing.Size(58, 17);
            this.cultureSuffixCheckBox.TabIndex = 34;
            this.cultureSuffixCheckBox.Text = "Suffix?";
            this.cultureSuffixCheckBox.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(25, 200);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 13);
            this.label13.TabIndex = 39;
            this.label13.Text = "Female Patronym:";
            // 
            // cultureFemalePatronymTextBox
            // 
            this.cultureFemalePatronymTextBox.Location = new System.Drawing.Point(131, 197);
            this.cultureFemalePatronymTextBox.Name = "cultureFemalePatronymTextBox";
            this.cultureFemalePatronymTextBox.Size = new System.Drawing.Size(100, 20);
            this.cultureFemalePatronymTextBox.TabIndex = 33;
            // 
            // cultureMalePatronymTextBox
            // 
            this.cultureMalePatronymTextBox.Location = new System.Drawing.Point(131, 171);
            this.cultureMalePatronymTextBox.Name = "cultureMalePatronymTextBox";
            this.cultureMalePatronymTextBox.Size = new System.Drawing.Size(100, 20);
            this.cultureMalePatronymTextBox.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 174);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "Male Patronym:";
            // 
            // cultureBastardTextBox
            // 
            this.cultureBastardTextBox.Location = new System.Drawing.Point(131, 119);
            this.cultureBastardTextBox.Name = "cultureBastardTextBox";
            this.cultureBastardTextBox.Size = new System.Drawing.Size(138, 20);
            this.cultureBastardTextBox.TabIndex = 29;
            // 
            // cultureModifierTextBox
            // 
            this.cultureModifierTextBox.Location = new System.Drawing.Point(130, 145);
            this.cultureModifierTextBox.Name = "cultureModifierTextBox";
            this.cultureModifierTextBox.Size = new System.Drawing.Size(139, 20);
            this.cultureModifierTextBox.TabIndex = 31;
            // 
            // cultureDynastyPrefixTextBox
            // 
            this.cultureDynastyPrefixTextBox.Location = new System.Drawing.Point(130, 93);
            this.cultureDynastyPrefixTextBox.Name = "cultureDynastyPrefixTextBox";
            this.cultureDynastyPrefixTextBox.Size = new System.Drawing.Size(100, 20);
            this.cultureDynastyPrefixTextBox.TabIndex = 27;
            // 
            // cultureColorTextBox
            // 
            this.cultureColorTextBox.Location = new System.Drawing.Point(130, 67);
            this.cultureColorTextBox.Name = "cultureColorTextBox";
            this.cultureColorTextBox.Size = new System.Drawing.Size(100, 20);
            this.cultureColorTextBox.TabIndex = 25;
            // 
            // cultureGfxTextBox
            // 
            this.cultureGfxTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cultureGfxTextBox.AutoCompleteCustomSource.AddRange(new string[] {
            "frankishgfx",
            "englishgfx",
            "germangfx",
            "iberiangfx",
            "italiangfx",
            "celticgfx",
            "norsegfx",
            "knightsgfx",
            "republicsgfx",
            "saxongfx",
            "normangfx",
            "easternslavicgfx",
            "westernslavicgfx",
            "byzantinegfx",
            "ugricgfx",
            "arabicgfx",
            "turkishgfx",
            "mongolgfx",
            "africangfx",
            "muslimgfx",
            "easterngfx",
            "westerngfx"});
            this.cultureGfxTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cultureGfxTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cultureGfxTextBox.Location = new System.Drawing.Point(130, 41);
            this.cultureGfxTextBox.Name = "cultureGfxTextBox";
            this.cultureGfxTextBox.Size = new System.Drawing.Size(139, 20);
            this.cultureGfxTextBox.TabIndex = 23;
            // 
            // cultureNameTextBox
            // 
            this.cultureNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cultureNameTextBox.Location = new System.Drawing.Point(130, 16);
            this.cultureNameTextBox.Name = "cultureNameTextBox";
            this.cultureNameTextBox.Size = new System.Drawing.Size(139, 20);
            this.cultureNameTextBox.TabIndex = 21;
            this.cultureNameTextBox.TextChanged += new System.EventHandler(this.cultureNameTextBox_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(25, 122);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(75, 13);
            this.label22.TabIndex = 32;
            this.label22.Text = "Bastard Prefix:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(25, 148);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 30;
            this.label15.Text = "Modifier:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.cultureMotherTextBox);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.cultureMatGMTextBox);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.culturePatGMTextBox);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Location = new System.Drawing.Point(352, 125);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(247, 100);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Female Name Chance";
            // 
            // cultureMotherTextBox
            // 
            this.cultureMotherTextBox.Location = new System.Drawing.Point(199, 68);
            this.cultureMotherTextBox.Name = "cultureMotherTextBox";
            this.cultureMotherTextBox.Size = new System.Drawing.Size(42, 20);
            this.cultureMotherTextBox.TabIndex = 43;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(22, 19);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(113, 13);
            this.label21.TabIndex = 6;
            this.label21.Text = "Paternal Grandmother:";
            // 
            // cultureMatGMTextBox
            // 
            this.cultureMatGMTextBox.Location = new System.Drawing.Point(199, 42);
            this.cultureMatGMTextBox.Name = "cultureMatGMTextBox";
            this.cultureMatGMTextBox.Size = new System.Drawing.Size(42, 20);
            this.cultureMatGMTextBox.TabIndex = 41;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(22, 45);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(115, 13);
            this.label20.TabIndex = 7;
            this.label20.Text = "Maternal Grandmother:";
            // 
            // culturePatGMTextBox
            // 
            this.culturePatGMTextBox.Location = new System.Drawing.Point(199, 16);
            this.culturePatGMTextBox.Name = "culturePatGMTextBox";
            this.culturePatGMTextBox.Size = new System.Drawing.Size(42, 20);
            this.culturePatGMTextBox.TabIndex = 39;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(22, 71);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 13);
            this.label19.TabIndex = 8;
            this.label19.Text = "Mother:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cultureFatherTextBox);
            this.groupBox2.Controls.Add(this.cultureMatGFTextBox);
            this.groupBox2.Controls.Add(this.culturePatGFTextBox);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Location = new System.Drawing.Point(350, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(249, 100);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Male Name Chance";
            // 
            // cultureFatherTextBox
            // 
            this.cultureFatherTextBox.Location = new System.Drawing.Point(201, 68);
            this.cultureFatherTextBox.Name = "cultureFatherTextBox";
            this.cultureFatherTextBox.Size = new System.Drawing.Size(42, 20);
            this.cultureFatherTextBox.TabIndex = 37;
            // 
            // cultureMatGFTextBox
            // 
            this.cultureMatGFTextBox.Location = new System.Drawing.Point(201, 42);
            this.cultureMatGFTextBox.Name = "cultureMatGFTextBox";
            this.cultureMatGFTextBox.Size = new System.Drawing.Size(42, 20);
            this.cultureMatGFTextBox.TabIndex = 35;
            // 
            // culturePatGFTextBox
            // 
            this.culturePatGFTextBox.Location = new System.Drawing.Point(201, 16);
            this.culturePatGFTextBox.Name = "culturePatGFTextBox";
            this.culturePatGFTextBox.Size = new System.Drawing.Size(42, 20);
            this.culturePatGFTextBox.TabIndex = 33;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(24, 71);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(40, 13);
            this.label18.TabIndex = 2;
            this.label18.Text = "Father:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(24, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(110, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "Maternal Grandfather:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(24, 19);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(108, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Paternal Grandfather:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(25, 94);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Dynasty Prefix:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Color:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Graphical Culture:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Name:";
            // 
            // cultureNamesGroupBox
            // 
            this.cultureNamesGroupBox.Controls.Add(this.splitContainer3);
            this.cultureNamesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cultureNamesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.cultureNamesGroupBox.Name = "cultureNamesGroupBox";
            this.cultureNamesGroupBox.Size = new System.Drawing.Size(611, 226);
            this.cultureNamesGroupBox.TabIndex = 0;
            this.cultureNamesGroupBox.TabStop = false;
            this.cultureNamesGroupBox.Text = "Culture Names (Male left, Female right)";
            this.cultureNamesGroupBox.Visible = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 16);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.cultureMaleNamesRichTextBox);
            this.splitContainer3.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.cultureFemaleNamesRichTextBox);
            this.splitContainer3.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer3.Size = new System.Drawing.Size(605, 207);
            this.splitContainer3.SplitterDistance = 271;
            this.splitContainer3.TabIndex = 0;
            // 
            // cultureMaleNamesRichTextBox
            // 
            this.cultureMaleNamesRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cultureMaleNamesRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.cultureMaleNamesRichTextBox.Name = "cultureMaleNamesRichTextBox";
            this.cultureMaleNamesRichTextBox.Size = new System.Drawing.Size(271, 207);
            this.cultureMaleNamesRichTextBox.TabIndex = 0;
            this.cultureMaleNamesRichTextBox.Text = "";
            this.cultureMaleNamesRichTextBox.WordWrap = false;
            // 
            // cultureFemaleNamesRichTextBox
            // 
            this.cultureFemaleNamesRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cultureFemaleNamesRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.cultureFemaleNamesRichTextBox.Name = "cultureFemaleNamesRichTextBox";
            this.cultureFemaleNamesRichTextBox.Size = new System.Drawing.Size(330, 207);
            this.cultureFemaleNamesRichTextBox.TabIndex = 0;
            this.cultureFemaleNamesRichTextBox.Text = "";
            this.cultureFemaleNamesRichTextBox.WordWrap = false;
            // 
            // tabDynasties
            // 
            this.tabDynasties.BackColor = System.Drawing.Color.Transparent;
            this.tabDynasties.Controls.Add(this.textBoxDynastyFilterByCulture);
            this.tabDynasties.Controls.Add(this.label8);
            this.tabDynasties.Controls.Add(this.textBoxDynastyFilterByName);
            this.tabDynasties.Controls.Add(this.label7);
            this.tabDynasties.Controls.Add(this.buttonDynastyClearFilter);
            this.tabDynasties.Controls.Add(this.buttonDynastyFilter);
            this.tabDynasties.Controls.Add(this.label6);
            this.tabDynasties.Controls.Add(this.textBoxDynastyFilterByID);
            this.tabDynasties.Controls.Add(this.dynastyGridView);
            this.tabDynasties.Location = new System.Drawing.Point(4, 22);
            this.tabDynasties.Name = "tabDynasties";
            this.tabDynasties.Padding = new System.Windows.Forms.Padding(3);
            this.tabDynasties.Size = new System.Drawing.Size(773, 484);
            this.tabDynasties.TabIndex = 1;
            this.tabDynasties.Text = "Dynasties";
            // 
            // textBoxDynastyFilterByCulture
            // 
            this.textBoxDynastyFilterByCulture.Location = new System.Drawing.Point(444, 8);
            this.textBoxDynastyFilterByCulture.Name = "textBoxDynastyFilterByCulture";
            this.textBoxDynastyFilterByCulture.Size = new System.Drawing.Size(140, 20);
            this.textBoxDynastyFilterByCulture.TabIndex = 8;
            this.textBoxDynastyFilterByCulture.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDynastyFilterByCulture_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(380, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "By Culture:";
            // 
            // textBoxDynastyFilterByName
            // 
            this.textBoxDynastyFilterByName.Location = new System.Drawing.Point(237, 8);
            this.textBoxDynastyFilterByName.Name = "textBoxDynastyFilterByName";
            this.textBoxDynastyFilterByName.Size = new System.Drawing.Size(137, 20);
            this.textBoxDynastyFilterByName.TabIndex = 6;
            this.textBoxDynastyFilterByName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDynastyFilterByName_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(178, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "By Name:";
            // 
            // buttonDynastyClearFilter
            // 
            this.buttonDynastyClearFilter.Location = new System.Drawing.Point(681, 7);
            this.buttonDynastyClearFilter.Name = "buttonDynastyClearFilter";
            this.buttonDynastyClearFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonDynastyClearFilter.TabIndex = 4;
            this.buttonDynastyClearFilter.Text = "Clear Filters";
            this.buttonDynastyClearFilter.UseVisualStyleBackColor = true;
            this.buttonDynastyClearFilter.Click += new System.EventHandler(this.buttonDynastyClearFilter_Click);
            // 
            // buttonDynastyFilter
            // 
            this.buttonDynastyFilter.Location = new System.Drawing.Point(600, 6);
            this.buttonDynastyFilter.Name = "buttonDynastyFilter";
            this.buttonDynastyFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonDynastyFilter.TabIndex = 3;
            this.buttonDynastyFilter.Text = "Filter";
            this.buttonDynastyFilter.UseVisualStyleBackColor = true;
            this.buttonDynastyFilter.Click += new System.EventHandler(this.buttonDynastyFilterByID_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Filter by ID:";
            // 
            // textBoxDynastyFilterByID
            // 
            this.textBoxDynastyFilterByID.Location = new System.Drawing.Point(72, 8);
            this.textBoxDynastyFilterByID.Name = "textBoxDynastyFilterByID";
            this.textBoxDynastyFilterByID.Size = new System.Drawing.Size(100, 20);
            this.textBoxDynastyFilterByID.TabIndex = 1;
            this.textBoxDynastyFilterByID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDynastyFilterByID_KeyDown);
            // 
            // dynastyGridView
            // 
            this.dynastyGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynastyGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dynastyGridView.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dynastyGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dynastyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dynastyGridView.Location = new System.Drawing.Point(3, 38);
            this.dynastyGridView.Name = "dynastyGridView";
            this.dynastyGridView.Size = new System.Drawing.Size(767, 443);
            this.dynastyGridView.TabIndex = 0;
            // 
            // tabCharacters
            // 
            this.tabCharacters.Controls.Add(this.splitContainer6);
            this.tabCharacters.Location = new System.Drawing.Point(4, 22);
            this.tabCharacters.Name = "tabCharacters";
            this.tabCharacters.Padding = new System.Windows.Forms.Padding(3);
            this.tabCharacters.Size = new System.Drawing.Size(773, 484);
            this.tabCharacters.TabIndex = 5;
            this.tabCharacters.Text = "Characters";
            this.tabCharacters.UseVisualStyleBackColor = true;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(3, 3);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.characterFilesFilter);
            this.splitContainer6.Panel1.Controls.Add(this.characterFilesListBox);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(767, 478);
            this.splitContainer6.SplitterDistance = 150;
            this.splitContainer6.TabIndex = 0;
            // 
            // characterFilesFilter
            // 
            this.characterFilesFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterFilesFilter.Location = new System.Drawing.Point(3, 3);
            this.characterFilesFilter.Name = "characterFilesFilter";
            this.characterFilesFilter.Size = new System.Drawing.Size(144, 20);
            this.characterFilesFilter.TabIndex = 0;
            this.characterFilesFilter.TextChanged += new System.EventHandler(this.characterFilesFilter_TextChanged);
            // 
            // characterFilesListBox
            // 
            this.characterFilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterFilesListBox.FormattingEnabled = true;
            this.characterFilesListBox.Location = new System.Drawing.Point(3, 26);
            this.characterFilesListBox.Name = "characterFilesListBox";
            this.characterFilesListBox.Size = new System.Drawing.Size(144, 446);
            this.characterFilesListBox.TabIndex = 0;
            this.characterFilesListBox.SelectedIndexChanged += new System.EventHandler(this.characterFilesListBox_SelectedIndexChanged);
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.characterFilter);
            this.splitContainer7.Panel1.Controls.Add(this.characterListBox);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.AutoScroll = true;
            this.splitContainer7.Panel2.Controls.Add(this.characterPropertyGrid);
            this.splitContainer7.Size = new System.Drawing.Size(613, 478);
            this.splitContainer7.SplitterDistance = 150;
            this.splitContainer7.TabIndex = 0;
            // 
            // characterListBox
            // 
            this.characterListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterListBox.FormattingEnabled = true;
            this.characterListBox.Location = new System.Drawing.Point(0, 26);
            this.characterListBox.Name = "characterListBox";
            this.characterListBox.Size = new System.Drawing.Size(150, 446);
            this.characterListBox.TabIndex = 0;
            this.characterListBox.SelectedIndexChanged += new System.EventHandler(this.characterListBox_SelectedIndexChanged);
            // 
            // characterPropertyGrid
            // 
            this.characterPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.characterPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.characterPropertyGrid.Name = "characterPropertyGrid";
            this.characterPropertyGrid.Size = new System.Drawing.Size(459, 478);
            this.characterPropertyGrid.TabIndex = 0;
            // 
            // characterFilesContextMenuStrip
            // 
            this.characterFilesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.characterFilesMenuAdd,
            this.editSelectedFileToolStripMenuItem,
            this.deleteSelectedFileToolStripMenuItem});
            this.characterFilesContextMenuStrip.Name = "characterFilesContextMenuStrip";
            this.characterFilesContextMenuStrip.Size = new System.Drawing.Size(176, 70);
            // 
            // characterFilesMenuAdd
            // 
            this.characterFilesMenuAdd.Name = "characterFilesMenuAdd";
            this.characterFilesMenuAdd.Size = new System.Drawing.Size(175, 22);
            this.characterFilesMenuAdd.Text = "Add File...";
            this.characterFilesMenuAdd.Click += new System.EventHandler(this.characterFilesMenuAdd_Click);
            // 
            // editSelectedFileToolStripMenuItem
            // 
            this.editSelectedFileToolStripMenuItem.Name = "editSelectedFileToolStripMenuItem";
            this.editSelectedFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.editSelectedFileToolStripMenuItem.Text = "Edit Selected File...";
            this.editSelectedFileToolStripMenuItem.Click += new System.EventHandler(this.editSelectedFileToolStripMenuItem_Click);
            // 
            // deleteSelectedFileToolStripMenuItem
            // 
            this.deleteSelectedFileToolStripMenuItem.Name = "deleteSelectedFileToolStripMenuItem";
            this.deleteSelectedFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.deleteSelectedFileToolStripMenuItem.Text = "Delete Selected File";
            this.deleteSelectedFileToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedFileToolStripMenuItem_Click);
            // 
            // dynastyBackgroundWorker
            // 
            this.dynastyBackgroundWorker.WorkerReportsProgress = true;
            this.dynastyBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dynastyBackgroundWorker_DoWork);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // cultureBackgroundWorker
            // 
            this.cultureBackgroundWorker.WorkerReportsProgress = true;
            this.cultureBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.cultureBackgroundWorker_DoWork);
            // 
            // cultureSubContextMenuStrip
            // 
            this.cultureSubContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cultureStripAdd,
            this.cultureStripRemove});
            this.cultureSubContextMenuStrip.Name = "cultureSubContextMenuStrip";
            this.cultureSubContextMenuStrip.Size = new System.Drawing.Size(139, 48);
            // 
            // cultureStripAdd
            // 
            this.cultureStripAdd.Name = "cultureStripAdd";
            this.cultureStripAdd.Size = new System.Drawing.Size(138, 22);
            this.cultureStripAdd.Text = "Add Culture";
            this.cultureStripAdd.Click += new System.EventHandler(this.cultureStripAdd_Click);
            // 
            // cultureStripRemove
            // 
            this.cultureStripRemove.Name = "cultureStripRemove";
            this.cultureStripRemove.Size = new System.Drawing.Size(138, 22);
            this.cultureStripRemove.Text = "Remove";
            this.cultureStripRemove.Click += new System.EventHandler(this.cultureStripRemove_Click);
            // 
            // cultureRootContextMenuStrip
            // 
            this.cultureRootContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cultureGroupStripMenuAdd});
            this.cultureRootContextMenuStrip.Name = "cultureRootContextMenuStrip";
            this.cultureRootContextMenuStrip.Size = new System.Drawing.Size(175, 26);
            // 
            // cultureGroupStripMenuAdd
            // 
            this.cultureGroupStripMenuAdd.Name = "cultureGroupStripMenuAdd";
            this.cultureGroupStripMenuAdd.Size = new System.Drawing.Size(174, 22);
            this.cultureGroupStripMenuAdd.Text = "Add Culture Group";
            this.cultureGroupStripMenuAdd.Click += new System.EventHandler(this.cultureGroupStripMenuAdd_Click);
            // 
            // cultureContextMenuStrip
            // 
            this.cultureContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cultureToolStripMenuRemove});
            this.cultureContextMenuStrip.Name = "contextMenuStrip1";
            this.cultureContextMenuStrip.Size = new System.Drawing.Size(118, 26);
            // 
            // cultureToolStripMenuRemove
            // 
            this.cultureToolStripMenuRemove.Name = "cultureToolStripMenuRemove";
            this.cultureToolStripMenuRemove.Size = new System.Drawing.Size(117, 22);
            this.cultureToolStripMenuRemove.Text = "Remove";
            this.cultureToolStripMenuRemove.Click += new System.EventHandler(this.cultureToolStripMenuRemove_Click);
            // 
            // characterBackgroundWorker
            // 
            this.characterBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.characterBackgroundWorker_DoWork);
            // 
            // characterFilter
            // 
            this.characterFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterFilter.Location = new System.Drawing.Point(-1, 3);
            this.characterFilter.Name = "characterFilter";
            this.characterFilter.Size = new System.Drawing.Size(151, 20);
            this.characterFilter.TabIndex = 1;
            this.characterFilter.TextChanged += new System.EventHandler(this.characterFilter_TextChanged);
            // 
            // CK2Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "CK2Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CK2 Modder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabModProperties.ResumeLayout(false);
            this.tabModProperties.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.culturesTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.cultureInformationGroupBox.ResumeLayout(false);
            this.cultureInformationGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.cultureNamesGroupBox.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabDynasties.ResumeLayout(false);
            this.tabDynasties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dynastyGridView)).EndInit();
            this.tabCharacters.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel1.PerformLayout();
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.characterFilesContextMenuStrip.ResumeLayout(false);
            this.cultureSubContextMenuStrip.ResumeLayout(false);
            this.cultureRootContextMenuStrip.ResumeLayout(false);
            this.cultureContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel workingLocationStripStatusLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDynasties;
        private System.Windows.Forms.DataGridView dynastyGridView;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.ComponentModel.BackgroundWorker dynastyBackgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modToolStripMenuItem;
        private System.Windows.Forms.TabPage tabModProperties;
        private System.Windows.Forms.TextBox textBoxModName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxModRawOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBoxDependencies;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonImportDynasties;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonImportCharacters;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workingLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeModToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeWithoutSavingToolStripMenuItem;
        private System.Windows.Forms.Button buttonDynastyFilter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxDynastyFilterByID;
        private System.Windows.Forms.Button buttonDynastyClearFilter;
        private System.Windows.Forms.TextBox textBoxDynastyFilterByName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxDynastyFilterByCulture;
        private System.Windows.Forms.Label label8;
        private System.ComponentModel.BackgroundWorker cultureBackgroundWorker;
        private System.Windows.Forms.TabPage culturesTabPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView cultureTreeView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox cultureInformationGroupBox;
        private System.Windows.Forms.GroupBox cultureNamesGroupBox;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox cultureMaleNamesRichTextBox;
        private System.Windows.Forms.RichTextBox cultureFemaleNamesRichTextBox;
        private System.Windows.Forms.TextBox cultureBastardTextBox;
        private System.Windows.Forms.TextBox cultureModifierTextBox;
        private System.Windows.Forms.TextBox cultureDynastyPrefixTextBox;
        private System.Windows.Forms.TextBox cultureColorTextBox;
        private System.Windows.Forms.TextBox cultureGfxTextBox;
        private System.Windows.Forms.TextBox cultureNameTextBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox cultureMotherTextBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox cultureMatGMTextBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox culturePatGMTextBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox cultureFatherTextBox;
        private System.Windows.Forms.TextBox cultureMatGFTextBox;
        private System.Windows.Forms.TextBox culturePatGFTextBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ContextMenuStrip cultureSubContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cultureStripAdd;
        private System.Windows.Forms.ToolStripMenuItem cultureStripRemove;
        private System.Windows.Forms.ContextMenuStrip cultureRootContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cultureGroupStripMenuAdd;
        private System.Windows.Forms.ContextMenuStrip cultureContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cultureToolStripMenuRemove;
        private System.Windows.Forms.TextBox cultureFemalePatronymTextBox;
        private System.Windows.Forms.TextBox cultureMalePatronymTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox cultureSuffixCheckBox;
        private System.Windows.Forms.Button buttonImportCultures;
        private System.ComponentModel.BackgroundWorker characterBackgroundWorker;
        private System.Windows.Forms.ContextMenuStrip characterFilesContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem characterFilesMenuAdd;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSelectedFileToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button addPathButton;
        private System.Windows.Forms.ComboBox replacePathsComboBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ListBox replacePathsListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox userDirectoryTextBox;
        private System.Windows.Forms.SaveFileDialog newModDialog;
        private System.Windows.Forms.TabPage tabCharacters;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.ListBox characterFilesListBox;
        private System.Windows.Forms.ListBox characterListBox;
        private System.Windows.Forms.PropertyGrid characterPropertyGrid;
        private System.Windows.Forms.TextBox characterFilesFilter;
        private System.Windows.Forms.TextBox characterFilter;
    }
}

