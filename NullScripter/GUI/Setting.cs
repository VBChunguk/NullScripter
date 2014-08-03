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
        #region Declarement
        public Font font;
        #endregion

        public Setting(NullScripterSetting nss)
        {
            #region Initializing
            InitializeComponent();

            font = nss.font;

            Initialize();
            #endregion
        }
        public void Initialize()
        {
            #region Initializing
            FontName.Text = font.Name;
            FontSize.Text = font.Size.ToString();
            #endregion
        }

        private void FontSetting_Click(object sender, EventArgs e)
        {
            #region Font Setting
            FontDialog fd = new FontDialog();
            fd.Font = font;
            fd.ShowDialog();
            font = fd.Font;

            Initialize();
            #endregion
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            #region Confirm
            NullScripterSetting.CreateSetting(font);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            #endregion
        }

        private void Cancle_Click(object sender, EventArgs e)
        {
            #region Cancle
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            #endregion
        }
    }
}
