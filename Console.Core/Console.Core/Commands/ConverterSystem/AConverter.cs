using System;

namespace Console.Core.Commands.ExpandSystem
{
    public abstract class AConverter
    {
        public abstract bool CanConvert(object parameter, Type target);
        public abstract object Convert(object parameter, Type target);
    }
}