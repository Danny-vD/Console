using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml.Linq;
using Console.Core;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Attributes.PropertySystem.Helper;
using Console.Core.Console;

namespace Console.CLI
{
    class Program
    {
        [ConsoleProperty("console.networking.tick")]
        private static float ConsoleTick = 0.2f;

        static void Main(string[] args)
        {
            CLIConsoleManager cm = new CLIConsoleManager();
            ConsolePropertyAttributeUtils.InitializePropertySystem();
            ConsolePropertyAttributeUtils.AddProperties<ConsoleCoreConfig>();
            ConsolePropertyAttributeUtils.AddProperties<Program>();

            Thread t = new Thread(Loop);

            Directory.CreateDirectory(".\\extensions\\");
            string[] exts = Directory.GetFiles(".\\extensions\\", "*.dll", SearchOption.AllDirectories);
            foreach (string ext in exts)
            {
                ConsoleCoreConfig.LoadExtension(ext);
            }
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
