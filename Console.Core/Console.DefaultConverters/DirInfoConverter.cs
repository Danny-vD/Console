using System;
using System.IO;
using Console.Core.ConverterSystem;

namespace Console.DefaultConverters
{
    /// <summary>
    /// AConverter Implementation that converts a Path String into a DirectoryInfo class.
    /// </summary>
    public class DirInfoConverter : AConverter
    {
        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string && target == typeof(DirectoryInfo);
        }
        /// <summary>
        /// Converts the Parameter into the Target Type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public override object Convert(object parameter, Type target)
        {
            return new DirectoryInfo(parameter.ToString());
        }
    }
}