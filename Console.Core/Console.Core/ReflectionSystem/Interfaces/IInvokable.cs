using System;

namespace Console.Core.ReflectionSystem.Interfaces
{
    public interface IInvokable : IAttributeData
    {
        string Name { get; }
        int ParameterCount { get; }
        ParameterMetaData[] ParameterTypes { get; }
        Type ReturnType { get; }
        object Invoke(params object[] parameters);
    }
}