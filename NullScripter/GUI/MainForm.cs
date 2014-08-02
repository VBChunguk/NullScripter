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
        private static Compiler Comp;
        private static NullScriptProject nsp;
        private static List<string> OpenedFIle;

        public MainForm()
        {
            InitializeComponent();

            OpenedFIle = new List<string>();
            Comp = new Compiler();

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Ready];
        }

        private void Compile()
        {
            if (nsp == null)
                return;

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Compiling];
            bool crashed = false;

            string script = null;
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
                StatusBox.Text = "Script is null";
                crashed = true;
            }

            stw.Stop();

            if (!crashed)
                StatusBox.Text = StringMap.Dic[StringMap.Status.Compiled];

            StatusBox.Text += "\r\n" + stw.ElapsedMilliseconds.ToString() + "ms Elapsed.";

            sw.Flush();
            sw.Close();

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Compiled];
        }
        private void OpenNullScriptProject()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Open Project";
            open.DefaultExt = ".nsp";
            open.Filter = "NullScript Project (.nsp)|*.nsp";

            open.InitialDirectory = System.Environment.CurrentDirectory + @"..\..\";
            if (open.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

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
        }
        private void SaveNullScriptProject()
        {
            foreach (TabPage tp in ScriptTab.TabPages)
            {
                StreamWriter sw = new StreamWriter(Path.Combine(nsp.path, "Script", tp.Text));
                sw.Write(tp.Controls[0].Text);
                sw.Flush();
                sw.Close();

                StatusStripLabel.Text = tp.Text + StringMap.Dic[StringMap.Status.Saving];
            }

            StatusStripLabel.Text = StringMap.Dic[StringMap.Status.Saved];
        }
        private void CreateNullScriptProject()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Title = "Save";
            save.DefaultExt = ".nsp";
            save.Filter = "NullScript Project (.nsp)|*.nsp";

            if (save.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string path = Path.GetDirectoryName(save.FileName);
            string filename = Path.GetFileName(save.FileName);

            Directory.CreateDirectory(Path.Combine(path, "Script"));
            Directory.CreateDirectory(Path.Combine(path, "Sound"));
            Directory.CreateDirectory(Path.Combine(path, "Image"));

            XmlWriterSettings xmlsetting = new XmlWriterSettings();
            xmlsetting.Indent = true;
            xmlsetting.IndentChars = "\t";
            xmlsetting.CloseOutput = true;
            XmlWriter xw = XmlWriter.Create(File.Create(Path.Combine(path, filename)), xmlsetting);

            xw.WriteStartElement("NullScripter");

            xw.WriteStartElement("Project");
            xw.WriteAttributeString("Name", Path.GetFileNameWithoutExtension(filename));
            xw.Flush();

            xw.Close();

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
        }
        private void FileTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.Contains(".ns") && !OpenedFIle.Contains(e.Node.Text))
            {
                OpenedFIle.Add(e.Node.Text);

                TabPage tab = new TabPage(e.Node.Text);
                TextBox box = new TextBox();
                box.Multiline = true;
                box.Dock = DockStyle.Fill;
                box.Font = new Font("malgun gothic", 10);

                tab.Controls.Add(box);
                ScriptTab.Controls.Add(tab);
                ScriptTab.SelectedIndex = ScriptTab.TabCount - 1;

                StreamReader sr = new StreamReader(Path.Combine(nsp.path, e.Node.Parent.Text, e.Node.Text));
                ScriptTab.TabPages[ScriptTab.SelectedIndex].Controls[0].Text = sr.ReadToEnd();
                sr.Close();
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.O:
                    if (e.Modifiers == Keys.Control)
                        OpenNullScriptProject();
                    break;

                case Keys.S:
                    if (e.Modifiers == Keys.Control)
                        SaveNullScriptProject();
                    break;

                case Keys.W:
                    if (e.Modifiers == Keys.Control)
                    {
                        OpenedFIle.RemoveAt(ScriptTab.SelectedIndex);
                        ScriptTab.TabPages.Remove(ScriptTab.SelectedTab);
                    }
                    break;

                case Keys.N:
                    if (e.Modifiers == (Keys.Shift | Keys.Control))
                        CreateNullScriptProject();
                    break;

                case Keys.F5:
                    if (e.Modifiers == Keys.Control)
                        Compile();
                    break;
            }
        }
    
        private void Menu_Build_Compile(object sender, EventArgs e)
        {
            Compile();
        }
        private void Menu_File_Open_Project(object sender, EventArgs e)
        {
            OpenNullScriptProject();
        }
        private void Menu_File_New_Project(object sender, EventArgs e)
        {
            CreateNullScriptProject();
        }
    }
}
