using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.CommandSystem.Helper;

namespace Console.Core.Commands.ConverterSystem
{
    public class ArrayConverter : AConverter
    {
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is Array &&
                   (target.IsArray || (typeof(IList).IsAssignableFrom(target) && target.IsGenericType));
        }

        //Used as reflection
        private static List<T> GetAsList<T>(Array arr)
        {
            List<T> list = new List<T>();

            foreach (object item in arr)
            {
                list.Add((T)System.Convert.ChangeType(item, typeof(T)));
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

                MethodInfo info = typeof(CommandAttributeUtils)
                    .GetMethod(nameof(GetAsList), BindingFlags.NonPublic | BindingFlags.Static) //Get method info
                    ?.MakeGenericMethod(target.GetGenericArguments().First()); //Create Generic Method Call with the generic type of the target
                return info.Invoke(null, new object[] { paramArr }); //Invoke the GetAsList<T> method.
            }
            //Error
            return null;
        }
    }
}