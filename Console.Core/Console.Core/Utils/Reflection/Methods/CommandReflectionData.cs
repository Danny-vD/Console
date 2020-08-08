using System;
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

        /// <summary>
        /// Gets called by the Console System
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object Invoke(object[] parameter)
        {
            if (!IsAllowedSignature(parameter))
            {
                //Cast
                object[] parame = Cast(parameter);
                string s = "Parameters: ";
                foreach (object o in parame)
                {
                    s += o + "; ";
                }
                return InvokeDirect(parame);
            }
            else
            {
                return InvokeDirect(parameter.Length == 0 ? null : parameter);
            }
        }

        public object InvokeDirect(object[] parameter)
        {
            return Info.InvokePreserveStack(Instance, parameter);
        }

        private object[] Cast(object[] parameter)
        {
            ParameterInfo[] pt = AllowedParameterTypes;

            object[] ret = new object[pt.Length];
            int off = 0;
            for (int i = 0; i < pt.Length; i++)
            {
                SelectionPropertyAttribute sp = pt[i].GetCustomAttribute<SelectionPropertyAttribute>();
                if (sp != null && AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0 &&
                    pt.Length == parameter.Length)
                {

                }
                if (sp != null)
                {

                    object v = AConsoleManager.Instance.ObjectSelector.SelectedObjects.ToArray();
                    if (AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0)
                    {
                        if (pt.Length == parameter.Length)
                        {
                            ret[i] = CommandAttributeUtils.ConvertToNonGeneric(parameter[i - off], pt[i].ParameterType);
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

                Type type = pt[i].ParameterType;
                ret[i] = CommandAttributeUtils.ConvertToNonGeneric(parameter[i - off], type);
            }

            return ret;
        }

        private bool IsAllowedSignature(object[] parameter)
        {
            ParameterInfo[] pt = AllowedParameterTypes;
            int off = 0;
            if (pt.Length != parameter.Length) return false;

            bool ret = true;

            for (int i = 0; i < pt.Length; i++)
            {
                SelectionPropertyAttribute sp = pt[i].GetCustomAttribute<SelectionPropertyAttribute>();
                if (sp != null)
                {
                    off++;
                    continue;
                }

                Type type = pt[i].ParameterType;
                ret &= type.IsInstanceOfType(parameter[i - off]);
            }

            return ret;
        }
    }
}