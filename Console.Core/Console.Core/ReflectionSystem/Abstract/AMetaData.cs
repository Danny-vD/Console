using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem.Abstract
{
    /// <summary>
    /// Contains Meta Data from a Specific C# Member
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AMetaData<T>
        where T : MemberInfo
    {
        /// <summary>
        /// Attributes of the Member
        /// </summary>
        public List<Attribute> Attributes { get; }
        /// <summary>
        /// The Inner Member Info Instance
        /// </summary>
        public readonly T ReflectedInfo;

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="member">Member Info used as Backend</param>
        protected AMetaData(T member)
        {
            ReflectedInfo = member;
            Attributes = member.GetCustomAttributes(true).OfType<Attribute>().ToList();
        }
    }
}