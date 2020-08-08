using System;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.Utils;
using Console.Core.Utils.Reflection.Properties;

namespace Console.UtilExtension
{
    public class ConsoleUtilCommands
    {
        [Command("add-properties", "Adds all static Console Properties of the specified Type")]
        private static void AddProperties(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            ConsolePropertyAttributeUtils.AddPropertiesByType(t);
        }

        [Command("add-properties-any", "Adds all static public Properties of the specified Type with the specified prefix")]
        private static void AddPropertiesAny(string qualifiedName)
        {
            AddPropertiesAny(qualifiedName, "");
        }

        [Command("add-properties-any", "Adds all static public Properties of the specified Type with the specified prefix")]
        private static void AddPropertiesAny(string qualifiedName, string prefix)
        {
            Type t = Type.GetType(qualifiedName);

            AddAnyPropertiesByType(prefix, t);
        }
        private static bool ValidType(Type t) => t.IsPrimitive || t == typeof(string);


        private static void AddAnyPropertiesByType(string prefix, Type t)
        {
            PropertyInfo[] infos = t.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(x => ValidType(x.PropertyType)).ToArray();
            foreach (PropertyInfo propertyInfo in infos)
            {
                ConsolePropertyAttributeUtils.SetProperty(prefix + propertyInfo.Name, new StaticPropertyHelper(propertyInfo));
            }
        }


        [Command("add-commands", "Adds all static Console Commands of the specified Type")]
        private static void AddCommands(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            CommandAttributeUtils.AddCommands(t);
        }
    }
}