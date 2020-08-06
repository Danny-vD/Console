using System;
using System.Linq;
using Console.Core.Commands.ConverterSystem;

namespace Console.DefaultConverters
{
    public class EnumDigitConverter : AConverter
    {
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string s && s.All(x => x == '-' || (x >= '0' && x <= '9')) && target == typeof(Enum);
        }

        public override object Convert(object parameter, Type target)
        {
            string s = parameter.ToString();
            return Enum.ToObject(target, int.Parse(s));
        }
    }
}