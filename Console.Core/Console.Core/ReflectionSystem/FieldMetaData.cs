using System;
using System.Reflection;
using Console.Core.ReflectionSystem.Abstract;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    public class FieldMetaData : AInstancedMetaData<FieldInfo>, IValueTypeContainer
    {
        public bool CanWrite => !ReflectedInfo.IsInitOnly;
        public bool CanRead => true;
        public Type ValueType => ReflectedInfo.FieldType;

        public FieldMetaData(object instance, FieldInfo info) : base(instance, info)
        {

        }

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