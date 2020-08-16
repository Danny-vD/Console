using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
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
                try
                {
                    ExceptionDispatchInfo.Capture(actual).Throw(); //Throw the Exception but do not change the stacktrace
                }
                catch (Exception exception)
                {
                    AConsoleManager.Instance.LogWarning(exception.Message);
                    //throw actual;
                }
                return null;
            }
        }

        public static object Get(this PropertyInfo info, object instance) =>
            InvokePreserveStack(info.GetMethod, instance, null);

        public static void Set(this PropertyInfo info, object instance, object value) =>
            InvokePreserveStack(info.SetMethod, instance, new[] { value });


        #region Reflection Helper Functions

        public static Dictionary<T, IValueTypeContainer> GetStaticConsoleProps<T>(Type t) where T : Attribute
        {
            return t.GetPropertiesWithAttribute<T>(BindingFlags.Static).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new StaticPropertyMetaData(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }
        public static Dictionary<T, IValueTypeContainer> GetStaticConsoleFields<T>(Type t) where T : Attribute
        {
            return t.GetFieldsWithAttribute<T>(BindingFlags.Static).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new StaticFieldMetaData(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<T, IValueTypeContainer> GetConsoleProps<T>(object instance) where T : Attribute
        {
            return instance.GetType().GetPropertiesWithAttribute<T>(BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new PropertyMetaData(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }
        public static Dictionary<T, IValueTypeContainer> GetConsoleFields<T>(object instance) where T : Attribute
        {
            return instance.GetType().GetFieldsWithAttribute<T>(BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new FieldMetaData(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }


        #endregion


        #region Attribute Inspection

        public static Dictionary<T, FieldInfo> GetFieldsWithAttribute<T>(this Type t, BindingFlags flag)
            where T : Attribute
        {
            return GetAttributes<T, FieldInfo>( t.GetFields(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0).Cast<MemberInfo>().ToList());
        }

        public static Dictionary<T, PropertyInfo> GetPropertiesWithAttribute<T>(this Type t, BindingFlags flag) where T : Attribute
        {
            return GetAttributes<T, PropertyInfo>(t.GetProperties(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0).Cast<MemberInfo>().ToList());
        }
        

        private static Dictionary<K, V> GetAttributes<K, V>(List<MemberInfo> info)
            where K : Attribute
            where V : MemberInfo
        {
            Dictionary<K, V> ret = new Dictionary<K, V>();
            foreach (V propertyInfo in info)
            {
                K[] a = propertyInfo.GetCustomAttributes<K>().ToArray();
                foreach (K attribute in a)
                {
                    ret.Add(attribute, propertyInfo);
                }
            }
            return ret;
        }


        #endregion
    }

}