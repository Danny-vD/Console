using System;
using System.Collections;
using System.Reflection;
using Console.Core.Attributes.CommandSystem.Helper;

namespace Console.Core.Attributes.PropertySystem.Helper
{
    public class PropertyHelper : ReflectionHelper
    {
        internal PropertyInfo Info { get; }
        internal object Instance { get; }

        internal PropertyHelper(object instance, PropertyInfo info)
        {
            Instance = instance;
            Info = info;
        }
        public override object GetValue()
        {
            return Info.Get(Instance);
            //return Info.GetMethod.InvokePreserveStack(Instance, null);
        }

        public override void SetValue(object value)
        {
            object convertedValue = null;
            if (value is Array ar && !typeof(IEnumerable).IsAssignableFrom(Info.PropertyType))
            {
                convertedValue = ar.GetValue(0); //get the first item of the array
            }
            else
            {
                convertedValue = CommandAttributeUtils.ConvertToNonGeneric(value, Info.PropertyType);
            }

            Info.Set(Instance, convertedValue);
            //Info.SetValue(Instance, convertedValue);
        }
    }
}