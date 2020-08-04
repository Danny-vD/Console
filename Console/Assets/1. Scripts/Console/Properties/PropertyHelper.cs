using System.Reflection;
using Console.Attributes;

namespace Console.Properties
{
    internal abstract class ReflectionHelper
    {
        internal abstract object GetValue();
        internal abstract void SetValue(object value);
    }

    internal class PropertyHelper : ReflectionHelper
    {
        internal PropertyInfo Info { get; }
        internal object Instance { get; }

        internal PropertyHelper(object instance, PropertyInfo info)
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
            object convertedValue = CommandAttributeUtils.ConvertToNonGeneric(value, Info.PropertyType);

            Info.SetValue(Instance, convertedValue);
        }
    }

    internal class StaticFieldHelper : FieldHelper
    {
        internal StaticFieldHelper(FieldInfo info) :base(null, info){ }
    }

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
            object convertedValue = CommandAttributeUtils.ConvertToNonGeneric(value, Info.FieldType);

            Info.SetValue(Instance, convertedValue);
        }
    }
}