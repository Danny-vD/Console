using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// IAttributeCollection implementation. Using a Parameter Info Instance as backend.
    /// </summary>
    public class ParameterMetaData : IAttributeCollection
    {
        /// <summary>
        /// List of attributes on this parameter
        /// </summary>
        public List<Attribute> Attributes => ReflectedInfo.GetCustomAttributes(true).OfType<Attribute>().ToList();
        /// <summary>
        /// The Parameter Info
        /// </summary>
        public readonly ParameterInfo ReflectedInfo;
        /// <summary>
        /// The Parameter Type
        /// </summary>
        public Type ParameterType => ReflectedInfo.ParameterType;

        /// <summary>
        /// Public Parameter Info
        /// </summary>
        /// <param name="info">Parameter Info used as Backend</param>
        public ParameterMetaData(ParameterInfo info)
        {
            ReflectedInfo = info;
        }
    }
}