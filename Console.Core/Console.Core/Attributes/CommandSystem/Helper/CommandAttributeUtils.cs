using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.Commands;

namespace Console.Core.Attributes.CommandSystem.Helper
{
    public static class CommandAttributeUtils
    {

        /// <summary>
		/// Call this to add all static commands from a type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void AddCommands<T>()
        {
            Type t = typeof(T);
            ReflectionCommand[] cmds = GetStaticCommandData(t).Select(x => new ReflectionCommand(x)).ToArray();

            foreach (ReflectionCommand refCommand in cmds)
            {
                CommandManager.AddCommand(refCommand);
            }
        }

        /// <summary>
        /// Call this to add all commands from an instance
        /// </summary>
        /// <param name="instance"></param>
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
        /// <param name="t"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
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
        /// <param name="t"></param>
        /// <returns></returns>
        private static CommandReflectionData[] GetStaticCommandData(Type t)
        {
            CommandReflectionData[] i =
                GetCommands(t, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Select(x => new CommandReflectionData(null, x)).ToArray();
            return i;
        }

        /// <summary>
        /// Get all commands from a type/instance(only works for non static functions)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static CommandReflectionData[] GetCommandData(Type t, object instance)
        {
            CommandReflectionData[] i =
                GetCommands(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Select(x => new CommandReflectionData(instance, x)).ToArray();
            return i;
        }

        /// <summary>
        /// 1:1 implementation of AbstractCommand.ConvertTo but not generic
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static object ConvertToNonGeneric(object parameter, Type target)
        {
            if (target.IsInstanceOfType(parameter))
                return parameter;

            //TODO: Reimplement Component/Gameobject Conversion
            //if (typeof(Component).IsAssignableFrom(target))
            //{
            //    GameObject gameobject = (GameObject)parameter;

            //    if (gameobject.TryGetComponent(target, out Component component))
            //    {
            //        ConsoleManager.LogError($"{parameter} does not have component {target.Name}!");
            //    }

            //    return component;
            //}

            if (parameter is Array paramArr)
            {
                if (target.IsArray)
                {
                    Array result = (Array)Activator.CreateInstance(target, paramArr.Length);
                    //Array result = Array.CreateInstance(target.MakeArrayType(), paramArr.Length);
                    paramArr.CopyTo(result, 0);
                    return result;
                }
                if (typeof(IList).IsAssignableFrom(target) && target.IsGenericType)
                {
                    //return lst.ConvertAll(input => FUCK);

                    MethodInfo info = typeof(CommandAttributeUtils)
                        .GetMethod("GetAsList", BindingFlags.NonPublic|BindingFlags.Static) //Get method info
                        ?.MakeGenericMethod(target.GetGenericArguments().First()); //Create Generic Method Call with the generic type of the target
                    return info.Invoke(null, new object[] {paramArr}); //Invoke the GetAsList<T> method.
                }
            }

            return Convert.ChangeType(parameter, target);
        }

        //Used as reflection
        private static List<T> GetAsList<T>(Array arr)
        {
            List<T> list = new List<T>();

            foreach (object item in arr)
            {
                list.Add((T)Convert.ChangeType(item, typeof(T)));
            }
            return list;
        }
    }
}