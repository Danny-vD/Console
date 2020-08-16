using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Console.Core.ActivationSystem
{
    public static class ActivateOnAttributeUtils
    {
        public static object[] ActivateObjects(Assembly asm, Type baseType, params object[] parameter)
        {
            List<object> ret = new List<object>();
            foreach (Type type in asm.GetTypes())
            {
                if (!baseType.IsAssignableFrom(type) && !type.GetInterfaces().Contains(baseType))
                    continue;
                ActivateOnAttribute ao = type.GetCustomAttribute<ActivateOnAttribute>();
                Type[] pTypes = parameter.Select(x => x.GetType()).ToArray();
                if (ao != null && !type.IsAbstract && type.GetConstructor(pTypes) != null)
                {
                    ConstructorInfo ci = type.GetConstructor(pTypes);
                    ret.Add(ci.Invoke(parameter));
                }
            }
            return ret.ToArray();
        }

        public static T[] ActivateObjects<T>(Assembly asm, params object[] parameters)
        {
            return ActivateObjects(asm, typeof(T), parameters).Cast<T>().ToArray();
        }
    }
}