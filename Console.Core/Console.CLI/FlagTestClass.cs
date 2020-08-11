using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.Utils;

namespace Console.CLI
{
    public class FlagTestClass
    {
        public FlagTestClass()
        {
            //PropertyAttributeUtils.AddProperties(this);
            CommandAttributeUtils.AddCommands(this);
        }

        [Command("flag-tests", "Manipulates data depending on the flags set.")]
        public void TestCommand(string data, [CommandFlag("to-lower")] bool lower, [CommandFlag]bool reverse)
        {
            AConsoleManager.Instance.Log("Data: " + data);
            AConsoleManager.Instance.Log("Lower: " + lower);
            AConsoleManager.Instance.Log("Reverse: " + reverse);
        }
        [Command("flag-tests", "Manipulates data depending on the flags set.")]
        public void TestCommand(string data, [CommandFlag] bool reverse)
        {
            AConsoleManager.Instance.Log("Data: " + data);
            AConsoleManager.Instance.Log("Reverse: " + reverse);
        }

    }
}