namespace NullScripter.GUI
{
    partial class MainForm
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
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_New = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_New_Project = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_New_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open_Project = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Build = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Build_Compile = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Setting = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Splitter2 = new System.Windows.Forms.SplitContainer();
            this.FileTreeView = new System.Windows.Forms.TreeView();
            this.Splitter1 = new System.Windows.Forms.SplitContainer();
            this.ScriptTab = new System.Windows.Forms.TabControl();
            this.StatusBox = new System.Windows.Forms.TextBox();
            this.MenuStrip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Splitter2)).BeginInit();
            this.Splitter2.Panel1.SuspendLayout();
            this.Splitter2.Panel2.SuspendLayout();
            this.Splitter2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Splitter1)).BeginInit();
            this.Splitter1.Panel1.SuspendLayout();
            this.Splitter1.Panel2.SuspendLayout();
            this.Splitter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Build,
            this.Menu_Setting});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(584, 24);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_New,
            this.Menu_File_Open});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // Menu_File_New
            // 
            this.Menu_File_New.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_New_Project,
            this.Menu_File_New_File});
            this.Menu_File_New.Name = "Menu_File_New";
            this.Menu_File_New.Size = new System.Drawing.Size(103, 22);
            this.Menu_File_New.Text = "New";
            // 
            // Menu_File_New_Project
            // 
            this.Menu_File_New_Project.Name = "Menu_File_New_Project";
            this.Menu_File_New_Project.Size = new System.Drawing.Size(111, 22);
            this.Menu_File_New_Project.Text = "Project";
            this.Menu_File_New_Project.Click += new System.EventHandler(this.Menu_File_New_Project_Click);
            // 
            // Menu_File_New_File
            // 
            this.Menu_File_New_File.Name = "Menu_File_New_File";
            this.Menu_File_New_File.Size = new System.Drawing.Size(111, 22);
            this.Menu_File_New_File.Text = "File";
            this.Menu_File_New_File.Click += new System.EventHandler(this.Menu_File_New_File_Click);
            // 
            // Menu_File_Open
            // 
            this.Menu_File_Open.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_Open_Project,
            this.Menu_File_Open_File});
            this.Menu_File_Open.Name = "Menu_File_Open";
            this.Menu_File_Open.Size = new System.Drawing.Size(103, 22);
            this.Menu_File_Open.Text = "Open";
            // 
            // Menu_File_Open_Project
            // 
            this.Menu_File_Open_Project.Name = "Menu_File_Open_Project";
            this.Menu_File_Open_Project.Size = new System.Drawing.Size(111, 22);
            this.Menu_File_Open_Project.Text = "Project";
            this.Menu_File_Open_Project.Click += new System.EventHandler(this.Menu_File_Open_Project_Click);
            // 
            // Menu_File_Open_File
            // 
            this.Menu_File_Open_File.Name = "Menu_File_Open_File";
            this.Menu_File_Open_File.Size = new System.Drawing.Size(111, 22);
            this.Menu_File_Open_File.Text = "File";
            this.Menu_File_Open_File.Click += new System.EventHandler(this.Menu_File_Open_File_Click);
            // 
            // Menu_Build
            // 
            this.Menu_Build.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Build_Compile});
            this.Menu_Build.Name = "Menu_Build";
            this.Menu_Build.Size = new System.Drawing.Size(46, 20);
            this.Menu_Build.Text = "Build";
            // 
            // Menu_Build_Compile
            // 
            this.Menu_Build_Compile.Name = "Menu_Build_Compile";
            this.Menu_Build_Compile.Size = new System.Drawing.Size(119, 22);
            this.Menu_Build_Compile.Text = "Compile";
            this.Menu_Build_Compile.Click += new System.EventHandler(this.Menu_Build_Compile_Click);
            // 
            // Menu_Setting
            // 
            this.Menu_Setting.Name = "Menu_Setting";
            this.Menu_Setting.Size = new System.Drawing.Size(57, 20);
            this.Menu_Setting.Text = "Setting";
            this.Menu_Setting.Click += new System.EventHandler(this.Menu_Setting_Click);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 339);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(584, 22);
            this.StatusStrip.TabIndex = 2;
            this.StatusStrip.Text = "StatusStrip";
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Splitter2
            // 
            this.Splitter2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splitter2.Location = new System.Drawing.Point(0, 24);
            this.Splitter2.Name = "Splitter2";
            // 
            // Splitter2.Panel1
            // 
            this.Splitter2.Panel1.Controls.Add(this.FileTreeView);
            // 
            // Splitter2.Panel2
            // 
            this.Splitter2.Panel2.Controls.Add(this.Splitter1);
            this.Splitter2.Size = new System.Drawing.Size(584, 315);
            this.Splitter2.SplitterDistance = 194;
            this.Splitter2.TabIndex = 3;
            // 
            // FileTreeView
            // 
            this.FileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTreeView.Location = new System.Drawing.Point(0, 0);
            this.FileTreeView.Name = "FileTreeView";
            this.FileTreeView.Size = new System.Drawing.Size(194, 315);
            this.FileTreeView.TabIndex = 0;
            this.FileTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FileTreeView_NodeMouseClick);
            this.FileTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FileTreeView_NodeMouseDoubleClick);
            // 
            // Splitter1
            // 
            this.Splitter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splitter1.Location = new System.Drawing.Point(0, 0);
            this.Splitter1.Name = "Splitter1";
            this.Splitter1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Splitter1.Panel1
            // 
            this.Splitter1.Panel1.Controls.Add(this.ScriptTab);
            // 
            // Splitter1.Panel2
            // 
            this.Splitter1.Panel2.Controls.Add(this.StatusBox);
            this.Splitter1.Size = new System.Drawing.Size(386, 315);
            this.Splitter1.SplitterDistance = 204;
            this.Splitter1.TabIndex = 1;
            // 
            // ScriptTab
            // 
            this.ScriptTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptTab.Location = new System.Drawing.Point(0, 0);
            this.ScriptTab.Name = "ScriptTab";
            this.ScriptTab.SelectedIndex = 0;
            this.ScriptTab.Size = new System.Drawing.Size(386, 204);
            this.ScriptTab.TabIndex = 0;
            // 
            // StatusBox
            // 
            this.StatusBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusBox.Location = new System.Drawing.Point(0, 0);
            this.StatusBox.Multiline = true;
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.ReadOnly = true;
            this.StatusBox.Size = new System.Drawing.Size(386, 107);
            this.StatusBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.Splitter2);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.MenuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainForm";
            this.Text = "NullScripter";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.Splitter2.Panel1.ResumeLayout(false);
            this.Splitter2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Splitter2)).EndInit();
            this.Splitter2.ResumeLayout(false);
            this.Splitter1.Panel1.ResumeLayout(false);
            this.Splitter1.Panel2.ResumeLayout(false);
            this.Splitter1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Splitter1)).EndInit();
            this.Splitter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_Build;
        private System.Windows.Forms.ToolStripMenuItem Menu_Build_Compile;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_New;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_New_Project;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_New_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open_Project;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open_File;
        private System.Windows.Forms.SplitContainer Splitter2;
        private System.Windows.Forms.SplitContainer Splitter1;
        private System.Windows.Forms.TextBox StatusBox;
        private System.Windows.Forms.TreeView FileTreeView;
        private System.Windows.Forms.TabControl ScriptTab;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
        private System.Windows.Forms.ToolStripMenuItem Menu_Setting;

    }
}

