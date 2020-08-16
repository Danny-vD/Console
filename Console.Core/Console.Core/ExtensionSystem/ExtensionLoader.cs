using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Console.Core.ActivationSystem;

namespace Console.Core.ExtensionSystem
{
    public static class ExtensionLoader
    {
        #region Load Extensions

        public static void LoadFromFolder(string folder)
        {
            LoadExtensionFiles(Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories));
        }
        
        public static void LoadExtensionFile(string file)
        {
            LoadExtensionFiles(new[] { file });
        }

        public static void LoadExtensionFiles(string[] paths)
        {
            AConsoleManager.Instance.Log($"Loading {paths.Length} Extensions...");
            List<AExtensionInitializer> exts = new List<AExtensionInitializer>();
            paths.Select(LoadAssembly).Where(x => x != null).ToList().ForEach(x => exts.AddRange(x));
            ProcessLoadOrder(exts.ToArray());
        }

        public static void ProcessLoadOrder(AExtensionInitializer[] exts)
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
            InitializeExtensions(extensions);
        }

        #endregion

        #region Internal

        private static void InitializeExtensions(Dictionary<LoadOrder, List<AExtensionInitializer>> extensions)
        {
            string s = "Initializing Extensions...";
            int i = InitializeExtensions(extensions[LoadOrder.First]);
            s += "\nLoadOrder.First: " + extensions[LoadOrder.First].Count;
            i += InitializeExtensions(extensions[LoadOrder.Default]);
            s += "\nLoadOrder.Default: " + extensions[LoadOrder.Default].Count;
            i += InitializeExtensions(extensions[LoadOrder.After]);
            s += "\nLoadOrder.After: " + extensions[LoadOrder.After].Count;
            s += "\nTotal Initialized Extensions: " + i + "\n";
            AConsoleManager.Instance.Log(s);
        }

        private static int InitializeExtensions(List<AExtensionInitializer> extensions)
        {
            foreach (AExtensionInitializer aExtensionInitializer in extensions)
            {
                aExtensionInitializer.Initialize();
            }
            return extensions.Count;
        }


        private static AExtensionInitializer[] LoadAssembly(string path)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(path);
                AExtensionInitializer[] inits = ActivateOnAttributeUtils.ActivateObjects<AExtensionInitializer>(asm);
                if (inits.Length == 0)
                    AConsoleManager.Instance.LogWarning("Assembly " + asm.GetName().Name + " does not have an Initializer but is loaded.");
                return inits;
            }
            catch (Exception e)
            {
                AConsoleManager.Instance.LogWarning("Can not load Extension: " + path);
                AConsoleManager.Instance.LogWarning(e);
                return new AExtensionInitializer[0];
            }
        }

        #endregion

    }
}