using System;
using Console.Core.ActivationSystem;

namespace Console.Core.ConverterSystem
{
    [ActivateOn]
    public abstract class AConverter
    {
        public abstract bool CanConvert(object parameter, Type target);
        public abstract object Convert(object parameter, Type target);
    }
}