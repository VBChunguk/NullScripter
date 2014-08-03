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
            Debug.Init();

            StringMap.Init(StringMap.Language.kr);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Debug.WriteLine("NullScripter is On.");
            Debug.CR();

            Application.Run(new NullScripter.GUI.MainForm());

            Debug.CR();
            Debug.WriteLine("NullScripter is Off.");
        }
    }
}
