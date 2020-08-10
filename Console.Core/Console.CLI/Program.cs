using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Console.ArrayConverter;
using Console.ClassQueries;
using Console.Core;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.PropertySystem;
using Console.DefaultConverters;
using Console.EnvironmentVariables;
using Console.IO;
using Console.Networking;
using Console.PersistentProperties;
using Console.PropEnvCompat;
using Console.PropIOCompat;
using Console.ScriptIOCompat;
using Console.ScriptSystem;
using Console.UtilExtension;

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
            List<AExtensionInitializer> ii = new List<AExtensionInitializer>
            {
                new ScriptIOCompatInitializer(),
                new EnvInitializer(),
                new NetworkedInitializer(),
                new PropCompatInitializer(),
                new IOInitializer(),
                new ScriptSystemInitializer(),
                new IOCompatInitializer(),
                new PersistentPropertiesInitializer(),
                new ArrayConverterInitializer(),
                new DefaultConverterInitializer(),
                new ClassQueryInitializer(),
                new UtilExtensionInitializer()
            };
            CLIConsoleManager cm = new CLIConsoleManager(ii.ToArray(), AConsoleManager.ConsoleInitOptions.All);

            //Running the OnTick Loop
            Thread t = new Thread(Loop);
            t.Start();


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
