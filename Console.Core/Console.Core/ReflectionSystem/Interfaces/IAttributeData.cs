using System;
using System.Collections.Generic;

namespace Console.Core.ReflectionSystem.Interfaces
{
    public interface IAttributeData
    {
        List<Attribute> Attributes { get; }
    }
}