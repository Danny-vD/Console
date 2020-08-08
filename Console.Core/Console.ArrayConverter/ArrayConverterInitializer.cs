using Console.Core;
using Console.Core.ConverterSystem;

namespace Console.ArrayConverter
{
    public class ArrayConverterInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            CustomConvertManager.AddConverter(new ArrayCustomConverter());
        }
    }
}