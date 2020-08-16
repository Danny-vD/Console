using System;

namespace Console.Core.ReflectionSystem.Interfaces
{
    public interface IValueTypeContainer : IAttributeData, ISettable, IGettable
    {
        Type ValueType { get; }
        bool CanRead { get; }
        bool CanWrite { get; }
    }
}