using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    public class ParameterMetaData : IAttributeData
    {
        public List<Attribute> Attributes => ReflectedInfo.GetCustomAttributes(true).OfType<Attribute>().ToList();
        public readonly ParameterInfo ReflectedInfo;
        public Type ParameterType => ReflectedInfo.ParameterType;

        public ParameterMetaData(ParameterInfo info)
        {
            ReflectedInfo = info;
        }
    }
}