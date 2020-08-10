using System;
using System.Globalization;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;

namespace Console.DefaultConverters
{
    public class DateTimeConverter : AConverter
    {

        [Property("defaultconverters.datetime.culture")]
        private static string DateTimeCulture = "en-US";

        [Property("defaultconverters.datetime.format")]
        private static string DateTimeFormat = "G";

        private static CultureInfo Culture => CultureInfo.CreateSpecificCulture(DateTimeCulture);

        public override object Convert(object parameter, Type target)
        {
            if (parameter is DateTime dt)
            {
                return dt.ToString(DateTimeFormat, Culture);
            }
            return DateTime.Parse(parameter.ToString(), Culture);
        }

        public override bool CanConvert(object parameter, Type target)
        {
            return (parameter is DateTime && typeof(string) == target) ||
                   (parameter is string && typeof(DateTime) == target);
        }
    }
}