using System;

namespace Console.EnvironmentVariables
{
    public class DefaultVariables : VariableContainer
    {
        public static readonly DefaultVariables Instance;
        

        static DefaultVariables()
        {
            Instance = new DefaultVariables();
        }

        private DefaultVariables():base("")
        {
            AddProvider(new DelegateVariableProvider("user", s => Environment.UserName));
            AddProvider(new DelegateVariableProvider("machine", s => Environment.MachineName));
            AddProvider( new DelegateVariableProvider("envs-all", s => EnvironmentVariableManager.EnvList));
        }


    }
}