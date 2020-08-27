using System.IO;
using Console.EnvironmentVariables;

namespace Console.PropIOCompat
{
    /// <summary>
    /// VariableProvider Implementation that returns the Directories in the Specified Directory.
    /// </summary>
    public class DirectoriesVariableProvider : VariableProvider
    {
        /// <summary>
        /// The Function name that is used to get this Variable.
        /// </summary>
        public override string FunctionName => "dirs";

        /// <summary>
        /// Returns the Directories in the Specified Directory
        /// </summary>
        /// <param name="parameter">Path of the directory to be searched.</param>
        /// <returns>A list of Directories</returns>
        public override string GetValue(string parameter)
        {
            return FilesVariableProvider.ToList(Directory.GetDirectories(parameter, "*",
                SearchOption.TopDirectoryOnly));
        }
    }
}