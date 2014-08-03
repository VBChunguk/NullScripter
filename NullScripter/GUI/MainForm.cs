using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using NullScripter.Script;

namespace NullScripter.GUI
{
    public partial class MainForm : Form
    {
        #region Declarement
        private Compiler Comp;
        private NullScriptProject nsp;
        private List<string> OpenedFile;

        public NullScripterSetting nss;
        #endregion

        public MainForm()
        {
            #region Initializing
            InitializeComponent();

            OpenedFile = new List<string>();
            Comp = new Compiler();
            nss = new NullScripterSetting();
            #endregion
            #region Re-Setting MenuStrip names
            this.Menu_File.Text = StringMap.Dic[StringMap.Status.Menu_File];
            this.Menu_File_New_Project.Text = StringMap.Dic[StringMap.Status.Menu_Project];
            this.Menu_File_New_File.Text = StringMap.Dic[StringMap.Status.Menu_File];
            this.Menu_File_New.Text = StringMap.Dic[StringMap.Status.Menu_New];
            this.Menu_File_Open.Text = StringMap.Dic[StringMap.Status.Menu_Open];
            this.Menu_File_Open_Project.Text = StringMap.Dic[StringMap.Status.Menu_Project];
            this.Menu_File_Open_File.Text = StringMap.Dic[StringMap.Status.Menu_File];
            this.Menu_Build.Text = StringMap.Dic[StringMap.Status.Menu_Build];
            this.Menu_Build_Compile.Text = StringMap.Dic[StringMap.Status.Menu_Compile];
            this.Menu_Setting.Text= StringMap.Dic[StringMap.Status.Menu_Setting];
            #endregion
            #region FileTreeView Context Menu Setting
            FileTreeView.ContextMenu = new ContextMenu();
            FileTreeView.ContextMenu.MenuItems.Add(StringMap.Dic[StringMap.Status.Menu_Add]);
            FileTreeView.ContextMenu.MenuItems[0].Click += TreeViewMenu_Add_Click;
            #endregion
            #region ScriptTab Context Menu Setting
            ScriptTab.ContextMenu = new ContextMenu();
            ScriptTab.ContextMenu.MenuItems.Add(StringMap.Dic[StringMap.Status.Menu_Close]);
            ScriptTab.ContextMenu.MenuItems[0].Click += ScriptTab_Close_Click;
            #endregion

            #region NullScripter is on Ready Status!
            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Ready];
            #endregion
        }

        void MainForm_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Compile()
        {
            #region Scanning .nsp File
            if (nsp == null)
                return;

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Compiling];
            bool crashed = false;

            string script = null;
#warning Scaffolding
            StreamWriter sw = new StreamWriter(Path.Combine(nsp.path, "output.txt"));
            Stopwatch stw = new Stopwatch();
            stw.Start();
            
            string startfile = null;
            foreach (string e in Directory.EnumerateFiles(Path.Combine(nsp.path, "Script")))
            {
                StreamReader sr = new StreamReader(e);
                if (sr.ReadToEnd().Contains("<start>"))
                    startfile = e;
                sr.Close();
            }
            #endregion
            #region Compile Script
            try
            {
                StreamReader sr = new StreamReader(startfile);
                script = Regex.Replace(sr.ReadToEnd() + "\r\n", "<start>", "");
                sr.Close();

                foreach (string e in Directory.EnumerateFiles(Path.Combine(nsp.path, "Script")))
                {
                    if (e == startfile)
                        continue;

                    sr = new StreamReader(e);
                    script += sr.ReadToEnd() + "\r\n";
                    sr.Close();
                }

                StatusBox.Text = null;
                sw.WriteLine(Comp.Compile(script));
            }
            catch (CompileErrorCollection ce)
            {
                Regex CRRegex = new Regex(@"(.*?)\r\n");
                MatchCollection mc = CRRegex.Matches(script);

                for (int i = 0; i < ce.count; i++)
                    StatusBox.Text = ce[i].col.ToString() + " > " + mc[ce[i].col].Groups[1].Value + " --- " + ce[i].Message;
                
                crashed = true;
            }
            catch (NullReferenceException)
            {
                StatusBox.Text = StringMap.Dic[StringMap.Status.EmptyScript];
                crashed = true;
            }
            finally
            {
                StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Compiled];
            }
            #endregion

            #region Establish Status Message
            if (!crashed)
                StatusBox.Text = StringMap.Dic[StringMap.Status.Compiled];
            
            stw.Stop();
            StatusBox.Text += "\r\n" + stw.ElapsedMilliseconds.ToString() + "ms " + StringMap.Dic[StringMap.Status.Elapsed];

            sw.Flush();
            sw.Close();

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Compiled];
            #endregion
        }
        private void OpenNullScriptProject()
        {
            #region Opening Files
            OpenFileDialog open = new OpenFileDialog();
            open.Title = StringMap.Dic[StringMap.Status.OpenProject];
            open.DefaultExt = ".nsp";
            open.Filter = "NullScript Project (.nsp)|*.nsp";

            open.InitialDirectory = System.Environment.CurrentDirectory + @"..\..\";
            if (open.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            #endregion
            #region Establish TabControl
            FileTreeView.Controls.Clear();
            ScriptTab.Controls.Clear();
            
            nsp = new NullScriptProject(open.FileName);
            MainForm.ActiveForm.Text = "NullScripter - " + nsp.ProjectName;

            FileTreeView.Nodes.Add(nsp.ProjectName);
            FileTreeView.Nodes[0].Nodes.Add("Script");
            FileTreeView.Nodes[0].Nodes.Add("Sound");
            FileTreeView.Nodes[0].Nodes.Add("Image");

            foreach (string filepath in Directory.EnumerateFiles(Path.Combine(nsp.path, "Script")))
                FileTreeView.Nodes[0].Nodes[0].Nodes.Add(Path.GetFileName(filepath));

            foreach (string filepath in Directory.EnumerateFiles(Path.Combine(nsp.path, "Sound")))
                FileTreeView.Nodes[0].Nodes[1].Nodes.Add(Path.GetFileName(filepath));

            foreach (string filepath in Directory.EnumerateFiles(Path.Combine(nsp.path, "Image")))
                FileTreeView.Nodes[0].Nodes[2].Nodes.Add(Path.GetFileName(filepath));

            FileTreeView.ExpandAll();
            #endregion
        }
        private void SaveNullScriptProject()
        {
            #region Saving Files
            foreach (TabPage tp in ScriptTab.TabPages)
            {
                StreamWriter sw = new StreamWriter(Path.Combine(nsp.path, "Script", tp.Text));
                sw.Write(tp.Controls[0].Text);
                sw.Flush();
                sw.Close();

                StatusStripLabel.Text = tp.Text + StringMap.Dic[StringMap.Status.Saving];
            }

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Saved];
            #endregion
        }
        private void CreateNullScriptProject()
        {
            #region Create Directories & Files
            SaveFileDialog save = new SaveFileDialog();

            save.Title = StringMap.Dic[StringMap.Status.CreateProject];
            save.DefaultExt = ".nsp";
            save.Filter = "NullScripter Project (.nsp)|*.nsp";

            if (save.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string path = Path.GetDirectoryName(save.FileName);
            string filename = Path.GetFileName(save.FileName);

            Directory.CreateDirectory(Path.Combine(path, "Script"));
            Directory.CreateDirectory(Path.Combine(path, "Sound"));
            Directory.CreateDirectory(Path.Combine(path, "Image"));
            #endregion
            #region Establish .nsp File
            XmlWriterSettings xmlsetting = new XmlWriterSettings();
            xmlsetting.Indent = true;
            xmlsetting.IndentChars = "\t";
            xmlsetting.CloseOutput = true;
            using (XmlWriter xw = XmlWriter.Create(File.Create(Path.Combine(path, filename)), xmlsetting))
            {
                xw.WriteStartElement("NullScripter");

                xw.WriteStartElement("Project");
                xw.WriteAttributeString("Name", Path.GetFileNameWithoutExtension(filename));
                xw.Flush();

                xw.Close();
            }
            #endregion

            #region Open Created .nsp File
            nsp = new NullScriptProject(Path.Combine(path, filename));

            FileTreeView.Controls.Clear();
            ScriptTab.Controls.Clear();

            MainForm.ActiveForm.Text = "NullScripter - " + nsp.ProjectName;

            FileTreeView.Nodes.Add(nsp.ProjectName);
            FileTreeView.Nodes[0].Nodes.Add("Script");
            FileTreeView.Nodes[0].Nodes.Add("Sound");
            FileTreeView.Nodes[0].Nodes.Add("Image");

            foreach (string filepath in Directory.EnumerateFiles(Path.Combine(nsp.path, "Script")))
                FileTreeView.Nodes[0].Nodes[0].Nodes.Add(Path.GetFileName(filepath));

            foreach (string filepath in Directory.EnumerateFiles(Path.Combine(nsp.path, "Sound")))
                FileTreeView.Nodes[0].Nodes[1].Nodes.Add(Path.GetFileName(filepath));

            foreach (string filepath in Directory.EnumerateFiles(Path.Combine(nsp.path, "Image")))
                FileTreeView.Nodes[0].Nodes[2].Nodes.Add(Path.GetFileName(filepath));

            FileTreeView.ExpandAll();
            #endregion
        }
        private void Setting()
        {
            #region Setting
            if ((new Setting(nss)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                nss = new NullScripterSetting();
            
                foreach (TabPage tp in ScriptTab.TabPages)
                    tp.Controls[0].Font = nss.font;
            }
            #endregion
        }
        
        private void FileTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            #region Exception Check
            if (!e.Node.Text.Contains(".ns") || OpenedFile.Contains(e.Node.Text))
                return;
            #endregion

            #region Expand Control Tab
            OpenedFile.Add(e.Node.Text);

            TabPage tab = new TabPage(e.Node.Text);
            TextBox box = new TextBox();
            box.Multiline = true;
            box.Dock = DockStyle.Fill;
            box.Font = nss.font;
            box.ScrollBars = ScrollBars.Vertical;

            tab.Controls.Add(box);
            ScriptTab.Controls.Add(tab);
            ScriptTab.SelectedIndex = ScriptTab.TabCount - 1;

            StreamReader sr = new StreamReader(Path.Combine(nsp.path, e.Node.Parent.Text, e.Node.Text));
            ScriptTab.SelectedTab.Controls[0].Text = sr.ReadToEnd();
            sr.Close();
            #endregion
        }

        #region Key Processing
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    if (e.Modifiers == Keys.Control)
                    {
                        if (ScriptTab.TabCount == 0)
                            return;

                        TextBox tb = ScriptTab.SelectedTab.Controls[0] as TextBox;
                        tb.SelectAll();
                    }
                    break;

                case Keys.C:
                    if (e.Modifiers == Keys.Control)
                    {
                        if (ScriptTab.TabCount == 0)
                            return;

                        TextBox tb = ScriptTab.SelectedTab.Controls[0] as TextBox;
                        tb.Copy();
                    }
                    break;

                case Keys.N:
                    if (e.Modifiers == (Keys.Shift | Keys.Control))
                        CreateNullScriptProject();
                    break;

                case Keys.O:
                    if (e.Modifiers == Keys.Control)
                        OpenNullScriptProject();
                    break;

                case Keys.S:
                    if (e.Modifiers == Keys.Control)
                        SaveNullScriptProject();
                    break;

                case Keys.V:
                    if (e.Modifiers == Keys.Control)
                    {
                        if (ScriptTab.TabCount == 0)
                            return;

                        TextBox tb = ScriptTab.SelectedTab.Controls[0] as TextBox;
                        tb.Paste();
                    }
                    break;

                case Keys.W:
                    if (e.Modifiers == Keys.Control)
                    {
                        OpenedFile.RemoveAt(ScriptTab.SelectedIndex);
                        ScriptTab.TabPages.Remove(ScriptTab.SelectedTab);
                    }
                    break;

                case Keys.X:
                    if (e.Modifiers == Keys.Control)
                    {
                        if (ScriptTab.TabCount == 0)
                            return;

                        TextBox tb = ScriptTab.SelectedTab.Controls[0] as TextBox;
                        tb.Cut();
                    }
                    break;

                case Keys.F5:
                    if (e.Modifiers == Keys.Control)
                    {
                        SaveNullScriptProject();
                        Compile();
                    }
                    break;
            }
        }
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Fucking beep sound
            switch ((int)e.KeyChar)
            {
                case 1:  // Ctrl + A
                case 3:  // Ctrl + C
                case 14: // Ctrl + N
                case 15: // Ctrl + O
                case 19: // Ctrl + S
                case 22: // Ctrl + V
                case 24: // Ctrl + X
                    e.Handled = true;
                    return;
            }
        }
        #endregion

        #region Menu Proecdures
        private void Menu_File_New_Project_Click(object sender, EventArgs e)
        {
            CreateNullScriptProject();
        }

        private void Menu_File_New_File_Click(object sender, EventArgs e)
        {
#warning Not implemented
            throw new NotImplementedException();
        }

        private void Menu_File_Open_Project_Click(object sender, EventArgs e)
        {
            OpenNullScriptProject();
        }

        private void Menu_File_Open_File_Click(object sender, EventArgs e)
        {
#warning Not implemented
            throw new NotImplementedException();
        }

        private void Menu_Build_Compile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void Menu_Setting_Click(object sender, EventArgs e)
        {
            Setting();
        }

        private void TreeViewMenu_Add_Click(object sender, EventArgs e)
        {
#warning Not implemented
            throw new NotImplementedException();
        }
        private void ScriptTab_Close_Click(object sender, EventArgs e)
        {
            OpenedFile.RemoveAt(ScriptTab.SelectedIndex);
            ScriptTab.TabPages.Remove(ScriptTab.SelectedTab);
        }
        #endregion
    }
}
