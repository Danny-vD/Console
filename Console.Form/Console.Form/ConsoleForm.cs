using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;


namespace Console.Form
{
    public partial class ConsoleForm : System.Windows.Forms.Form
    {
        #region Settings

        [Property("version.form")]
        public static Version FormVersion => Assembly.GetExecutingAssembly().GetName().Version;
        [Property("form.out.maxsize")]
        public static int ConsoleOutputSize = 500;

        [Property("form.out.background")]
        public static Color ConsoleOutBackground
        {
            get => Instance.rtbOut.BackColor;
            set => Instance.rtbOut.BackColor = value;
        }
        [Property("form.out.textcolor")]
        public static Color ConsoleOutTextColor
        {
            get => Instance.rtbOut.ForeColor;
            set => Instance.rtbOut.ForeColor = value;
        }
        [Property("form.background")]
        public static Color ConsoleFormBackground
        {
            get => Instance.rtbOut.BackColor;
            set => Instance.rtbOut.BackColor = value;
        }
        [Property("form.textcolor")]
        public static Color ConsoleFormTextColor
        {
            get => Instance.rtbOut.ForeColor;
            set => Instance.rtbOut.ForeColor = value;
        }
        [Property("form.in.background")]
        public static Color ConsoleInBackground
        {
            get => Instance.tbConsoleIn.BackColor;
            set => Instance.tbConsoleIn.BackColor = value;
        }
        [Property("form.in.textcolor")]
        public static Color ConsoleInTextColor
        {
            get => Instance.tbConsoleIn.ForeColor;
            set => Instance.tbConsoleIn.ForeColor = value;
        }
        [Property("form.in.borderstyle")]
        public static BorderStyle ConsoleInBorderStyle
        {
            get => Instance.tbConsoleIn.BorderStyle;
            set => Instance.tbConsoleIn.BorderStyle = value;
        }
        [Property("form.out.borderstyle")]
        public static BorderStyle ConsoleOutBorderStyle
        {
            get => Instance.rtbOut.BorderStyle;
            set => Instance.rtbOut.BorderStyle = value;
        }
        [Property("form.submit.style")]
        public static FlatStyle ConsoleSubmitButtonBorderStyle
        {
            get => Instance.btnSubmit.FlatStyle;
            set => Instance.btnSubmit.FlatStyle = value;
        }

        [Property("form.log.backcolor")]
        public static Color LogLineOutputBackColor = Color.Black;
        [Property("form.log.textcolor")]
        public static Color LogLineOutputForeColor = Color.White;
        [Property("form.warning.backcolor")]
        public static Color LogWarningLineOutputBackColor = Color.Black;
        [Property("form.warning.textcolor")]
        public static Color LogWarningLineOutputForeColor = Color.Orange;
        [Property("form.error.backcolor")]
        public static Color LogErrorLineOutputBackColor = Color.Black;
        [Property("form.error.textcolor")]
        public static Color LogErrorLineOutputForeColor = Color.Red;
        [Property("form.command.backcolor")]
        public static Color LogCommandLineOutputBackColor = Color.Black;
        [Property("form.command.textcolor")]
        public static Color LogCommandLineOutputForeColor = Color.Green;

        #endregion

        public static ConsoleForm Instance { get; private set; }
        public static FormConsoleManager ConsoleManager => Instance.consoleManager;
        private readonly FormConsoleManager consoleManager;

        [Command("exit", "Closes the Console.")]
        private static void Exit() => Exit(0);
        [Command("exit", "Closes the Console with a specfic exit code.")]
        private static void Exit(int exitCode) => Environment.Exit(exitCode);

        private class ConsoleLine
        {
            public readonly string Content;
            public readonly Color TextColor;
            public readonly Color BackColor;

            public ConsoleLine(string content, Color textColor, Color backColor)
            {
                Content = content;
                TextColor = textColor;
                BackColor = backColor;
            }
            public void WriteToOut(RichTextBox rtb)
            {
                rtb.AppendLine(Content, TextColor, BackColor);
            }
        }

        private readonly List<ConsoleLine> OutLines = new List<ConsoleLine>();

        public ConsoleForm()
        {
            if (Instance != null) throw new Exception("There is already a console running.");


            Instance = this;
            InitializeComponent();
            Closed += (sender, args) => ReleaseInstance();
            tbConsoleIn.KeyDown += TbConsoleIn_KeyDown;

            if (!Directory.Exists(".\\extensions\\")) Directory.CreateDirectory(".\\extensions\\");
            consoleManager = new FormConsoleManager(".\\extensions\\", AConsoleManager.ConsoleInitOptions.All);
            tbConsoleIn.Select();
        }

        private void TbConsoleIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                EnterCommand();
        }

        private void ReleaseInstance()
        {
            Instance = null;
        }

        public void Clear()
        {
            OutLines.Clear();
            rtbOut.Clear();
        }

        public void LogPlainText(string obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), rtbOut.ForeColor, rtbOut.BackColor);
            Log(line);
        }

        public void Log(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogLineOutputForeColor, LogLineOutputBackColor);
            Log(line);
        }

        public void LogWarning(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogWarningLineOutputForeColor, LogWarningLineOutputBackColor);
            Log(line);
        }

        public void LogError(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogErrorLineOutputForeColor, LogErrorLineOutputBackColor);
            Log(line);
        }

        public void LogCommand(object obj)
        {
            ConsoleLine line = new ConsoleLine(obj.ToString(), LogCommandLineOutputForeColor, LogCommandLineOutputBackColor);
            Log(line);
        }

        private void Log(ConsoleLine line)
        {
            OutLines.Add(line);
            line.WriteToOut(rtbOut);
            EnforceOutputLength();
            rtbOut.ScrollToCaret();
        }

        private void RebuildConsoleOut()
        {
            rtbOut.Clear();
            foreach (ConsoleLine line in OutLines)
            {
                line.WriteToOut(rtbOut);
            }
        }

        private void EnforceOutputLength()
        {
            if (OutLines.Count > ConsoleOutputSize)
            {
                OutLines.RemoveRange(0, OutLines.Count - ConsoleOutputSize);
                RebuildConsoleOut();
            }
        }

        private void EnterCommand()
        {
            if (!string.IsNullOrWhiteSpace(tbConsoleIn.Text))
            {
                consoleManager.EnterCommand(tbConsoleIn.Text);
                tbConsoleIn.Text = "";
                tbConsoleIn.Focus();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            EnterCommand();
        }
    }
}
