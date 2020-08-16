using System;
using System.Collections.Generic;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    public class FakeValueContainer : IValueTypeContainer
    {
        public bool CanRead => true;
        public bool CanWrite => true;
        public Type ValueType => Value.GetType();

        public List<Attribute> Attributes { get; }

        public object Value;

        public FakeValueContainer(object value)
        {
            Value = value;
        }

        public object Get()
        {
            return Value;
        }

        public void Set(object value)
        {
            Value = value;
        }

    }
}