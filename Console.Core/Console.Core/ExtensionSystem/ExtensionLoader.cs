using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Console.Core.ActivationSystem;

/// <summary>
/// The Console.Core.ExtensionSystem namespace is used by the Core Library to load Extensions.
/// This namespace contains the AExtensionInitializer class that can be used to execute any initialization logic on load from within the extension
/// </summary>
namespace Console.Core.ExtensionSystem
{
    /// <summary>
    /// Extension System API
    /// </summary>
    public static class ExtensionLoader
    {
        #region Load Extensions

        /// <summary>
        /// Loads all Extensions contained in the specified folder.
        /// </summary>
        /// <param name="folder">Folder Containing Extensions</param>
        public static void LoadFromFolder(string folder)
        {
            LoadExtensionFiles(Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories));
        }
        
        /// <summary>
        /// Loads an Extension from File.
        /// </summary>
        /// <param name="file">Extension Path</param>
        public static void LoadExtensionFile(string file)
        {
            LoadExtensionFiles(new[] { file });
        }

        /// <summary>
        /// Loads all specified Extensions
        /// </summary>
        /// <param name="paths">Extension Paths</param>
        public static void LoadExtensionFiles(string[] paths)
        {
            ConsoleCoreConfig.CoreLogger.Log($"Loading {paths.Length} Extensions...");
            List<AExtensionInitializer> exts = new List<AExtensionInitializer>();
            paths.Select(LoadAssembly).Where(x => x != null).ToList().ForEach(x => exts.AddRange(x));
            ProcessLoadOrder(exts.ToArray());
        }

        /// <summary>
        /// Orders the Extensions in the correct load order.
        /// </summary>
        /// <param name="exts">Extensions to be Ordered.</param>
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

        /// <summary>
        /// Initializes the Ordered Extensions.
        /// </summary>
        /// <param name="extensions">Extensions to Initialize</param>
        private static void InitializeExtensions(Dictionary<LoadOrder, List<AExtensionInitializer>> extensions)
        {
            string s = "\nInitializing Extensions...";
            int i = InitializeExtensions(extensions[LoadOrder.First]);
            s += "\nLoadOrder.First: " + extensions[LoadOrder.First].Count;
            i += InitializeExtensions(extensions[LoadOrder.Default]);
            s += "\nLoadOrder.Default: " + extensions[LoadOrder.Default].Count;
            i += InitializeExtensions(extensions[LoadOrder.After]);
            s += "\nLoadOrder.After: " + extensions[LoadOrder.After].Count;
            s += "\nTotal Initialized Extensions: " + i + "\n";
            ConsoleCoreConfig.CoreLogger.Log(s);
        }

        /// <summary>
        /// Initializes the Specified Extensions
        /// </summary>
        /// <param name="extensions">Extensions to Initialize</param>
        /// <returns>Loaded Extensions</returns>
        private static int InitializeExtensions(List<AExtensionInitializer> extensions)
        {
            foreach (AExtensionInitializer aExtensionInitializer in extensions)
            {
                aExtensionInitializer._InnerInitialize();
            }
            return extensions.Count;
        }

        /// <summary>
        /// Loads all Extension Initializer Instances from an Assembly.
        /// </summary>
        /// <param name="path">Path to the Assembly</param>
        /// <returns>Extension Initializers in the Extension</returns>
        private static AExtensionInitializer[] LoadAssembly(string path)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(path);
                AExtensionInitializer[] inits = ActivateOnAttributeUtils.ActivateObjects<AExtensionInitializer>(asm);
                if (inits.Length == 0)
                    ConsoleCoreConfig.CoreLogger.LogWarning("Assembly " + asm.GetName().Name + " does not have an Initializer but is loaded.");
                return inits;
            }
            catch (Exception e)
            {
                ConsoleCoreConfig.CoreLogger.LogWarning("Can not load Extension: " + path);
                ConsoleCoreConfig.CoreLogger.LogWarning(e);
                return new AExtensionInitializer[0];
            }
        }

        #endregion

    }
}