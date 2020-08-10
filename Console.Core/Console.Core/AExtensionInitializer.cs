using System.IO;
using Console.Core.Commands.BuiltIn;

namespace Console.Core
{
    /// <summary>
    /// Helper Class that has to be implemented with an empty public constructor to be detected and loaded by the console system
    /// </summary>
    public abstract class AExtensionInitializer
    {
        public virtual LoadOrder Order => LoadOrder.Default;

        /// <summary>
        /// Initializes the Extensions in this Assembly.
        /// </summary>
        public abstract void Initialize();

        public static void LoadExtensions(string folder)
        {
            Directory.CreateDirectory(folder);
            string[] exts = Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories);

            ExtensionCommands.LoadExtensions(exts);
        }
    }
}