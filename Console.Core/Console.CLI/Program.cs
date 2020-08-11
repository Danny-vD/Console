using System;
using System.Reflection;
using System.Threading;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.PropertySystem;
namespace Console.CLI
{
    class Program
    {
        private const string ExtensionPath = ".\\extensions\\";
        [Property("networking.tick")]
        private static float ConsoleTick = 0.2f;

        [Property("version.cli")]
        private static Version CLIVersion => Assembly.GetExecutingAssembly().GetName().Version;
        

        [Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
        private static void Exit()
        {
            Exit(0);
        }

        [Command("exit", "Closes the application with the specified Exit Code.", "Exit", "Quit", "quit")]
        private static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }


        static void Main(string[] args)
        {
            CLIConsoleManager cm = new CLIConsoleManager(ExtensionPath, AConsoleManager.ConsoleInitOptions.All);

            //Running the OnTick Loop
            Thread t = new Thread(Loop);
            t.Start();

            FlagTestClass tc = new FlagTestClass();

            while (true)
            {
                System.Console.Write("CLI>");
                string cmd = System.Console.ReadLine();
                //if (cmd != null && cmd.ToLower() == "exit") break;
                cm.EnterCommand(cmd);
            }
        }

        private static void Loop()
        {
            while (true)
            {
                AConsoleManager.Instance.InvokeOnTick();
                Thread.Sleep((int)(ConsoleTick * 1000));
            }
        }
    }
}
