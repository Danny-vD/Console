
using System.IO;
using Console.Core;
using Console.EnvironmentVariables;

public class IOCompatInitializer : AExtensionInitializer
{
    public override void Initialize()
    {
        EnvironmentVariableManager.AddProvider(new FilesVariableProvider());
        EnvironmentVariableManager.AddProvider(new DirectoriesVariableProvider());
        DefaultVariables.Instance.AddProvider(new DelegateVariableProvider("cd", s => Directory.GetCurrentDirectory()));
        DefaultVariables.Instance.AddProvider(new DelegateVariableProvider("files", s => FilesVariableProvider.ToList(Directory.GetFiles(".\\", "*", SearchOption.TopDirectoryOnly))));
        DefaultVariables.Instance.AddProvider(new DelegateVariableProvider("dirs", s => FilesVariableProvider.ToList(Directory.GetDirectories(".\\", "*", SearchOption.TopDirectoryOnly))));
    }
}