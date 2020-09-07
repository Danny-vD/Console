using System;
using System.Reflection;

using Console.Core;
using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Builder;
using Console.Core.ExpanderSystem;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;
using Console.Vars.DefaultProviders;
using Console.Vars.DefaultProviders.IO;


/// <summary>
/// The EnvironmentVariables Extension Implements an AExpander that allows the usage of $[funcname]([data]) syntax
/// It implements a Default Funcname $([data]) that does not have a func name.
/// </summary>
namespace Console.Vars
{
    /// <summary>
    /// Initializer of the EnvironmentVariables Extension
    /// </summary>
    public class EnvInitializer : AExtensionInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        public const string VARS_NAMESPACE = "vars";

        [Property("logs."+ VARS_NAMESPACE + ".mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        /// <summary>
        /// Logger for this Extension
        /// </summary>
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());

        /// <summary>
        /// The Load Order of the EnvironmentVariable Extensions
        /// </summary>
        public override LoadOrder Order => LoadOrder.First;


        /// <summary>
        /// Version of the Environment Variables Extension
        /// </summary>
        [Property("version." + VARS_NAMESPACE)]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            CommandBuilder.AddProvider(new EnvironmentVariableAutoFillProvider());
            ConsoleCoreConfig.AddEscapeChar(EnvironmentVariableManager.ActivationPrefix);
            PropertyAttributeUtils.AddProperties<EnvInitializer>();
            EnvironmentVariableManager.AddProvider(DefaultVariables.Instance);
            EnvironmentVariableManager.AddProvider(new PropertyVariableProvider());
            EnvironmentVariableManager.AddProvider(new RangeVariableProvider());
            EnvironmentVariableManager.AddProvider(new ClassQueryProvider());
            IOProviderInitializer.Initialize();
            CommandAttributeUtils.AddCommands(typeof(EnvironmentVariableManager));
            AConsoleManager.ExpanderManager.AddExpander(new EnvExpander());
        }

        /// <summary>
        /// AExpander Implementation for the environment variables
        /// </summary>
        private class EnvExpander : AExpander
        {

            /// <summary>
            /// Returns the Expanded String based on the Input String
            /// </summary>
            /// <param name="input">Input String</param>
            /// <returns>Expanded String</returns>
            public override string Expand(string input)
            {
                return EnvironmentVariableManager.Expand(input);
            }

        }

    }
}