using System;

namespace Console.Core.ReflectionSystem.Interfaces
{
    /// <summary>
    /// Inherited by Classes that represent a Settable/Gettable Container for a Value.
    /// </summary>
    public interface IValueTypeContainer : IAttributeCollection, ISettable, IGettable
    {
        /// <summary>
        /// The Type of the Value
        /// </summary>
        Type ValueType { get; }
        /// <summary>
        /// Flag that determines if the Value can be Read
        /// </summary>
        bool CanRead { get; }
        /// <summary>
        /// Flag that determines if the Value can be written.
        /// </summary>
        bool CanWrite { get; }
    }
}