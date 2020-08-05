using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml.Linq;
using Console.Core;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Attributes.PropertySystem.Helper;
using Console.Core.Commands.ConverterSystem;
using Console.Core.Console;
using Console.EnvironmentVariables;
using Console.IO;
using Console.Networking;
using Console.PropEnvCompat;
using Console.ScriptSystem;

namespace Console.CLI
{
    class Program
    {
        private const string ExtensionPath = ".\\extensions\\";
        [ConsoleProperty("console.networking.tick")]
        private static float ConsoleTick = 0.2f;
        

        static void Main(string[] args)
        {
            CLIConsoleManager cm = new CLIConsoleManager();
            
            ConsolePropertyAttributeUtils.InitializePropertySystem();
            ConsolePropertyAttributeUtils.AddProperties<ConsoleCoreConfig>();
            ConsolePropertyAttributeUtils.AddProperties<Program>();
            CustomConvertManager.AddConverter(new ArrayConverter());
            CommandAttributeUtils.AddCommands<Program>();


            //AExtensionInitializer.LoadExtensions(ExtensionPath);
            new EnvInitializer().Initialize();
            new NetworkedInitializer().Initialize();
            new PropCompatInitializer().Initialize();
            new IOInitializer().Initialize();
            new ScriptSystemInitializer().Initialize();
            new IOCompatInitializer().Initialize();

            Thread t = new Thread(Loop);


            t.Start();

            //For debugging adding as reference.
            //new NetworkedInitializer().Initialize();
            //new PropCompatInitializer().Initialize();
            //new EnvInitializer().Initialize();

            while (true)
            {
                System.Console.Write("CLI>");
                string cmd = System.Console.ReadLine();
                if (cmd != null && cmd.ToLower() == "exit") break;
                cm.EnterCommand(cmd);
            }
            t.Abort();
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
