using System.IO;
using Console.EnvironmentVariables;

namespace Console.PropIOCompat
{
    /// <summary>
    /// VariableProvider Implementation that returns the Files in the Specified Directory.
    /// </summary>
    public class FilesVariableProvider : VariableProvider
    {
        /// <summary>
        /// Helper Function that turns a String Array into a ; seperated list.
        /// </summary>
        /// <param name="li"></param>
        /// <returns></returns>
        public static string ToList(string[] li)
        {
            string s = "";
            foreach (string s1 in li)
            {
                s += s1 + "; ";
            }
            return s;
        }

        /// <summary>
        /// The Function name that is used to get this Variable.
        /// </summary>
        public override string FunctionName => "files";


        /// <summary>
        /// Returns the Files in the Specified Directory
        /// </summary>
        /// <param name="parameter">Path of the directory to be searched.</param>
        /// <returns>A list of Files</returns>
        public override string GetValue(string parameter)
        {
            return ToList(Directory.GetFiles(parameter, "*", SearchOption.TopDirectoryOnly));
        }
    }
}