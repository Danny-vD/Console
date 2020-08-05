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
using Console.Networking;
using Console.PropEnvCompat;

namespace Console.CLI
{
    class Program
    {
        private const string ExtensionPath = ".\\extensions\\";
        [ConsoleProperty("console.networking.tick")]
        private static float ConsoleTick = 0.2f;

        [ConsoleProperty("o")]
        private static object o ;
        [ConsoleProperty("oa")]
        private static object[] oa;
        [ConsoleProperty("ol")]
        private static List<object> ol;

        [Command("second", "")]
        private static void TestCommandSecond(string test, [SelectionProperty] object value)
        {
            AConsoleManager.Instance.Log(value);
        }

        [Command("first","")]
        private static void TestCommandFirst([SelectionProperty] object[] value, string test)
        {
            for (int i = 0; i < value.Length; i++)
            {
                AConsoleManager.Instance.Log(value[i]);
            }
        }

        private class TestClass
        {
            private int Index;

            public TestClass(int index)
            {
                Index = index;
            }
            public override string ToString()
            {
                return "Test Class: " + Index;
            }
        }

        static void Main(string[] args)
        {
            CLIConsoleManager cm = new CLIConsoleManager();

            if (cm.ObjectSelector is CLIObjSelector s)
            {
                for (int i = 0; i < 1000; i++)
                {
                    s.SelectableObjects.Add(i.ToString(), new TestClass(i));
                }
            }
            
            ConsolePropertyAttributeUtils.InitializePropertySystem();
            ConsolePropertyAttributeUtils.AddProperties<ConsoleCoreConfig>();
            ConsolePropertyAttributeUtils.AddProperties<Program>();
            CustomConvertManager.AddConverter(new ArrayConverter());
            CommandAttributeUtils.AddCommands<Program>();


            //AExtensionInitializer.LoadExtensions(ExtensionPath);
            new EnvInitializer().Initialize();
            new NetworkedInitializer().Initialize();
            new PropCompatInitializer().Initialize();


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
