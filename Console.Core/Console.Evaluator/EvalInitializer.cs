using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

namespace Console.Evaluator
{
    public class EvalInitializer : AExtensionInitializer
    {
        [Property("version.evaluator")]
        private static Version EvalVersion => Assembly.GetExecutingAssembly().GetName().Version;

        public override void Initialize()
        {
            EvalVariableProvider ep = new EvalVariableProvider();
            PropertyAttributeUtils.AddProperties<EvalInitializer>();
            PropertyAttributeUtils.AddProperties<EvalVariableProvider>();
        }
    }
}