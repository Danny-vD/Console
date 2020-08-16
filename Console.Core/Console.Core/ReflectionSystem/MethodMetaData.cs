using System;
using System.Linq;
using System.Reflection;
using Console.Core.ReflectionSystem.Abstract;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    public class MethodMetaData : AInstancedMetaData<MethodInfo>, IInvokable
    {
        public string Name => ReflectedInfo.Name;
        public int ParameterCount => ParameterTypes.Length;
        public ParameterMetaData[] ParameterTypes { get; }
        public Type ReturnType => ReflectedInfo.ReturnType;

        public MethodMetaData(object instance, MethodInfo info) : base(instance, info)
        {
            ParameterTypes = info.GetParameters().Select(x => new ParameterMetaData(x)).ToArray();
        }

        public object Invoke(params object[] parameters)
        {
            return ReflectedInfo.Invoke(Instance, parameters.Length == 0 ? null : parameters);
        }
    }
}