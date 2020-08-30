using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;


/// <summary>
/// Compatibility Layer for the Property System and the EnvironmentVariable Extension
/// Allows the Usage of Properties as Environment Variables
/// </summary>
namespace Console.PropEnvCompat
{
    /// <summary>
    /// Initializer of the PropEnvCompat Extension
    /// </summary>
    public class PropCompatInitializer : AExtensionInitializer
    {
        [Property("logs.propenvcompat.mute")]
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
        /// Version of the PropEnvCompat Extension
        /// </summary>
        [Property("version.propenvcompat")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<PropCompatInitializer>();
            EnvironmentVariableManager.AddProvider(new PropertyVariableProvider());
        }
    }
}