using System;
using System.Windows.Forms;

/// <summary>
/// Root Namespace of the Console.Form Project
/// </summary>
namespace Console.Form
{
    /// <summary>
    /// The Entry Point of the Form Application
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConsoleForm());
        }
    }
}
