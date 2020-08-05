using Console.Core.Console;
using Console.Core.ObjectSelection;

namespace Console.CLI
{
    public class CLIConsoleManager : AConsoleManager
    {
        public override AObjectSelector ObjectSelector { get; }
        private DefaultCommandAdder DefaultCommands;
        private CommandHandler Handler;
        public CLIConsoleManager()
        {
            ObjectSelector = new CLIObjSelector();
            Handler = new CommandHandler();
            DefaultCommands = new DefaultCommandAdder();
            DefaultCommands.AddCommands();
        }

        public override void Clear()
        {
            System.Console.Clear();
        }

        public override void Log(object @object)
        {
            InvokeLogEvent(@object.ToString());
            System.Console.WriteLine(@object);
        }

        public override void LogCommand(string command)
        {
            //Log(command);
        }

        public override void LogError(object @object)
        {
            Log("Error: " + @object);
        }

        public override void LogPlainText(string text)
        {
            Log(text);
        }

        public override void LogWarning(object @object)
        {
            Log("Warning: " + @object);
        }

        protected override void SubmitCommand(string command)
        {
            string cmd = ExpanderManager.Expand(command);
            LogCommand(command);
            Handler.OnSubmitCommand(cmd);
        }
    }
}