using System;

using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;

namespace Console.Utility.Converters
{
    /// <summary>
    /// AConverter Implementation that converts the name of an enum to the corresponding enum value
    /// </summary>
    public class EnumConverter : AConverter
    {

        /// <summary>
        /// Flag that if set to true will Parse the Names in case sensitive.
        /// </summary>
        [Property("utility.converter.enum.casesensitive")]
        private static readonly bool CaseSensitive = true;

        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string && typeof(Enum).IsAssignableFrom(target);
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
            if (s.StartsWith(target.Name))
            {
                s = s.Remove(0, target.Name.Length);
            }

            return Enum.Parse(target, s, !CaseSensitive);
        }

    }
}