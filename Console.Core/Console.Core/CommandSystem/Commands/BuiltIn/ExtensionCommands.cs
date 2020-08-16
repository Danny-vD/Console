using Console.Core.ExtensionSystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    public class ExtensionCommands
    {

        public static void AddExtensionsCommands()
        {
            CommandAttributeUtils.AddCommands<ExtensionCommands>();
        }
        
        [Command("load-extensions", "Loads the Extensions in the specified Folder")]
        public static void LoadFromFolder(string folder)
        {
            ExtensionLoader.LoadFromFolder(folder);
        }

        [Command("load-extension", "Loads the specified extension")]
        public static void LoadExtensionFile(string file)
        {
            ExtensionLoader.LoadExtensionFile(file);
        }
    }
}