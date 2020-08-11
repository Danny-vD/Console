using System.Linq;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Commands;
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

        [Command("test", "")]
        public void TestCommand(string data, [CommandFlag("to-lower")] bool lower, [CommandFlag]bool reverse)
        {
            string r = lower ? data.ToLower() : data;
            r = reverse ? new string(r.Reverse().ToArray(), 0, r.Length) : r;
            AConsoleManager.Instance.Log(r);
        }

    }
}