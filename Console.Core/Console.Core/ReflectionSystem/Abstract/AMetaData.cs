using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem.Abstract
{
    public abstract class AMetaData<T> : IMetaData
        where T : MemberInfo
    {
        public List<Attribute> Attributes { get; }
        public readonly T ReflectedInfo;

        protected AMetaData(T member)
        {
            ReflectedInfo = member;
            Attributes = member.GetCustomAttributes(true).OfType<Attribute>().ToList();
        }

        public MemberInfo GetMemberInfo() => ReflectedInfo;
    }
}