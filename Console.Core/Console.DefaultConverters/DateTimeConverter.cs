using System;
using System.Globalization;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;

namespace Console.DefaultConverters
{
    /// <summary>
    /// AConverter Implementation that converts from DateTime structs to string representations and from string representations to DateTime Structs
    /// </summary>
    public class DateTimeConverter : AConverter
    {
        /// <summary>
        /// Culture that is used when converting
        /// </summary>
        [Property("defaultconverters.datetime.culture")]
        private static string DateTimeCulture = "en-US";

        /// <summary>
        /// The Format string that is used when converting from DateTime To String.
        /// </summary>
        [Property("defaultconverters.datetime.format")]
        private static string DateTimeFormat = "G";

        /// <summary>
        /// Helper Property that provides the CultureInstance
        /// </summary>
        private static CultureInfo Culture => CultureInfo.CreateSpecificCulture(DateTimeCulture);

        /// <summary>
        /// Converts the Parameter into the Target Type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public override object Convert(object parameter, Type target)
        {
            if (parameter is DateTime dt)
            {
                return dt.ToString(DateTimeFormat, Culture);
            }
            return DateTime.Parse(parameter.ToString(), Culture);
        }

        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public override bool CanConvert(object parameter, Type target)
        {
            return (parameter is DateTime && typeof(string) == target) ||
                   (parameter is string && typeof(DateTime) == target);
        }
    }
}