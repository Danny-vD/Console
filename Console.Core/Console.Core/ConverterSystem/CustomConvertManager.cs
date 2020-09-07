using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Console.Core.ConverterSystem namespace is providing the abstract AConverter class that can be inherited to create custom converters for the command input parameters
/// </summary>
namespace Console.Core.ConverterSystem
{
    /// <summary>
    /// Converter System API
    /// </summary>
    public static class CustomConvertManager
    {

        /// <summary>
        /// All Loaded Converters.
        /// </summary>
        private static readonly List<AConverter> Converters = new List<AConverter>();

        /// <summary>
        /// Adds a Converter to the List of Loaded Converters.
        /// </summary>
        /// <param name="converter">Converter to Add</param>
        public static void AddConverter(AConverter converter)
        {
            Converters.Add(converter);
        }

        /// <summary>
        /// Returns true when one of the Loaded Converters is able to provide a conversion
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public static bool CanConvert(object parameter, Type target)
        {
            return Converters.Any(x => x.CanConvert(parameter, target));
        }

        /// <summary>
        /// Returns the Converted Parameter
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public static object Convert(object parameter, Type target)
        {
            AConverter conv = Converters.FirstOrDefault(x => x.CanConvert(parameter, target));
            if (conv != null)
            {
                return conv.Convert(parameter, target);
            }

            return parameter;
        }


        /// <summary>
        /// Extension Method.
        /// Tries to Convert the Specified object into the Specified Parameter.
        /// </summary>
        /// <typeparam name="TNewType">Target Type</typeparam>
        /// <param name="object">Input Value</param>
        /// <returns>Value of type TNewType</returns>
        public static TNewType ConvertTo<TNewType>(this object @object)
        {
            try
            {
                return (TNewType) System.Convert.ChangeType(@object, typeof(TNewType));
            }
            catch
            {
                return (TNewType) @object;
            }
        }

    }
}