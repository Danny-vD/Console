using System;

namespace Console.Vars.DefaultProviders
{
    /// <summary>
    /// VariableContainer Implementation that has no func name and provides default environment variables.
    /// </summary>
    public class DefaultVariables : VariableContainer
    {

        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static readonly DefaultVariables Instance;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static DefaultVariables()
        {
            Instance = new DefaultVariables();
        }

        /// <summary>
        /// Private Constructor.
        /// </summary>
        private DefaultVariables() : base("")
        {
            AddProvider(new DelegateVariableProvider("user", s => Environment.UserName));
            AddProvider(new DelegateVariableProvider("machine", s => Environment.MachineName));
            AddProvider(new DelegateVariableProvider("envs-all", s => EnvironmentVariableManager.GetEnvironmentList()));
        }

    }
}