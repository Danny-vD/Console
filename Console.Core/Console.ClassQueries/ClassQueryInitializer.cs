using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;

/// <summary>
/// ClassQueries Extension is used to query all Loaded C# Types by a search string.
/// The Return is the Fully Qualified Assembly Name of the Type
/// </summary>
namespace Console.ClassQueries
{

    /// <summary>
    /// Initializer of the ClassQueries Extension
    /// </summary>
    public class ClassQueryInitializer : AExtensionInitializer
    {

        /// <summary>
        /// Version of the ClassQueries Extension
        /// </summary>
        [Property("version.classqueries")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ClassQueryInitializer>();
            EnvironmentVariableManager.AddProvider(new ClassQueryProvider());
        }
    }
}
