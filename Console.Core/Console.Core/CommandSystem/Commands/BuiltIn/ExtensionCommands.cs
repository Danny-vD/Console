using Console.Core.ExtensionSystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Contains Default Commands to load Extensions from Files.
    /// </summary>
    public class ExtensionCommands
    {

        /// <summary>
        /// Adds all Extension commands.
        /// </summary>
        public static void AddExtensionsCommands()
        {
            CommandAttributeUtils.AddCommands<ExtensionCommands>();
        }
        
        /// <summary>
        /// Loads all extensions from the specified folder.
        /// </summary>
        /// <param name="folder">Folder with Extensions</param>
        [Command("load-extensions", "Loads the Extensions in the specified Folder")]
        public static void LoadFromFolder(string folder)
        {
            ExtensionLoader.LoadFromFolder(folder);
        }

        /// <summary>
        /// Loads the Specified Extension from File.
        /// </summary>
        /// <param name="file">.dll Library</param>
        [Command("load-extension", "Loads the specified extension")]
        public static void LoadExtensionFile(string file)
        {
            ExtensionLoader.LoadExtensionFile(file);
        }
    }
}