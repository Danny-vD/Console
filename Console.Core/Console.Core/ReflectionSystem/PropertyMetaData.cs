using System;
using System.Reflection;
using Console.Core.ReflectionSystem.Abstract;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    public class PropertyMetaData : AInstancedMetaData<PropertyInfo>, IValueTypeContainer
    {
        public PropertyMetaData(object instance, PropertyInfo info) : base(instance, info) { }

        public Type ValueType => ReflectedInfo.PropertyType;
        public bool CanWrite => ReflectedInfo.CanWrite;
        public bool CanRead => ReflectedInfo.CanRead;

        public void Set(object value)
        {
            if (ValueType.IsInstanceOfType(value) || value == null)
            {
                ReflectedInfo.SetValue(Instance, value);
            }
        }

        public object Get() => ReflectedInfo.GetValue(Instance);

        public T Get<T>()
        {
            return (T)Get();
        }
    }
}