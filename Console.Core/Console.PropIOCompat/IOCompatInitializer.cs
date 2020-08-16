using System;
using System.IO;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;

namespace Console.PropIOCompat
{
    public class IOCompatInitializer : AExtensionInitializer
    {

        [Property("version.propiocompat")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
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