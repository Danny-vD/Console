using System.Reflection;

namespace Console.Core.ReflectionSystem.Interfaces
{
    public interface IMetaData : IAttributeData
    {
        MemberInfo GetMemberInfo();
    }
}