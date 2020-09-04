using System;

namespace Console.Core
{
    /// <summary>
    /// Implements Eye Friendly ToString Method
    /// </summary>
    public abstract class ConsoleAttribute : Attribute
    {
        /// <summary>
        /// Returns the Name of the Attribute
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}