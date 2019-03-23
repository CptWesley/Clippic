using System;
using System.Windows.Forms;

namespace Clippic
{
    /// <summary>
    /// Entry point of the program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool sound = true;
            foreach (string arg in args)
            {
                if (arg == "-nosound")
                {
                    sound = false;
                }
            }

            Application.Run(new SelectionForm(sound));
        }
    }
}
