using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using NullScripter;

namespace NullScripter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StringMap.Init(StringMap.Language.kr);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Debugger.Initialize();
            Debugger.WriteLine("NullScripter is On.");
            Debugger.CarriageReturn();
#if DEBUG
            Debugger.Show();
#endif

            Application.Run(new NullScripter.GUI.MainForm());

            Debugger.CarriageReturn();
            Debugger.WriteLine("NullScripter is Off.");
        }
    }
}
