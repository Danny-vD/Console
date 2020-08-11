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
            return parameter is string && typeof(Enum).IsAssignableFrom(target);
        }

        public override object Convert(object parameter, Type target)
        {
            string s = parameter.ToString();
            if (s.StartsWith(target.Name)) s = s.Remove(0, target.Name.Length);
            return Enum.Parse(target, s, !CaseSensitive);
        }
    }
}