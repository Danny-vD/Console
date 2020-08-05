using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Console;

namespace Console.Core
{
    public class ConsoleCoreConfig
    {
        private ConsoleCoreConfig() { }

        [ConsoleProperty("console.input.prefix")]
        public static string ConsolePrefix = "";

        [ConsoleProperty("console.input.stringchar")]
        public static char StringChar = '"';


        public static void LoadExtension(string path)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(path);
                AExtensionInitializer i = GetInitializer(asm);
                i.Initialize();
                AConsoleManager.Instance.Log("Loaded: " + Path.GetFileNameWithoutExtension(path));
            }
            catch (Exception e)
            {
                AConsoleManager.Instance.LogWarning("Can not load Extension: " + path);
                AConsoleManager.Instance.LogWarning(e);
            }
        }

        private static AExtensionInitializer GetInitializer(Assembly asm)
        {
            Type t = asm.GetTypes().First(x =>
                typeof(AExtensionInitializer).IsAssignableFrom(x) && x != typeof(AExtensionInitializer));
            return (AExtensionInitializer)Activator.CreateInstance(t);
        }

    }
}
