using System;
using Console.Core.ActivationSystem;

namespace Console.Core.ConverterSystem
{
    /// <summary>
    /// Converter Class is used to Implement Custom Conversion Logic
    /// </summary>
    [ActivateOn]
    public abstract class AConverter
    {
        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public abstract bool CanConvert(object parameter, Type target);
        /// <summary>
        /// Converts the Parameter into the Target Type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public abstract object Convert(object parameter, Type target);
    }
}