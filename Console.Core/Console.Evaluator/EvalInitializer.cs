using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;


/// <summary>
/// The Evaluator Extension implements a Variable Container in the EnvironmentVariable System.
/// Common Expressions can be evaluated during runtime.
/// </summary>
namespace Console.Evaluator
{


    /// <summary>
    /// Initializer of the Evaluator Extension
    /// </summary>
    public class EvalInitializer : AExtensionInitializer
    {
        [Property("logs.evaluator.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());
        /// <summary>
        /// Version of the Evaluator Extension
        /// </summary>
        [Property("version.evaluator")]
        private static Version EvalVersion => Assembly.GetExecutingAssembly().GetName().Version;


        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            EvalVariableProvider ep = new EvalVariableProvider();
            PropertyAttributeUtils.AddProperties<EvalInitializer>();
            PropertyAttributeUtils.AddProperties<EvalVariableProvider>();
        }
    }
}