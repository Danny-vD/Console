using System;
using System.Reflection;
using Console.Core;
using Console.Core.PropertySystem;
using Console.Core.Utils;

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