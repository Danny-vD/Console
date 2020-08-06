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
            LoadExtensions(paths.Select(LoadExtension).Where(x=>x!=null).ToArray());
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
            extensions[LoadOrder.First].ForEach(x => x.Initialize());
            extensions[LoadOrder.Default].ForEach(x => x.Initialize());
            extensions[LoadOrder.After].ForEach(x => x.Initialize());
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
            return (AExtensionInitializer) Activator.CreateInstance(t);
        }
    }
}