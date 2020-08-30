using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

/// <summary>
/// Commandline Interface Test Application
/// </summary>
namespace Console.CLI
{
    /// <summary>
    /// CLI Entry Class
    /// </summary>
    internal class Program
    {
        [Property("logs.cli.mute")]
        private bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }
        internal static readonly ALogger Logger = new PrefixLogger(Assembly.GetExecutingAssembly().GetName().Name);

        /// <summary>
        /// Path of the Extensions
        /// </summary>
        private const string ExtensionPath = ".\\extensions\\";

        /// <summary>
        /// Defines in what intervall the console tick event gets invoked
        /// </summary>
        [Property("networking.tick")] private static readonly float ConsoleTick = 0.2f;

        /// <summary>
        /// The Commandline Interface Version
        /// </summary>
        [Property("version.cli")]
        private static Version CLIVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Closes the Commandline with Error Code 0
        /// </summary>
        [Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
        private static void Exit()
        {
            Exit(0);
        }

        /// <summary>
        /// Closes the Commandline with a specified ErrorCode
        /// </summary>
        /// <param name="exitCode">Exit Code</param>
        [Command("exit", "Closes the application with the specified Exit Code.", "Exit", "Quit", "quit")]
        private static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Main Entry Point
        /// </summary>
        /// <param name="args">CLI parameter</param>
        private static void Main(string[] args)
        {

            string initDir = Directory.GetCurrentDirectory();
            //List<AExtensionInitializer> ii = new List<AExtensionInitializer>
            //{
            //    new ClassQueryInitializer(),
            //    new ScriptSystemInitializer(),
            //    new EnvInitializer(),
            //    new NetworkedInitializer()
            //};
            //CLIConsoleManager cm = new CLIConsoleManager(ii.ToArray(), AConsoleManager.ConsoleInitOptions.All);
            CLIConsoleManager cm = new CLIConsoleManager(ExtensionPath);
            //new EnvInitializer().Initialize();
            //new DefaultConverterInitializer().Initialize();

            //Running the OnTick Loop
            Thread t = new Thread(Loop);
            t.Start();


            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    cm.EnterCommand($"run " + args[i]);
                }
            }

            while (true)
            {
                System.Console.Write($"{Directory.GetCurrentDirectory().Replace(initDir, "").Replace('\\', '/')}/>");
                string cmd = System.Console.ReadLine();
                //if (cmd != null && cmd.ToLower() == "exit") break;
                cm.EnterCommand(cmd);
            }
        }

        /// <summary>
        /// Thread that invokes the OnTickEvent
        /// </summary>
        private static void Loop()
        {
            while (true)
            {
                AConsoleManager.Instance.InvokeOnTick();
                Thread.Sleep((int) (ConsoleTick * 1000));
            }
        }
    }
}