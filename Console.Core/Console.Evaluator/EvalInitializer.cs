using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
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


        /// <summary>
        /// Version of the Evaluator Extension
        /// </summary>
        [Property("version.evaluator")]
        private static Version EvalVersion => Assembly.GetExecutingAssembly().GetName().Version;


        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            EvalVariableProvider ep = new EvalVariableProvider();
            PropertyAttributeUtils.AddProperties<EvalInitializer>();
            PropertyAttributeUtils.AddProperties<EvalVariableProvider>();
        }
    }
}