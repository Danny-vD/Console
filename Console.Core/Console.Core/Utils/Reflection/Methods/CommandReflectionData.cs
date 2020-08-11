using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.ConverterSystem;

namespace Console.Core.Utils.Reflection.Methods
{
    /// <summary>
    /// Inner class that contains the reflected data
    /// </summary>
    public class CommandReflectionData
    {
        /// <summary>
        /// Instance is null when the Command Function is static
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="info"></param>
        public CommandReflectionData(object instance, MethodInfo info)
        {
            Instance = instance;
            Info = info;
            Attribute = info.GetCustomAttribute<CommandAttribute>();
        }

        public readonly object Instance;
        public readonly MethodInfo Info;

        /// <summary>
        /// Can be used to find name and alias
        /// </summary>
        public readonly CommandAttribute Attribute;

        /// <summary>
        /// Shortcut to get the types of the parameters
        /// </summary>
        public ParameterInfo[] AllowedParameterTypes => Info.GetParameters();
        public int SelectionAttributeCount =>
            AllowedParameterTypes.Count(x => x.GetCustomAttribute<SelectionPropertyAttribute>() != null);
        public int FlagAttributeCount => AllowedParameterTypes.Count(x => x.ParameterType == typeof(bool) && x.GetCustomAttribute<CommandFlagAttribute>() != null);

        /// <summary>
        /// Gets called by the Console System
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object Invoke(object[] parameter)
        {
            return InvokeDirect(parameter.Length == 0 ? null : Cast(parameter));
        }

        public object InvokeDirect(object[] parameter)
        {
            return Info.InvokePreserveStack(Instance, parameter);
        }

        private void ApplyFlagsSyntax(ParameterInfo[] pt, List<object> cparameter, object[] ret)
        {
            for (int i = 0; i < pt.Length; i++)
            {
                CommandFlagAttribute cfa = pt[i].GetCustomAttribute<CommandFlagAttribute>();
                if (cfa != null)
                {
                    string name = cfa.Name ?? pt[i].Name;
                    bool val = cparameter.Any(x => x.ToString().StartsWith("-") && x.ToString().Remove(0, 1) == name);
                    bool lval = cparameter.Any(x => x.ToString().StartsWith("--") && x.ToString().Remove(0, 2) == name);
                    if (val) cparameter.Remove("-" + name);
                    if (lval) cparameter.Remove("--" + name);
                    ret[i] = val || lval;
                }
            }
        }

        private object[] Cast(object[] parameter)
        {
            ParameterInfo[] pt = AllowedParameterTypes;
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
                            ret[i] = CommandAttributeUtils.ConvertToNonGeneric(cparameter[i - off], pt[i].ParameterType);
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

        //private bool IsAllowedSignature(object[] parameter)
        //{
        //    ParameterInfo[] pt = AllowedParameterTypes;

        //    int off = 0;
        //    if (pt.Length != parameter.Length) return false;

        //    bool ret = true;

        //    for (int i = 0; i < pt.Length; i++)
        //    {
        //        SelectionPropertyAttribute sp = pt[i].GetCustomAttribute<SelectionPropertyAttribute>();
        //        if (sp != null)
        //        {
        //            off++;
        //            continue;
        //        }
        //        CommandFlagAttribute cfa = pt[i].GetCustomAttribute<CommandFlagAttribute>();
        //        if (cfa != null)
        //        {
        //            off++;
        //            continue;
        //        }

        //        Type type = pt[i].ParameterType;
        //        ret &= type.IsInstanceOfType(parameter[i - off]);
        //    }

        //    return ret;
        //}
    }
}