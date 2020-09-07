using System;
using System.Linq;
using System.Reflection;

using Console.Core.CommandSystem.Attributes;
using Console.Core.PropertySystem;
using Console.Core.ReflectionSystem;

namespace Console.Utility.Commands.Properties
{
    /// <summary>
    /// Utility Commands for the Property System
    /// </summary>
    public class UtilPropertyCommands
    {

        private const string PROPERTY_NAMESPACE = UtilExtensionInitializer.UTIL_NAMESPACE + "::properties";

        /// <summary>
        /// Adds all Static Properties of the Specified Type
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Type Name</param>
        [Command(
            "add-properties",
            Namespace = PROPERTY_NAMESPACE,
            HelpMessage = "Adds all static Console Properties of the specified Type"
        )]
        private static void AddProperties(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            PropertyAttributeUtils.AddProperties(t);
        }

        /// <summary>
        /// Adds all static public properties of the specified type.
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Type Name</param>
        [Command(
            "add-properties-any",
            Namespace = PROPERTY_NAMESPACE,
            HelpMessage = "Adds all static public Properties of the specified Type with the specified prefix"
        )]
        private static void AddPropertiesAny(string qualifiedName)
        {
            AddPropertiesAny(qualifiedName, "");
        }

        /// <summary>
        /// Adds all Static public properties of the specified type
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Type Name</param>
        /// <param name="prefix">Prefix of the Command</param>
        [Command(
            "add-properties-any",
            Namespace = PROPERTY_NAMESPACE,
            HelpMessage = "Adds all static public Properties of the specified Type with the specified prefix"
        )]
        private static void AddPropertiesAny(string qualifiedName, string prefix)
        {
            Type t = Type.GetType(qualifiedName);

            AddAnyPropertiesByType(prefix, t);
        }

        private static bool ValidType(Type t)
        {
            return t.IsPrimitive || t == typeof(string);
        }

        /// <summary>
        /// Adds all Properties from the Specified type with the specified prefix
        /// </summary>
        /// <param name="prefix">Prefix of the Command</param>
        /// <param name="t">Type containing the commands</param>
        private static void AddAnyPropertiesByType(string prefix, Type t)
        {
            PropertyInfo[] infos = t.GetProperties(BindingFlags.Static | BindingFlags.Public)
                                    .Where(x => ValidType(x.PropertyType)).ToArray();
            foreach (PropertyInfo propertyInfo in infos)
            {
                PropertyManager.SetProperty(prefix + propertyInfo.Name, new StaticPropertyMetaData(propertyInfo));
            }
        }

    }
}