using System.Reflection;
using Console.Core.LogSystem;

namespace Console.Core.ExtensionSystem
{
    internal class ExtensionData
    {
        public readonly Assembly Assembly;
        public readonly ALogger Logger;

        public ExtensionData(Assembly asm)
        {
            Assembly = asm;
            Logger = new PrefixLogger($"[{asm.GetName().Name}]");
        }
    }
}