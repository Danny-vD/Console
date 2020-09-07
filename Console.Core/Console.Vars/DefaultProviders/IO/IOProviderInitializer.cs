using System.IO;

namespace Console.Vars.DefaultProviders.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class IOProviderInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            EnvironmentVariableManager.AddProvider(new FilesVariableProvider());
            EnvironmentVariableManager.AddProvider(new DirectoriesVariableProvider());

            VariableProvider currentDirectoryProvider =
                new DelegateVariableProvider("cd", s => Directory.GetCurrentDirectory());
            DefaultVariables.Instance.AddProvider(currentDirectoryProvider);

            VariableProvider filesProvider =
                new DelegateVariableProvider(
                                             "files",
                                             s => FilesVariableProvider.ToList(
                                                                               Directory
                                                                                   .GetFiles(
                                                                                             ".\\",
                                                                                             "*",
                                                                                             SearchOption
                                                                                                 .TopDirectoryOnly
                                                                                            )
                                                                              )
                                            );
            DefaultVariables.Instance.AddProvider(filesProvider);

            VariableProvider dirsProvider =
                new DelegateVariableProvider(
                                             "dirs",
                                             s => FilesVariableProvider.ToList(
                                                                               Directory
                                                                                   .GetDirectories(
                                                                                                   ".\\",
                                                                                                   "*",
                                                                                                   SearchOption
                                                                                                       .TopDirectoryOnly
                                                                                                  )
                                                                              )
                                            );
            DefaultVariables.Instance.AddProvider(dirsProvider);
        }

    }
}