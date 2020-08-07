using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Console.Core
{
    public static class ReflectionHelper
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

    }
}