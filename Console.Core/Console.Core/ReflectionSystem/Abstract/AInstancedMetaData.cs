using System.Reflection;

namespace Console.Core.ReflectionSystem.Abstract
{
    /// <summary>
    /// Contains Meta Data from a Specific Instanced C# Member
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AInstancedMetaData<T> : AMetaData<T>
        where T : MemberInfo
    {
        /// <summary>
        /// The Object Instance that contains the Member
        /// </summary>
        public readonly object Instance;

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="instance">Instance bound to the Member</param>
        /// <param name="member">Member used as Backend</param>
        protected AInstancedMetaData(object instance, T member) : base(member)
        {
            Instance = instance;
        }
    }
}