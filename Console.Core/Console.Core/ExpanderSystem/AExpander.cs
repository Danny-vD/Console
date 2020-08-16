using Console.Core.ActivationSystem;

namespace Console.Core.ExpanderSystem
{
    [ActivateOn]
    public abstract class AExpander
    {
        public abstract string Expand(string input);
    }
}