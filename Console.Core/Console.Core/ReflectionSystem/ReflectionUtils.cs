using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Console.Core.ReflectionSystem.Interfaces;


/// <summary>
/// The Console.Core.ReflectionSystem namespace contains the abstractions and helper classes to interface with the C# Reflection System.
/// </summary>
namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// Utility Class used for Reflection
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Invokes a Method Info without breaking the stack when the Invocation Fais.
        /// </summary>
        /// <param name="info">The Info To Invoke</param>
        /// <param name="instance">The Instance</param>
        /// <param name="parameters">Parameters of the Invocation</param>
        /// <returns>Return of the Invocation</returns>
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
                    ExceptionDispatchInfo.Capture(actual)
                        .Throw(); //Throw the Exception but do not change the stacktrace
                }
                catch (Exception exception)
                {
                    if (ConsoleCoreConfig.LogExceptionMessageOnly)
                        ConsoleCoreConfig.CoreLogger.LogWarning(exception.Message);
                    else
                        ConsoleCoreConfig.CoreLogger.LogWarning(exception);

                    //throw actual;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the Value of the Property.
        /// </summary>
        /// <param name="info">The Info Containing the Value</param>
        /// <param name="instance">The Instance of the Property</param>
        /// <returns>The Value of the Property</returns>
        public static object Get(this PropertyInfo info, object instance) =>
            InvokePreserveStack(info.GetMethod, instance, null);

        /// <summary>
        /// Sets the Value of the Property.
        /// </summary>
        /// <param name="info">The Info Containing the Value</param>
        /// <param name="instance">The Instance of the Property</param>
        /// <param name="value">New Value</param>
        public static void Set(this PropertyInfo info, object instance, object value) =>
            InvokePreserveStack(info.SetMethod, instance, new[] {value});


        #region Reflection Helper Functions

        /// <summary>
        /// Returns all Static Property Meta Datas with the specified Attribute
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="t">Type Containing the Properties</param>
        /// <returns>Dictionary of Attributes and IValueTypeContainer Implementations</returns>
        public static Dictionary<T, IValueTypeContainer> GetStaticConsoleProps<T>(Type t) where T : Attribute
        {
            return t.GetPropertiesWithAttribute<T>(BindingFlags.Static).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new StaticPropertyMetaData(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Returns all Static FieldMetaDatas with the specified Attribute
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="t">Type Containing the Fields</param>
        /// <returns>Dictionary of Attributes and IValueTypeContainer Implementations</returns>
        public static Dictionary<T, IValueTypeContainer> GetStaticConsoleFields<T>(Type t) where T : Attribute
        {
            return t.GetFieldsWithAttribute<T>(BindingFlags.Static).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new StaticFieldMetaData(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Returns all Property Meta Datas with the specified Attribute
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="instance">Instance containing the Properties</param>
        /// <returns>Dictionary of Attributes and IValueTypeContainer Implementations</returns>
        public static Dictionary<T, IValueTypeContainer> GetConsoleProps<T>(object instance) where T : Attribute
        {
            return instance.GetType().GetPropertiesWithAttribute<T>(BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new PropertyMetaData(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Returns all FieldMetaDatas with the specified Attribute
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="instance">Instance containing the Fields</param>
        /// <returns>Dictionary of Attributes and IValueTypeContainer Implementations</returns>
        public static Dictionary<T, IValueTypeContainer> GetConsoleFields<T>(object instance) where T : Attribute
        {
            return instance.GetType().GetFieldsWithAttribute<T>(BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, IValueTypeContainer>(x.Key,
                    new FieldMetaData(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion


        #region Attribute Inspection

        /// <summary>
        /// Returns all Fields with the Specified Attribute
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="t">Type Containing the Fields</param>
        /// <param name="flag">Binding Flags of the Fields</param>
        /// <returns>Dictionary of Attributes and Field Infos</returns>
        public static Dictionary<T, FieldInfo> GetFieldsWithAttribute<T>(this Type t, BindingFlags flag)
            where T : Attribute
        {
            return GetAttributes<T, FieldInfo>(t.GetFields(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0).Cast<MemberInfo>().ToList());
        }

        /// <summary>
        /// Returns all Properties with the Specified Attribute
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="t">Type Containing the Fields</param>
        /// <param name="flag">Binding Flags of the Properties</param>
        /// <returns>Dictionary of Attributes and Property Infos</returns>
        public static Dictionary<T, PropertyInfo> GetPropertiesWithAttribute<T>(this Type t, BindingFlags flag)
            where T : Attribute
        {
            return GetAttributes<T, PropertyInfo>(t.GetProperties(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0).Cast<MemberInfo>().ToList());
        }


        /// <summary>
        /// Returns all Member Infos of a specified type and with a specified attribute type.
        /// </summary>
        /// <typeparam name="K">Attribute Type</typeparam>
        /// <typeparam name="V">Member Info Type</typeparam>
        /// <param name="info">List of Member Info to Search</param>
        /// <returns>Dictionary of Attributes and Member Infos</returns>
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