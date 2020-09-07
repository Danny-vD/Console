using System;

namespace Console.Core.ReflectionSystem.Interfaces
{
    /// <summary>
    /// Inherited by Classes that can be invoked
    /// </summary>
    public interface IInvokable : IAttributeCollection
    {

        /// <summary>
        /// Name of the Invokable Instance
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The Parameter Count of the Instance
        /// </summary>
        int ParameterCount { get; }

        /// <summary>
        /// The Meta Data of the Parameters
        /// </summary>
        ParameterMetaData[] ParameterTypes { get; }

        /// <summary>
        /// The Return type of the Invokable Instance
        /// </summary>
        Type ReturnType { get; }

        /// <summary>
        /// Invokes this Instance.
        /// </summary>
        /// <param name="parameters">Parameters of the Invocation</param>
        /// <returns>The return of this Invocation</returns>
        object Invoke(params object[] parameters);

    }
}