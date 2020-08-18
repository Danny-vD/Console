using System;
using Console.Core.ConverterSystem;
using UnityEngine;

namespace Console.Unity
{

    /// <summary>
    /// AConverter Implementation that Converts a Gameobject into a component
    /// </summary>
    public class GameObjectComponentConverter : AConverter
    {

        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public override bool CanConvert(object parameter, Type target)
        {
            return typeof(Component).IsAssignableFrom(target) && parameter is GameObject;
        }

        /// <summary>
        /// Converts the Parameter into the Target Type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public override object Convert(object parameter, Type target)
        {
            GameObject obj = parameter as GameObject;

            return obj.GetComponent(target);
        }
    } 
}
