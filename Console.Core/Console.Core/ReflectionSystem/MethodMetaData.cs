using System;
using System.Linq;
using System.Reflection;
using Console.Core.ReflectionSystem.Abstract;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// IInvokable Implementation. Using a MethodInfo as backend
    /// </summary>
    public class MethodMetaData : AInstancedMetaData<MethodInfo>, IInvokable
    {
        /// <summary>
        /// Name of the Method
        /// </summary>
        public string Name => ReflectedInfo.Name;
        /// <summary>
        /// The Parameter Count of the Method
        /// </summary>
        public int ParameterCount => ParameterTypes.Length;
        /// <summary>
        /// The Meta Data of the Commands.
        /// </summary>
        public ParameterMetaData[] ParameterTypes { get; }
        /// <summary>
        /// The Return type of the Invocation
        /// </summary>
        public Type ReturnType => ReflectedInfo.ReturnType;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="instance">Instance Bound to the method info</param>
        /// <param name="info">Method info used as Backend</param>
        public MethodMetaData(object instance, MethodInfo info) : base(instance, info)
        {
            ParameterTypes = info.GetParameters().Select(x => new ParameterMetaData(x)).ToArray();
        }

        /// <summary>
        /// Invokes the Method Info
        /// </summary>
        /// <param name="parameters">Parameters of the Invocation</param>
        /// <returns>The Return of the Invocation</returns>
        public object Invoke(params object[] parameters)
        {
            return ReflectedInfo.Invoke(Instance, parameters.Length == 0 ? null : parameters);
        }
    }
}