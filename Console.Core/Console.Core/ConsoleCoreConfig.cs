using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Console;

namespace Console.Core
{
    public class ConsoleCoreConfig
    {
        private ConsoleCoreConfig()
        {
        }

        [ConsoleProperty("console.output.writecommand")]
        public static bool WriteCommand = true;

        [ConsoleProperty("console.input.prefix")]
        public static string ConsolePrefix = "";

        [ConsoleProperty("console.input.stringchar")]
        public static char StringChar = '"';


        public static void LoadExtensions(string folder)
        {
            LoadExtensions(Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories));
        }

        public static void LoadExtensions(string[] paths)
        {
            LoadExtensions(paths.Select(LoadExtension).Where(x => x != null).ToArray());
        }

        public static void LoadExtensions(AExtensionInitializer[] exts)
        {
            Dictionary<LoadOrder, List<AExtensionInitializer>> extensions =
                new Dictionary<LoadOrder, List<AExtensionInitializer>>
                {
                    {LoadOrder.Default, new List<AExtensionInitializer>()},
                    {LoadOrder.First, new List<AExtensionInitializer>()},
                    {LoadOrder.After, new List<AExtensionInitializer>()},
                };

            foreach (AExtensionInitializer e in exts)
            {
                if (e != null)
                {
                    extensions[e.Order].Add(e);
                }
            }
            LoadExtensions(extensions);
        }

        public static void LoadExtensions(Dictionary<LoadOrder, List<AExtensionInitializer>> extensions)
        {
            string s = "Loading Extensions...";
            int i = LoadExtensions(extensions[LoadOrder.First]);
            s += "\nLoadOrder.First: " + extensions[LoadOrder.First].Count;
            i += LoadExtensions(extensions[LoadOrder.Default]);
            s += "\nLoadOrder.Default: " + extensions[LoadOrder.Default].Count;
            i += LoadExtensions(extensions[LoadOrder.After]);
            s += "\nLoadOrder.After: " + extensions[LoadOrder.After].Count;
            s += "\nTotal Loaded Extensions: " + i+"\n";
            AConsoleManager.Instance.Log(s);
        }

        private static int LoadExtensions(List<AExtensionInitializer> extensions)
        {
            foreach (AExtensionInitializer aExtensionInitializer in extensions)
            {
                aExtensionInitializer.Initialize();
            }
            return extensions.Count;
        }

        private static AExtensionInitializer LoadExtension(string path)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(path);
                AExtensionInitializer i = GetInitializer(asm);
                AConsoleManager.Instance.Log("Loaded: " + Path.GetFileNameWithoutExtension(path));
                return i;
            }
            catch (Exception e)
            {
                AConsoleManager.Instance.LogWarning("Can not load Extension: " + path);
                AConsoleManager.Instance.LogWarning(e);
                return null;
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