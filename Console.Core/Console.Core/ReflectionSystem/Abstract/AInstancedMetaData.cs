using System.Reflection;

namespace Console.Core.ReflectionSystem.Abstract
{
    public abstract class AInstancedMetaData<T> : AMetaData<T>
        where T : MemberInfo
    {
        public readonly object Instance;
        protected AInstancedMetaData(object instance, T member) : base(member)
        {
            Instance = instance;
        }
    }
}