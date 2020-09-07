using System.IO;

using Console.Core.CommandSystem.Attributes;

namespace Console.IO
{
    /// <summary>
    /// Commands that can be useful when working with filesystems
    /// </summary>
    public class IOCommands
    {

        /// <summary>
        /// 
        /// </summary>
        public const string IO_NAMESPACE = "io";

        /// <summary>
        /// Changes the Current Directory.
        /// </summary>
        /// <param name="dir">Relative Path</param>
        [Command(
            "change-dir",
            HelpMessage = "Changes the Current Directory",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "cd" }
        )]
        private static void ChangeDir(string dir)
        {
            Directory.SetCurrentDirectory(dir);
        }

        /// <summary>
        /// Lists files in the current directory
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        /// <param name="names">Only displays names</param>
        /// </summary>
        [Command(
            "list-files",
            HelpMessage = "Lists files in the current directory",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "ls", "dir" }
        )]
        private static void ListFiles(
            [CommandFlag]
            bool recursive, [CommandFlag]
            bool names)
        {
            ListFiles(".\\", recursive, names);
        }

        /// <summary>
        /// Lists files in the specified directory
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        /// <param name="names">Only displays names</param>
        [Command(
            "list-files",
            HelpMessage = "Lists files in the specified directory",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "ls", "dir" }
        )]
        private static void ListFiles(string folder, [CommandFlag]
            bool recursive,
            [CommandFlag]
            bool names)
        {
            ListFiles(folder, "*", recursive, names);
        }


        /// <summary>
        /// Lists files in the specified directory and all subdirectories that match the search term
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="searchTerms">The Search Term</param>
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        /// <param name="names">Only displays names</param>
        [Command(
            "list-files",
            HelpMessage =
                "Lists files in the selected directory that match the search pattern. Optionally recursing into the subdirectories",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "ls", "dir" }
        )]
        private static void ListFiles(string folder, string searchTerms, [CommandFlag] bool recursive,
            [CommandFlag]
            bool names)
        {
            string path = Path.GetFullPath(folder);
            string[] files = Directory.GetFiles(
                                                path,
                                                searchTerms,
                                                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
                                               );
            string s = "Files in " + path + "\n";
            foreach (string file in files)
            {
                s += "\t" + (names ? file.Replace(Path.GetFullPath(folder), "") : file) + "\n";
            }

            IOInitializer.Logger.Log(s);
        }

        /// <summary>
        /// Lists files in the current directory
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        /// <param name="names">Only displays names</param>
        /// </summary>
        [Command(
            "list-dir",
            HelpMessage = "Lists directories in the current directory",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "ld", "dirs" }
        )]
        private static void ListDirectories(
            [CommandFlag]
            bool recursive, [CommandFlag]
            bool names)
        {
            ListDirectories(".\\", recursive, names);
        }

        /// <summary>
        /// Lists files in the specified directory
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        /// <param name="names">Only displays names</param>
        [Command(
            "list-dir",
            HelpMessage = "Lists directories in the specified directory",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "ld", "dirs" }
        )]
        private static void ListDirectories(
            string folder, [CommandFlag]
            bool recursive, [CommandFlag]
            bool names)
        {
            ListDirectories(folder, "*", recursive, names);
        }

        /// <summary>
        /// Lists Directories in the specified directory and all subdirectories that match the search term
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="searchTerms">The Search Term</param>
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        /// <param name="names">Only displays names</param>
        [Command(
            "list-dir",
            HelpMessage =
                "Lists directories in the selected directory that match the search pattern. Optionally recursing into the subdirectories",
            Namespace = IO_NAMESPACE,
            Aliases = new[] { "ld", "dirs" }
        )]
        private static void ListDirectories(
            string folder, string searchTerms, [CommandFlag]
            bool recursive,
            [CommandFlag]
            bool names)
        {
            string path = Path.GetFullPath(folder);
            string[] files = Directory.GetDirectories(
                                                      path,
                                                      searchTerms,
                                                      recursive
                                                          ? SearchOption.AllDirectories
                                                          : SearchOption.TopDirectoryOnly
                                                     );
            string s = "Directories in " + path + "\n";
            foreach (string file in files)
            {
                s += "\t" + (names ? file.Replace(Path.GetFullPath(folder), "") : file) + "\n";
            }

            IOInitializer.Logger.Log(s);
        }

        /// <summary>
        /// Copies a File from A to B
        /// </summary>
        /// <param name="from">Source File</param>
        /// <param name="to">Destination File</param>
        [Command("copy", Namespace = IO_NAMESPACE, HelpMessage = "Copies a File to a specified Location")]
        private static void Copy(
            string from, 
            string to)
        {
            if (File.Exists(from))
            {
                File.Copy(from, to, true);
            }
        }

        /// <summary>
        /// Moves a File from A to B
        /// </summary>
        /// <param name="from">Source File</param>
        /// <param name="to">Destination File</param>
        [Command("move", Namespace = IO_NAMESPACE, HelpMessage = "Moves a File to a specified Location")]
        private static void Move(
            string from, 
            string to)
        {
            if (File.Exists(from))
            {
                if (File.Exists(to))
                {
                    File.Delete(to);
                }

                File.Move(from, to);
            }
            else if (Directory.Exists(from))
            {
                Directory.Move(from, to);
            }
        }

        /// <summary>
        /// Deletes the Specified File
        /// </summary>
        /// <param name="file">File to Delete</param>
        [Command("delete", Namespace = IO_NAMESPACE, HelpMessage = "Deletes a File", Aliases = new[] { "del", "rm" })]
        private static void Delete(
            string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

    }
}