using System;
using Console.Core.ConverterSystem;

namespace Console.DefaultConverters
{
    public class DateTimeConverter : AConverter
    {
        public override object Convert(object parameter, Type target)
        {
            return DateTime.Parse(parameter.ToString());
        }

        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string && typeof(DateTime) == target;
        }
    }
}