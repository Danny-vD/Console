using System;
using System.Linq;

using Console.Core.ConverterSystem;

namespace Console.Utility.Converters
{
    /// <summary>
    /// AConverter Implementation that converts an Integer into the corresponding Enum Value
    /// </summary>
    public class EnumDigitConverter : AConverter
    {

        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string s && s.All(x => x == '-' || x >= '0' && x <= '9') && target == typeof(Enum);
        }

        /// <summary>
        /// Converts the Parameter into the Target Type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public override object Convert(object parameter, Type target)
        {
            string s = parameter.ToString();
            return Enum.ToObject(target, int.Parse(s));
        }

    }
}