using System;
using System.Collections;
using System.Reflection;
using Console.Attributes.CommandSystem.Helper;

namespace Console.Attributes.PropertySystem.Helper
{
    internal class FieldHelper : ReflectionHelper
    {
        internal FieldInfo Info { get; }
        internal object Instance { get; }

        internal FieldHelper(object instance, FieldInfo info)
        {
            Instance = instance;
            Info = info;
        }

        internal override object GetValue()
        {
            return Info.GetValue(Instance);
        }

        internal override void SetValue(object value)
        {
            object convertedValue = null;
            if (value is Array ar && !typeof(IEnumerable).IsAssignableFrom(Info.FieldType))
            {
                convertedValue = ar.GetValue(0);
            }
            else
            {
                convertedValue = CommandAttributeUtils.ConvertToNonGeneric(value, Info.FieldType);
            }

            Info.SetValue(Instance, convertedValue);
        }
    }
}