using System;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;

namespace Console.DefaultConverters
{
    public class EnumConverter : AConverter
    {


        [Property("defaultconverters.enumconverter.casesensitive")]
        private static bool CaseSensitive = true;

        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string && target == typeof(Enum);
        }

        public override object Convert(object parameter, Type target)
        {
            string s = parameter.ToString();
            if (!s.StartsWith(target.Name)) s = target.Name + "." + s;
            return Enum.Parse(target, s, !CaseSensitive);
        }
    }
}