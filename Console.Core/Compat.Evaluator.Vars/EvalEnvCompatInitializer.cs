using Console.Core.ExtensionSystem;
using Console.Vars;

namespace Compat.Evaluator.Vars
{
    public class EvalEnvCompatInitializer : AExtensionInitializer
    {

        protected override void Initialize()
        {
            EnvironmentVariableManager.AddProvider(new EvalVariableProvider());
        }

    }
}
