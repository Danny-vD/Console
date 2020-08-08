﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Console.Core.Utils.Reflection;
using Console.Core.Utils.Reflection.Fields;
using Console.Core.Utils.Reflection.Properties;

namespace Console.Core.Utils
{
    public static class ReflectionUtils
    {
        public static object InvokePreserveStack(this MethodInfo info, object instance, object[] parameters)
        {
            try
            {
                return info.Invoke(instance, parameters);
            }
            catch (Exception e)
            {
                Exception actual = e is TargetInvocationException ? e.InnerException : e;
                if (actual == null) throw new Exception("The exception is null and can not be thrown.");
                ExceptionDispatchInfo.Capture(actual).Throw(); //Throw the Exception but do not change the stacktrace
                return null;
            }
        }

        public static object Get(this PropertyInfo info, object instance) =>
            InvokePreserveStack(info.GetMethod, instance, null);

        public static void Set(this PropertyInfo info, object instance, object value) =>
            InvokePreserveStack(info.SetMethod, instance, new[] {value});


        #region Reflection Helper Functions

        public static Dictionary<T, ReflectionHelper> GetStaticConsoleProps<T>(Type t) where T : Attribute
        {
            return t.GetPropertiesWithAttribute<T>(BindingFlags.Static).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new StaticPropertyHelper(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }
        public static Dictionary<T, ReflectionHelper> GetStaticConsoleFields<T>(Type t) where T : Attribute
        {
            return t.GetFieldsWithAttribute<T>(BindingFlags.Static).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new StaticFieldHelper(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<T, ReflectionHelper> GetConsoleProps<T>(object instance) where T : Attribute
        {
            return instance.GetType().GetPropertiesWithAttribute<T>(BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new PropertyHelper(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }
        public static Dictionary<T, ReflectionHelper> GetConsoleFields<T>(object instance) where T : Attribute
        {
            return instance.GetType().GetFieldsWithAttribute<T>(BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new FieldHelper(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }


        #endregion


        #region Attribute Inspection

        public static Dictionary<T, FieldInfo> GetFieldsWithAttribute<T>(this Type t, BindingFlags flag)
            where T : Attribute
        {
            return t.GetFields(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0)
                .ToDictionary(x => x.GetCustomAttribute<T>(), x => x);
        }

        public static Dictionary<T, PropertyInfo> GetPropertiesWithAttribute<T>(this Type t, BindingFlags flag) where T : Attribute
        {
            return t.GetProperties(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0)
                .ToDictionary(x => x.GetCustomAttribute<T>(), x => x);
        }



        #endregion
    }

}