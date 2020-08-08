using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.ConverterSystem;

namespace Console.ArrayConverter
{

    public class ArrayCustomConverter : AConverter
    {
        public override bool CanConvert(object parameter, Type target)
        {
            return (parameter is Array) &&
                   (target.IsArray || (typeof(IList).IsAssignableFrom(target) && target.IsGenericType) ||
                    target.IsAssignableFrom(parameter.GetType().GetElementType()));
        }

        //Used as reflection
        private static List<T> GetAsList<T>(Array arr)
        {
            List<T> list = new List<T>();

            foreach (object item in arr)
            {
                list.Add((T)item);
            }
            return list;
        }

        public override object Convert(object parameter, Type target)
        {
            Array paramArr = parameter as Array;
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

                MethodInfo info = typeof(ArrayCustomConverter)
                    .GetMethod(nameof(GetAsList), BindingFlags.NonPublic | BindingFlags.Static) //Get method info
                    ?.MakeGenericMethod(target.GetGenericArguments().First()); //Create Generic Method Call with the generic type of the target
                object r = info.Invoke(null, new object[] { paramArr }); //Invoke the GetAsList<T> method.
                return r;
            }
            if (target.IsAssignableFrom(parameter.GetType().GetElementType()))
            {
                return paramArr == null || paramArr.Length == 0 ? null : paramArr.GetValue(0);
            }
            //Error
            return null;
        }
    }
}
