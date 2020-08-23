using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;
using Console.Form.Internal;


namespace Console.Form
{

    /// <summary>
    /// The Windows Form that represents the Console Window
    /// </summary>
    public partial class ConsoleForm : System.Windows.Forms.Form
    {
        #region Settings

        /// <summary>
        /// Console.Form Version
        /// </summary>
        [Property("version.form")]
        public static Version FormVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// The Maximum Log Size in the Console Output Window
        /// </summary>
        [Property("form.out.maxsize")]
        public static int ConsoleOutputSize = 500;

        /// <summary>
        /// The Console Output Default Back Color
        /// </summary>
        [Property("form.out.background")]
        public static Color ConsoleOutBackground
        {
            get => Instance.rtbOut.BackColor;
            set => Instance.rtbOut.BackColor = value;
        }
        /// <summary>
        /// The Console Output Default Text Color
        /// </summary>
        [Property("form.out.textcolor")]
        public static Color ConsoleOutTextColor
        {
            get => Instance.rtbOut.ForeColor;
            set => Instance.rtbOut.ForeColor = value;
        }
        /// <summary>
        /// The Console Default Back Color
        /// </summary>
        [Property("form.background")]
        public static Color ConsoleFormBackground
        {
            get => Instance.BackColor;
            set => Instance.BackColor = value;
        }
        /// <summary>
        /// The Console Output Default Text Color
        /// </summary>
        [Property("form.textcolor")]
        public static Color ConsoleFormTextColor
        {
            get => Instance.ForeColor;
            set => Instance.ForeColor = value;
        }
        /// <summary>
        /// The Console Input Default Back Color
        /// </summary>
        [Property("form.in.background")]
        public static Color ConsoleInBackground
        {
            get => Instance.tbConsoleIn.BackColor;
            set => Instance.tbConsoleIn.BackColor = value;
        }

        /// <summary>
        /// The Console Input Default Text Color
        /// </summary>
        [Property("form.in.textcolor")]
        public static Color ConsoleInTextColor
        {
            get => Instance.tbConsoleIn.ForeColor;
            set => Instance.tbConsoleIn.ForeColor = value;
        }

        /// <summary>
        /// The Console Input BorderStyle
        /// </summary>
        [Property("form.in.borderstyle")]
        public static BorderStyle ConsoleInBorderStyle
        {
            get => Instance.tbConsoleIn.BorderStyle;
            set => Instance.tbConsoleIn.BorderStyle = value;
        }
        /// <summary>
        /// The Console Output BorderStyle
        /// </summary>
        [Property("form.out.borderstyle")]
        public static BorderStyle ConsoleOutBorderStyle
        {
            get => Instance.rtbOut.BorderStyle;
            set => Instance.rtbOut.BorderStyle = value;
        }

        /// <summary>
        /// The Console Submit Button FlatStyle
        /// </summary>
        [Property("form.submit.style")]
        public static FlatStyle ConsoleSubmitButtonBorderStyle
        {
            get => Instance.btnSubmit.FlatStyle;
            set => Instance.btnSubmit.FlatStyle = value;
        }

        /// <summary>
        /// The Console Log Back Color
        /// </summary>
        [Property("form.log.backcolor")]
        public static Color LogLineOutputBackColor = Color.Black;
        /// <summary>
        /// The Console Log Text Color
        /// </summary>
        [Property("form.log.textcolor")]
        public static Color LogLineOutputForeColor = Color.White;
        /// <summary>
        /// The Console Log Warning Back Color
        /// </summary>
        [Property("form.warning.backcolor")]
        public static Color LogWarningLineOutputBackColor = Color.Black;
        /// <summary>
        /// The Console Log Warning Text Color
        /// </summary>
        [Property("form.warning.textcolor")]
        public static Color LogWarningLineOutputForeColor = Color.Orange;
        /// <summary>
        /// The Console Log Error Back Color
        /// </summary>
        [Property("form.error.backcolor")]
        public static Color LogErrorLineOutputBackColor = Color.Black;
        /// <summary>
        /// The Console Log Error Text Color
        /// </summary>
        [Property("form.error.textcolor")]
        public static Color LogErrorLineOutputForeColor = Color.Red;
        /// <summary>
        /// The Console Log Command Back Color
        /// </summary>
        [Property("form.command.backcolor")]
        public static Color LogCommandLineOutputBackColor = Color.Black;
        /// <summary>
        /// The Console Log Command Text Color
        /// </summary>
        [Property("form.command.textcolor")]
        public static Color LogCommandLineOutputForeColor = Color.Green;

        #endregion

        /// <summary>
        /// The Instance of the ConsoleForm
        /// </summary>
        public static ConsoleForm Instance { get; private set; }
        /// <summary>
        /// The Console Manager Instance Wrapper
        /// </summary>
        public static FormConsoleManager ConsoleManager => Instance.consoleManager;
        /// <summary>
        /// The Console Manager Instance
        /// </summary>
        private readonly FormConsoleManager consoleManager;

        /// <summary>
        /// Exit Command
        /// </summary>
        [Command("exit", "Closes the Console.")]
        private static void Exit() => Exit(0);

        /// <summary>
        /// Exit Command
        /// </summary>
        /// <param name="exitCode">The Exit Code reported back to the Operating System</param>
        [Command("exit", "Closes the Console with a specfic exit code.")]
        private static void Exit(int exitCode) => Environment.Exit(exitCode);


        /// <summary>
        /// The History of Logs
        /// </summary>
        private readonly List<ConsoleLine> OutLines = new List<ConsoleLine>();
        /// <summary>
        /// Public Constructor
        /// </summary>
        public ConsoleForm()
        {
            if (Instance != null) throw new Exception("There is already a console running.");


            Instance = this;
            InitializeComponent();
            Closed += (sender, args) => ReleaseInstance();
            tbConsoleIn.KeyDown += (sender, args) => TbConsoleIn_KeyDown(args);

            if (!Directory.Exists(".\\extensions\\")) Directory.CreateDirectory(".\\extensions\\");
            consoleManager = new FormConsoleManager(".\\extensions\\", ConsoleInitOptions.All);
            tbConsoleIn.Select();
        }

        /// <summary>
        /// Gets Invoked when a Key gets pressed while the Input Textbox has focus.
        /// <param name="e">The KeyEventArgs containing the Key Stroke</param>
        /// </summary>
        private void TbConsoleIn_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                EnterCommand();
        }

        /// <summary>
        /// Releases the Instance Variable.
        /// </summary>
        private void ReleaseInstance()
        {
            Instance = null;
        }

        /// <summary>
        /// Clears the Console Output
        /// </summary>
        public void Clear()
        {
            OutLines.Clear();
            rtbOut.Clear();
        }

        /// <summary>
        /// Writes a Log in Plain Text.
        /// </summary>
        /// <param name="obj">Object to Log</param>
        public void LogPlainText(string obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), rtbOut.ForeColor, rtbOut.BackColor);
            Log(line);
        }

        /// <summary>
        /// Writes a Log
        /// </summary>
        /// <param name="obj">Object to Log</param>
        public void Log(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogLineOutputForeColor, LogLineOutputBackColor);
            Log(line);
        }

        /// <summary>
        /// Writes a Log Warning
        /// </summary>
        /// <param name="obj">Object to Log</param>
        public void LogWarning(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogWarningLineOutputForeColor, LogWarningLineOutputBackColor);
            Log(line);
        }

        /// <summary>
        /// Writes a Log Error
        /// </summary>
        /// <param name="obj">Object to Log</param>
        public void LogError(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogErrorLineOutputForeColor, LogErrorLineOutputBackColor);
            Log(line);
        }

        /// <summary>
        /// Writes a Log Command
        /// </summary>
        /// <param name="obj">Object to Log</param>
        public void LogCommand(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogCommandLineOutputForeColor, LogCommandLineOutputBackColor);
            Log(line);
        }


        /// <summary>
        /// Writes the Log to the Console
        /// </summary>
        /// <param name="line">Line to be Logged</param>
        private void Log(ConsoleLine line)
        {
            OutLines.Add(line);
            line.WriteToOut(rtbOut);
            EnforceOutputLength();
            rtbOut.ScrollToCaret();
        }

        /// <summary>
        /// Rebuilds the Console Output from scratch
        /// </summary>
        private void RebuildConsoleOut()
        {
            rtbOut.Clear();
            foreach (ConsoleLine line in OutLines)
            {
                line.WriteToOut(rtbOut);
            }
        }

        /// <summary>
        /// Enforces the Maximum Console Output Length
        /// </summary>
        private void EnforceOutputLength()
        {
            if (OutLines.Count > ConsoleOutputSize)
            {
                OutLines.RemoveRange(0, OutLines.Count - ConsoleOutputSize);
                RebuildConsoleOut();
            }
        }

        /// <summary>
        /// Enters the Command that has been written to the Input Field.
        /// </summary>
        private void EnterCommand()
        {
            if (!string.IsNullOrWhiteSpace(tbConsoleIn.Text))
            {
                consoleManager.EnterCommand(tbConsoleIn.Text);
                tbConsoleIn.Text = "";
                tbConsoleIn.Focus();
            }
        }

        /// <summary>
        /// Gets invoked when the Input Submit Button Gets Pressed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event args of the Click Event</param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            EnterCommand();
        }
    }
}
