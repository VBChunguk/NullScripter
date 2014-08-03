using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NullScripter
{
    public partial class DebugForm : Form
    {
        private static System.Diagnostics.Stopwatch sw;

        public DebugForm()
        {
            #region Initialize
            InitializeComponent();
#if DEBUG
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            #endregion
        }

        #region Strings
        public void WriteLine(string str)
        {
            #region WriteLine
#if DEBUG
            DebugTextBox.Text += sw.ElapsedMilliseconds + " : " + str + "\r\n";

            DebugTextBox.SelectionStart = DebugTextBox.TextLength;
            DebugTextBox.ScrollToCaret();
#endif
            #endregion
        }
        public void CarriageReturn()
        {
            #region Carriage Return
#if DEBUG
            DebugTextBox.Text += "\r\n"; 
            
            DebugTextBox.SelectionStart = DebugTextBox.TextLength;
            DebugTextBox.ScrollToCaret();
#endif
            #endregion
        }
        #endregion
    }

    class Debugger
    {
        private static DebugForm debugform = null;

        public static void Initialize()
        {
            #region Initialize
            debugform = new DebugForm();
            #endregion
        }
        public static void Show()
        {
            #region Show
            debugform.Show();
            Debugger.WriteLine("Debugger is On");
            #endregion
        }

        #region Strings
        public static void WriteLine(string str)
        {
            debugform.WriteLine(str);
        }
        public static void CarriageReturn()
        {
            debugform.CarriageReturn();
        }
        #endregion
    }
}
