using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NullScripter.Script;

namespace NullScripter.GUI
{
    public partial class Setting : Form
    {
        public NullScripterSetting nss;

        public Setting(NullScripterSetting nss)
        {
            InitializeComponent();

            this.nss = nss;
            Initialize();
        }
        public void Initialize()
        {
            FontName.Text = nss.font.Name;
            FontSize.Text = nss.font.Size.ToString();
        }

        private void FontSetting_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = nss.font;
            fd.ShowDialog();

            nss.ChangeSetting(fd.Font);
            Initialize();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
