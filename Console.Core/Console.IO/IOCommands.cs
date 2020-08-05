using System.IO;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;

namespace Console.IO
{
    public class IOCommands
    {
        [Command("change-dir", "Changes the Current Directory", "cd")]
        private static void ChangeDir(string dir)
        {
            Directory.SetCurrentDirectory(dir);
        }

        [Command("list-files", "Lists files in the current directory", "ls", "dir")]
        private static void ListFiles() => ListFiles(".\\");

        [Command("list-files", "Lists files in the specified directory", "ls", "dir")]
        private static void ListFiles(string folder) => ListFiles(folder, "*");

        [Command("list-files", "Lists files in the selected directory that match the search pattern", "ls", "dir")]
        private static void ListFiles(string folder, string searchTerm) => ListFiles(folder, searchTerm, false);

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

        [Command("list-dir", "Lists directories in the current directory", "ld", "dirs")]
        private static void ListDirectories() => ListDirectories(".\\");

        [Command("list-dir", "Lists directories in the specified directory", "ld", "dirs")]
        private static void ListDirectories(string folder) => ListDirectories(folder, "*");

        [Command("list-dir", "Lists directories in the selected directory that match the search pattern", "ld", "dirs")]
        private static void ListDirectories(string folder, string searchTerm) => ListDirectories(folder, searchTerm, false);

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


        private static void Copy(string from, string to)
        {
            if (File.Exists(from))
            {
                File.Copy(from, to, true);
            }
        }

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

        private static void Delete(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}