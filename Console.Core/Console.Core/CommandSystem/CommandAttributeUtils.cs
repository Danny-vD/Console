using System;
using System.Linq;
using System.Reflection;
using Console.Core.CommandSystem.Commands;
using Console.Core.ConverterSystem;
using Console.Core.ReflectionSystem;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.CommandSystem
{
    /// <summary>
    /// Utilities for adding commands marked with the CommandAttribute
    /// </summary>
    public static class CommandAttributeUtils
    {
        /// <summary>
        /// Call this to add all static commands from a type
        /// </summary>
        /// <typeparam name="T">Type containing the Commands.</typeparam>
        public static void AddCommands<T>()
        {
            Type t = typeof(T);
            AddCommands(t);
        }

        /// <summary>
        /// Adds all Static Commands contained in this type.
        /// </summary>
        /// <param name="type">Type Containing the Commands.</param>
        public static void AddCommands(Type type)
        {
            ReflectionCommand[] cmds = GetStaticCommandData(type).Select(x => new ReflectionCommand(x)).ToArray();

            foreach (ReflectionCommand refCommand in cmds)
            {
                CommandManager.AddCommand(refCommand);
            }
        }

        /// <summary>
        /// Call this to add all commands from an instance
        /// </summary>
        /// <param name="instance">The Instance containing the Commands.</param>
        public static void AddCommands(object instance)
        {
            Type t = instance.GetType();
            ReflectionCommand[] cmds = GetCommandData(t, instance).Select(x => new ReflectionCommand(x)).ToArray();

            foreach (ReflectionCommand refCommand in cmds)
            {
                CommandManager.AddCommand(refCommand);
            }
        }

        /// <summary>
        /// Helper function.
        /// Returns the Methods that satisfy the binding flags and have at least one command attribute
        /// </summary>
        /// <param name="t">Type containing the Commands.</param>
        /// <param name="flags">Binding Flags for the Reflection Queries</param>
        /// <returns>Array of Method Infos eligible to be used as commands.</returns>
        private static MethodInfo[] GetCommands(Type t, BindingFlags flags)
        {
            MethodInfo[] i = t.GetMethods(flags).Where(x => x.GetCustomAttributes<CommandAttribute>().Count() != 0)
                .ToArray();

            //Debug.Log("Flags: " + flags + "\nMethods: " + i.Length);
            return i;
        }

        /// <summary>
        /// Get all Commands from a type(only works for static functions)
        /// </summary>
        /// <param name="t">Type containing the Commands</param>
        /// <returns>Array of IInvokable Instances</returns>
        private static IInvokable[] GetStaticCommandData(Type t)
        {
            IInvokable[] i =
                GetCommands(t, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Select(x => new StaticMethodMetaData(x)).ToArray();
            return i;
        }

        /// <summary>
        /// Get all commands from a type/instance(only works for non static functions)
        /// </summary>
        /// <param name="t">Type containing the Commands</param>
        /// <param name="instance">The Instance of the Type containing the Commands.</param>
        /// <returns>Array of IInvokable Instances</returns>
        private static IInvokable[] GetCommandData(Type t, object instance)
        {
            IInvokable[] i =
                GetCommands(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Select(x => new MethodMetaData(instance, x)).ToArray();
            return i;
        }

        /// <summary>
        /// Converts the Parameter to the Target Type
        /// </summary>
        /// <param name="parameter">Parameter to Convert</param>
        /// <param name="target">Target Type</param>
        /// <returns>Returns the Changed Parameter</returns>
        public static object ConvertToNonGeneric(object parameter, Type target)
        {
            if (target.IsInstanceOfType(parameter))
                return parameter;

            try
            {
                if (CustomConvertManager.CanConvert(parameter, target))
                    return CustomConvertManager.Convert(parameter, target);

                return Convert.ChangeType(parameter, target);
            }
            catch (Exception e)
            {
                ConsoleCoreConfig.CoreLogger.LogError($"Can not cast value: \"{parameter}\"  to type: {target.Name}");
                if (target.IsValueType)
                    return null; //Return Default value if the target is int/float/...
                throw e;
            }
        }
    }
}