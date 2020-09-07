using System;
using System.Collections.Generic;

namespace Console.Core.ReflectionSystem.Interfaces
{
    /// <summary>
    /// Inherited by Classes that contain a Collection of Attributes.
    /// </summary>
    public interface IAttributeCollection
    {

        /// <summary>
        /// Collection of Attributes
        /// </summary>
        List<Attribute> Attributes { get; }

    }
}