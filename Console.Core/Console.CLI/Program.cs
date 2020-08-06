using System;
using System.Collections.Generic;
using System.Threading;
using Console.ArrayConverter;
using Console.Core;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Console;
using Console.DefaultConverters;
using Console.EnvironmentVariables;
using Console.IO;
using Console.Networking;
using Console.PersistentProperties;
using Console.PropEnvCompat;
using Console.PropIOCompat;
using Console.ScriptIOCompat;
using Console.ScriptSystem;

namespace Console.CLI
{
    class Program
    {
        private const string ExtensionPath = ".\\extensions\\";
        [ConsoleProperty("console.networking.tick")]
        private static float ConsoleTick = 0.2f;

        [Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
        private static void Exit()
        {
            Environment.Exit(0);
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
                new DefaultConverterInitializer()
            };
            CLIConsoleManager cm = new CLIConsoleManager(ii.ToArray());

            

            //Testing
            cm.ObjectSelector.AddToSelection("LOL1");
            cm.ObjectSelector.AddToSelection("LOL2");
            cm.ObjectSelector.AddToSelection("LOL3");


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
