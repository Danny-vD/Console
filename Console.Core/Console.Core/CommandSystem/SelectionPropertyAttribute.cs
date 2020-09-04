using System;

namespace Console.Core.CommandSystem
{
    /// <summary>
    /// Can be used to Specify that the Selected Objects should be passed to this parameter.
    /// This makes it impossible to specify the parameter in the command text.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SelectionPropertyAttribute : ConsoleAttribute
    {
        /// <summary>
        /// Should the Converter System Change the types of the selected objects to match the command?
        /// </summary>
        public readonly bool NoConverter;

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="noConverter">Flag that can be set to opt out of the automatic conversion system.</param>
        public SelectionPropertyAttribute(bool noConverter = false)
        {
            NoConverter = noConverter;
        }

        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns>String Representation</returns>
        public override string ToString()
        {
            return $"SelectionProperty{(NoConverter ? "" : "(NoConverter)")}";
        }
    }
}