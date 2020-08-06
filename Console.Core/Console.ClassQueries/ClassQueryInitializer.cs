using Console.Core;
using Console.EnvironmentVariables;

namespace Console.ClassQueries
{

    public class ClassQueryInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            EnvironmentVariableManager.AddProvider(new ClassQueryProvider());
        }
    }
}
