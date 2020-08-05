using System.IO;
using Console.EnvironmentVariables;

namespace Console.PropIOCompat
{
    public class FilesVariableProvider : VariableProvider
    {
        public static string ToList(string[] li)
        {
            string s = "";
            foreach (string s1 in li)
            {
                s += s1 + "; ";
            }
            return s;
        }

        public override string FunctionName => "files";
        public override string GetValue(string parameter)
        {
            return ToList(Directory.GetFiles(parameter, "*", SearchOption.TopDirectoryOnly));
        }
    }
}