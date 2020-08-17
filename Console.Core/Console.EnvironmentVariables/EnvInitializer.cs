using System;
using System.Reflection;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.ExpanderSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;


/// <summary>
/// The EnvironmentVariables Extension Implements an AExpander that allows the usage of $[funcname]([data]) syntax
/// It implements a Default Funcname $([data]) that does not have a func name.
/// </summary>
namespace Console.EnvironmentVariables
{


    /// <summary>
    /// Initializer of the EnvironmentVariables Extension
    /// </summary>
    public class EnvInitializer : AExtensionInitializer
    {
        /// <summary>
        /// The Load Order of the EnvironmentVariable Extensions
        /// </summary>
        public override LoadOrder Order => LoadOrder.First;

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



        /// <summary>
        /// Version of the Environment Variables Extension
        /// </summary>
        [Property("version.environmentvariables")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<EnvInitializer>();
            EnvironmentVariableManager.AddProvider(DefaultVariables.Instance);
            CommandAttributeUtils.AddCommands(typeof(EnvironmentVariableManager));
            AConsoleManager.ExpanderManager.AddExpander(new EnvExpander());
        }
    }
}