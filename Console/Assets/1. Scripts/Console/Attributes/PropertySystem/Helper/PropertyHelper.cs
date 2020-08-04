using System.Reflection;
using Console.Attributes.CommandSystem.Helper;

namespace Console.Attributes.PropertySystem.Helper
{
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
}