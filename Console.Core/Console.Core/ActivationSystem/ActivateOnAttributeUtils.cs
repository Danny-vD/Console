using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Console.Core.ActivationSystem
{
    /// <summary>
    /// Utility Class. Allows Activation of Types inside an Assembly
    /// </summary>
    public static class ActivateOnAttributeUtils
    {
        /// <summary>
        /// Activates all Types in this assembly that are or inherit from the base type.
        /// </summary>
        /// <param name="asm">Target Assembly</param>
        /// <param name="baseType">The Type that should be activated</param>
        /// <param name="parameter">The Constructor Parameters(if any)</param>
        /// <returns>Array of Activated Objects</returns>
        public static object[] ActivateObjects(Assembly asm, Type baseType, params object[] parameter)
        {
            List<object> ret = new List<object>();
            foreach (Type type in asm.GetTypes())
            {
                if (!baseType.IsAssignableFrom(type) && !type.GetInterfaces().Contains(baseType))
                {
                    continue;
                }
                ActivateOnAttribute ao = type.GetCustomAttribute<ActivateOnAttribute>();
                Type[] pTypes = parameter.Select(x => x.GetType()).ToArray();
                if (ao != null && !type.IsAbstract && type.GetConstructor(pTypes) != null)
                {
                    ConstructorInfo ci = type.GetConstructor(pTypes);
                    ret.Add(ci.Invoke(parameter));
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// Activates all Types in this assembly that are or inherit from the base type.
        /// </summary>
        /// <typeparam name="T">The Type of objects to activate</typeparam>
        /// <param name="asm">The Target Assembly</param>
        /// <param name="parameters">The Constructor parameters(if any)</param>
        /// <returns>Array of Activated Objects</returns>
        public static T[] ActivateObjects<T>(Assembly asm, params object[] parameters)
        {
            return ActivateObjects(asm, typeof(T), parameters).Cast<T>().ToArray();
        }
    }
}