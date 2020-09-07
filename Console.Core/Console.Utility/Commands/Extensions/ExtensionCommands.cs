using Console.Core.CommandSystem.Attributes;
using Console.Core.ExtensionSystem;
using Console.Utility.AutoFill.IOAutoFill;
using Console.Utility.AutoFill.IOAutoFill.Files;

namespace Console.Utility.Commands.Extensions
{
    /// <summary>
    /// Contains Default Commands to load Extensions from Files.
    /// </summary>
    public class ExtensionCommands
    {

        /// <summary>
        /// 
        /// </summary>
        private const string EXTENSION_NAMESPACE = UtilExtensionInitializer.UTIL_NAMESPACE + "::extension";

        /// <summary>
        /// Adds all Extension Commands.
        /// </summary>
        public static void AddExtensionsCommands()
        {
            CommandAttributeUtils.AddCommands<ExtensionCommands>();
        }

        /// <summary>
        /// Loads all extensions from the specified folder.
        /// </summary>
        /// <param name="folder">Folder with Extensions</param>
        [Command(
            "load-extensions",
            HelpMessage = "Loads the Extensions in the specified Folder",
            Namespace = EXTENSION_NAMESPACE
        )]
        public static void LoadFromFolder(
            [IOAutoFill]
            string folder)
        {
            ExtensionLoader.LoadFromFolder(folder);
        }

        /// <summary>
        /// Loads the Specified Extension from File.
        /// </summary>
        /// <param name="file">.dll Library</param>
        [Command(
            "load-extension",
            HelpMessage = "Loads the specified extension",
            Namespace = EXTENSION_NAMESPACE
        )]
        public static void LoadExtensionFile(
            [FileAutoFill("dll")]
            string file)
        {
            ExtensionLoader.LoadExtensionFile(file);
        }

    }
}