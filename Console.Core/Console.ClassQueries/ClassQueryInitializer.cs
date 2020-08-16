using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;

namespace Console.ClassQueries
{

    public class ClassQueryInitializer : AExtensionInitializer
    {
        [Property("version.classqueries")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ClassQueryInitializer>();
            EnvironmentVariableManager.AddProvider(new ClassQueryProvider());
        }
    }
}
