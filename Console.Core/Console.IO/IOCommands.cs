using System.IO;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.IO
{
    /// <summary>
    /// Commands that can be useful when working with filesystems
    /// </summary>
    public class IOCommands
    {
        /// <summary>
        /// Changes the Current Directory.
        /// </summary>
        /// <param name="dir"></param>
        [Command("change-dir", "Changes the Current Directory", "cd")]
        private static void ChangeDir(string dir)
        {
            Directory.SetCurrentDirectory(dir);
        }

        /// <summary>
        /// Lists files in the current directory
        /// </summary>
        [Command("list-files", "Lists files in the current directory", "ls", "dir")]
        private static void ListFiles() => ListFiles(".\\");

        /// <summary>
        /// Lists files in the specified directory
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        [Command("list-files", "Lists files in the specified directory", "ls", "dir")]
        private static void ListFiles(string folder) => ListFiles(folder, "*");


        /// <summary>
        /// Lists files in the specified directory that match the search term
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="searchTerm">The Search Term</param>
        [Command("list-files", "Lists files in the selected directory that match the search pattern", "ls", "dir")]
        private static void ListFiles(string folder, string searchTerm) => ListFiles(folder, searchTerm, false);

        /// <summary>
        /// Lists files in the specified directory and all subdirectories that match the search term
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="searchTerm">The Search Term</param>
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        [Command("list-files", "Lists files in the selected directory that match the search pattern. Optionally recursing into the subdirectories", "ls", "dir")]
        private static void ListFiles(string folder, string searchTerms, bool recursive)
        {
            string path = Path.GetFullPath(folder);
            string[] files = Directory.GetFiles(path, searchTerms,
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            string s = "Files in " + path + "\n";
            foreach (string file in files)
            {
                s += "\t" + file + "\n";
            }
            AConsoleManager.Instance.Log(s);
        }

        /// <summary>
        /// Lists files in the current directory
        /// </summary>
        [Command("list-dir", "Lists directories in the current directory", "ld", "dirs")]
        private static void ListDirectories() => ListDirectories(".\\");
        /// <summary>
        /// Lists files in the specified directory
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        [Command("list-dir", "Lists directories in the specified directory", "ld", "dirs")]
        private static void ListDirectories(string folder) => ListDirectories(folder, "*");
        /// <summary>
        /// Lists files in the specified directory that match the search term
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="searchTerm">The Search Term</param>
        [Command("list-dir", "Lists directories in the selected directory that match the search pattern", "ld", "dirs")]
        private static void ListDirectories(string folder, string searchTerm) => ListDirectories(folder, searchTerm, false);

        /// <summary>
        /// Lists Directories in the specified directory and all subdirectories that match the search term
        /// </summary>
        /// <param name="folder">Specified Directory</param>
        /// <param name="searchTerm">The Search Term</param>
        /// <param name="recursive">Flag that specifies if the Search should be recursive.</param>
        [Command("list-dir", "Lists directories in the selected directory that match the search pattern. Optionally recursing into the subdirectories", "ld", "dirs")]
        private static void ListDirectories(string folder, string searchTerms, bool recursive)
        {
            string path = Path.GetFullPath(folder);
            string[] files = Directory.GetDirectories(path, searchTerms,
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            string s = "Directories in " + path + "\n";
            foreach (string file in files)
            {
                s += "\t" + file + "\n";
            }
            AConsoleManager.Instance.Log(s);
        }

        /// <summary>
        /// Copies a File from A to B
        /// </summary>
        /// <param name="from">Source File</param>
        /// <param name="to">Destination File</param>
        private static void Copy(string from, string to)
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
        private static void Move(string from, string to)
        {
            if (File.Exists(from))
            {
                if (File.Exists(to)) File.Delete(to);
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
        private static void Delete(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}