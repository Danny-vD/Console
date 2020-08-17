using System;
using System.IO;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;



/// <summary>
/// Compatibility Layer for the Property System and the IO Extension
/// Allows the Usage of environment variables(like $(cd) = Current Directory)
/// </summary>
namespace Console.PropIOCompat
{


    /// <summary>
    /// Initializer of the PropIOCompat Extension
    /// </summary>
    public class IOCompatInitializer : AExtensionInitializer
    {

        /// <summary>
        /// Version of the PropIOCompat Extension
        /// </summary>
        [Property("version.propiocompat")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<IOCompatInitializer>();
            EnvironmentVariableManager.AddProvider(new FilesVariableProvider());
            EnvironmentVariableManager.AddProvider(new DirectoriesVariableProvider());
            DefaultVariables.Instance.AddProvider(new DelegateVariableProvider("cd", s => Directory.GetCurrentDirectory()));
            DefaultVariables.Instance.AddProvider(new DelegateVariableProvider("files", s => FilesVariableProvider.ToList(Directory.GetFiles(".\\", "*", SearchOption.TopDirectoryOnly))));
            DefaultVariables.Instance.AddProvider(new DelegateVariableProvider("dirs", s => FilesVariableProvider.ToList(Directory.GetDirectories(".\\", "*", SearchOption.TopDirectoryOnly))));
        }
    }
}