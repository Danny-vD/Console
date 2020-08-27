using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ConverterSystem;
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
        /// Gets called by the Console System
        /// </summary>
        /// <param name="parameter">Invocation Parameters</param>
        /// <returns>Invocation Return</returns>
        public object Invoke(object[] parameter)
        {
            return InvokeDirect(Cast(parameter));
        }

        /// <summary>
        /// Invokes the Method with the exact object array.
        /// Does not Convert the Types accordingly
        /// </summary>
        /// <param name="parameter">Invocation Parameters</param>
        /// <returns>Invocation Return</returns>
        public object InvokeDirect(object[] parameter)
        {
            return ReflectedInfo.InvokePreserveStack(Instance, parameter.Length == 0 ? null : parameter);
        }

        /// <summary>
        /// Fills the Right Indices of the Return array with flag values.
        /// </summary>
        /// <param name="pt">All Parameters</param>
        /// <param name="cparameter">The Current Parameters</param>
        /// <param name="ret">The Result Array</param>
        private void ApplyFlagsSyntax(ParameterInfo[] pt, List<object> cparameter, object[] ret)
        {
            for (int i = 0; i < pt.Length; i++)
            {
                CommandFlagAttribute cfa = pt[i].GetCustomAttribute<CommandFlagAttribute>();
                if (cfa != null)
                {
                    string name = cfa.Name ?? pt[i].Name;
                    bool lval = cparameter.Any(x =>
                        x.ToString().StartsWith(ConsoleCoreConfig.CommandFlagPrefix) &&
                        x.ToString().Remove(0, 2) == name);
                    if (lval) cparameter.Remove(ConsoleCoreConfig.CommandFlagPrefix + name);
                    ret[i] = lval;
                }
            }
        }

        /// <summary>
        /// Casts the Parameters to the right types for this Method.
        /// </summary>
        /// <param name="parameter">Parameter Array</param>
        /// <returns>Parameter Array with correct types.</returns>
        private object[] Cast(object[] parameter)
        {
            ParameterInfo[] pt = ReflectedInfo.GetParameters();
            List<object> cparameter = new List<object>(parameter);

            object[] ret = new object[pt.Length];
            int off = 0;

            ApplyFlagsSyntax(pt, cparameter, ret);

            for (int i = 0; i < pt.Length; i++)
            {
                SelectionPropertyAttribute sp = pt[i].GetCustomAttribute<SelectionPropertyAttribute>();
                if (sp != null && !(AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0 &&
                                    pt.Length == cparameter.Count))
                {

                    object v = AConsoleManager.Instance.ObjectSelector.SelectedObjects.ToArray();
                    if (AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0)
                    {
                        if (pt.Length == cparameter.Count)
                        {
                            ret[i] = CommandAttributeUtils.ConvertToNonGeneric(cparameter[i - off],
                                pt[i].ParameterType);
                            continue;
                        }
                        else
                        {
                            v = null;
                        }
                    }
                    else if (!sp.NoConverter && CustomConvertManager.CanConvert(v, pt[i].ParameterType))
                    {
                        v = CustomConvertManager.Convert(
                            v, pt[i].ParameterType);
                    }

                    ret[i] = v;

                    off++;
                    continue;
                }

                CommandFlagAttribute cfa = pt[i].GetCustomAttribute<CommandFlagAttribute>();
                if (cfa != null)
                {
                    if (ret[i] is bool b && !b && cparameter.Count > i - off)
                    {
                        ret[i] = cparameter[i - off].ToString().ToLower() == "true";
                    }
                    continue;
                }

                Type type = pt[i].ParameterType;
                ret[i] = CommandAttributeUtils.ConvertToNonGeneric(cparameter[i - off], type);
            }

            return ret;
        }
    }
}