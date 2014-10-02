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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CK2Form));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWithoutSavingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
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
            this.mainSplitPanel = new System.Windows.Forms.SplitContainer();
            this.modFilesTree = new System.Windows.Forms.TreeView();
            this.iconsList = new System.Windows.Forms.ImageList(this.components);
            this.secondarySplitPanel = new System.Windows.Forms.SplitContainer();
            this.dataListBox = new System.Windows.Forms.ListBox();
            this.dataFilter = new System.Windows.Forms.TextBox();
            this.dataTextEditor = new ScintillaNET.Scintilla();
            this.modClosedPanel = new System.Windows.Forms.Panel();
            this.loadModButton = new System.Windows.Forms.Button();
            this.newModButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mainMenuStrip.SuspendLayout();
            this.characterFilesContextMenuStrip.SuspendLayout();
            this.cultureSubContextMenuStrip.SuspendLayout();
            this.cultureRootContextMenuStrip.SuspendLayout();
            this.cultureContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitPanel)).BeginInit();
            this.mainSplitPanel.Panel1.SuspendLayout();
            this.mainSplitPanel.Panel2.SuspendLayout();
            this.mainSplitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secondarySplitPanel)).BeginInit();
            this.secondarySplitPanel.Panel1.SuspendLayout();
            this.secondarySplitPanel.Panel2.SuspendLayout();
            this.secondarySplitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTextEditor)).BeginInit();
            this.modClosedPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newModToolStripMenuItem,
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
            // newModToolStripMenuItem
            // 
            this.newModToolStripMenuItem.Name = "newModToolStripMenuItem";
            this.newModToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newModToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newModToolStripMenuItem.Text = "New Mod...";
            this.newModToolStripMenuItem.Click += new System.EventHandler(this.newModToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openToolStripMenuItem.Text = "Open...";
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
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
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
            // 
            // editSelectedFileToolStripMenuItem
            // 
            this.editSelectedFileToolStripMenuItem.Name = "editSelectedFileToolStripMenuItem";
            this.editSelectedFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.editSelectedFileToolStripMenuItem.Text = "Edit Selected File...";
            // 
            // deleteSelectedFileToolStripMenuItem
            // 
            this.deleteSelectedFileToolStripMenuItem.Name = "deleteSelectedFileToolStripMenuItem";
            this.deleteSelectedFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.deleteSelectedFileToolStripMenuItem.Text = "Delete Selected File";
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
            // 
            // cultureStripRemove
            // 
            this.cultureStripRemove.Name = "cultureStripRemove";
            this.cultureStripRemove.Size = new System.Drawing.Size(138, 22);
            this.cultureStripRemove.Text = "Remove";
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
            // 
            // characterBackgroundWorker
            // 
            this.characterBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.characterBackgroundWorker_DoWork);
            // 
            // mainSplitPanel
            // 
            this.mainSplitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainSplitPanel.Location = new System.Drawing.Point(0, 27);
            this.mainSplitPanel.Name = "mainSplitPanel";
            // 
            // mainSplitPanel.Panel1
            // 
            this.mainSplitPanel.Panel1.Controls.Add(this.modFilesTree);
            // 
            // mainSplitPanel.Panel2
            // 
            this.mainSplitPanel.Panel2.Controls.Add(this.secondarySplitPanel);
            this.mainSplitPanel.Size = new System.Drawing.Size(784, 510);
            this.mainSplitPanel.SplitterDistance = 100;
            this.mainSplitPanel.TabIndex = 5;
            // 
            // modFilesTree
            // 
            this.modFilesTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modFilesTree.ImageIndex = 0;
            this.modFilesTree.ImageList = this.iconsList;
            this.modFilesTree.Location = new System.Drawing.Point(3, 3);
            this.modFilesTree.Name = "modFilesTree";
            this.modFilesTree.SelectedImageIndex = 0;
            this.modFilesTree.Size = new System.Drawing.Size(97, 498);
            this.modFilesTree.TabIndex = 1;
            this.modFilesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.modFilesTree_AfterSelect);
            this.modFilesTree.DoubleClick += new System.EventHandler(this.modFilesTree_DoubleClick);
            // 
            // iconsList
            // 
            this.iconsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsList.ImageStream")));
            this.iconsList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconsList.Images.SetKeyName(0, "Folder-icon.png");
            this.iconsList.Images.SetKeyName(1, "Text-Document-icon.png");
            // 
            // secondarySplitPanel
            // 
            this.secondarySplitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secondarySplitPanel.Location = new System.Drawing.Point(-1, 0);
            this.secondarySplitPanel.Name = "secondarySplitPanel";
            // 
            // secondarySplitPanel.Panel1
            // 
            this.secondarySplitPanel.Panel1.Controls.Add(this.dataListBox);
            this.secondarySplitPanel.Panel1.Controls.Add(this.dataFilter);
            // 
            // secondarySplitPanel.Panel2
            // 
            this.secondarySplitPanel.Panel2.Controls.Add(this.dataTextEditor);
            this.secondarySplitPanel.Size = new System.Drawing.Size(678, 510);
            this.secondarySplitPanel.SplitterDistance = 100;
            this.secondarySplitPanel.TabIndex = 0;
            // 
            // dataListBox
            // 
            this.dataListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataListBox.FormattingEnabled = true;
            this.dataListBox.Location = new System.Drawing.Point(3, 28);
            this.dataListBox.Name = "dataListBox";
            this.dataListBox.Size = new System.Drawing.Size(94, 472);
            this.dataListBox.TabIndex = 1;
            this.dataListBox.SelectedIndexChanged += new System.EventHandler(this.dataListBox_SelectedIndexChanged);
            // 
            // dataFilter
            // 
            this.dataFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataFilter.Location = new System.Drawing.Point(3, 3);
            this.dataFilter.Name = "dataFilter";
            this.dataFilter.Size = new System.Drawing.Size(94, 20);
            this.dataFilter.TabIndex = 0;
            this.dataFilter.TextChanged += new System.EventHandler(this.dataFilter_TextChanged);
            // 
            // dataTextEditor
            // 
            this.dataTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTextEditor.Location = new System.Drawing.Point(3, 3);
            this.dataTextEditor.Name = "dataTextEditor";
            this.dataTextEditor.Size = new System.Drawing.Size(568, 498);
            this.dataTextEditor.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.dataTextEditor.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.dataTextEditor.TabIndex = 0;
            // 
            // modClosedPanel
            // 
            this.modClosedPanel.Controls.Add(this.loadModButton);
            this.modClosedPanel.Controls.Add(this.newModButton);
            this.modClosedPanel.Controls.Add(this.label2);
            this.modClosedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modClosedPanel.Location = new System.Drawing.Point(0, 0);
            this.modClosedPanel.Name = "modClosedPanel";
            this.modClosedPanel.Size = new System.Drawing.Size(784, 562);
            this.modClosedPanel.TabIndex = 7;
            // 
            // loadModButton
            // 
            this.loadModButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadModButton.Location = new System.Drawing.Point(401, 235);
            this.loadModButton.Name = "loadModButton";
            this.loadModButton.Size = new System.Drawing.Size(103, 23);
            this.loadModButton.TabIndex = 2;
            this.loadModButton.Text = "Load Mod...";
            this.loadModButton.UseVisualStyleBackColor = true;
            this.loadModButton.Click += new System.EventHandler(this.loadModButton_Click);
            // 
            // newModButton
            // 
            this.newModButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.newModButton.Location = new System.Drawing.Point(287, 235);
            this.newModButton.Name = "newModButton";
            this.newModButton.Size = new System.Drawing.Size(108, 23);
            this.newModButton.TabIndex = 1;
            this.newModButton.Text = "Create New...";
            this.newModButton.UseVisualStyleBackColor = true;
            this.newModButton.Click += new System.EventHandler(this.newModButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(760, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Create a New Mod or Load an Existing one";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CK2Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.mainSplitPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.modClosedPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "CK2Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CK2 Modder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.characterFilesContextMenuStrip.ResumeLayout(false);
            this.cultureSubContextMenuStrip.ResumeLayout(false);
            this.cultureRootContextMenuStrip.ResumeLayout(false);
            this.cultureContextMenuStrip.ResumeLayout(false);
            this.mainSplitPanel.Panel1.ResumeLayout(false);
            this.mainSplitPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitPanel)).EndInit();
            this.mainSplitPanel.ResumeLayout(false);
            this.secondarySplitPanel.Panel1.ResumeLayout(false);
            this.secondarySplitPanel.Panel1.PerformLayout();
            this.secondarySplitPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.secondarySplitPanel)).EndInit();
            this.secondarySplitPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTextEditor)).EndInit();
            this.modClosedPanel.ResumeLayout(false);
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
        private System.ComponentModel.BackgroundWorker dynastyBackgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeModToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeWithoutSavingToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker cultureBackgroundWorker;
        private System.Windows.Forms.ContextMenuStrip cultureSubContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cultureStripAdd;
        private System.Windows.Forms.ToolStripMenuItem cultureStripRemove;
        private System.Windows.Forms.ContextMenuStrip cultureRootContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cultureGroupStripMenuAdd;
        private System.Windows.Forms.ContextMenuStrip cultureContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cultureToolStripMenuRemove;
        private System.ComponentModel.BackgroundWorker characterBackgroundWorker;
        private System.Windows.Forms.ContextMenuStrip characterFilesContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem characterFilesMenuAdd;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSelectedFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog newModDialog;
        private System.Windows.Forms.ToolStripMenuItem newModToolStripMenuItem;
        private System.Windows.Forms.SplitContainer mainSplitPanel;
        private System.Windows.Forms.SplitContainer secondarySplitPanel;
        private System.Windows.Forms.TextBox dataFilter;
        private System.Windows.Forms.ListBox dataListBox;
        private ScintillaNET.Scintilla dataTextEditor;
        private System.Windows.Forms.Panel modClosedPanel;
        private System.Windows.Forms.Button loadModButton;
        private System.Windows.Forms.Button newModButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView modFilesTree;
        private System.Windows.Forms.ImageList iconsList;
    }
}

