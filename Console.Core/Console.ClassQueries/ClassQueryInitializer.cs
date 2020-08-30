using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
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
        [Property("logs.classqueries.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }


        /// <summary>
        /// The Load Order of the Extension
        /// </summary>
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Version of the ClassQueries Extension
        /// </summary>
        [Property("version.classqueries")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ClassQueryInitializer>();
            EnvironmentVariableManager.AddProvider(new ClassQueryProvider());
        }
    }
}