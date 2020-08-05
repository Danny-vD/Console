using System;
using System.Reflection;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Console;

namespace Console.Core.Commands.CommandImplementations.Reflection
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
                return Info.Invoke(Instance, Cast(parameter));
            }
            else
            {
                return Info.Invoke(Instance, parameter);
            }
        }

        private object[] Cast(object[] parameter)
        {
            ParameterInfo[] pt = AllowedParameterTypes;

            object[] ret = new object[pt.Length];
            int off = 0;
            for (int i = 0; i < pt.Length; i++)
            {
                SelectionPropertyAttribute sp = pt[i].GetCustomAttribute<SelectionPropertyAttribute>();
                if (AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count != 0 && sp != null)
                {

                    ret[i] = AConsoleManager.Instance.ObjectSelector.SelectedObjects.ToArray();

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