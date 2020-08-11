using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Console.Core.Commands;

namespace Console.Core.Utils.Reflection.Methods
{
    /// <summary>
    /// Wrapper for the Console.
    /// </summary>
    public class ReflectionCommand : AbstractCommand
    {
        public CommandReflectionData RefData { get; }

        public ReflectionCommand(CommandReflectionData refData) : base(refData.AllowedParameterTypes.Length)
        {
            RefData = refData;

            //Setting the Data from the attributes
            SetName(refData.Attribute.Name);
            SetHelpMessage(refData.Attribute.HelpMessage);
            Aliases.AddRange(refData.Attribute.Aliases);
        }

        public override string GetFullName()
        {
            ParameterInfo[] parameters = RefData.Info.GetParameters();

            StringBuilder stringBuilder = new StringBuilder();

            if (parameters.Length != 0)
            {
                stringBuilder.Append(" (");

                for (int i = 0; i < parameters.Length; i++)
                {
                    string text = parameters[i].ParameterType.Name + " " + parameters[i].Name;
                    List<Attribute> abs = parameters[i].GetCustomAttributes(true).Cast<Attribute>().ToList();
                    if (abs.Count != 0)
                    {
                        string a = "[";
                        for (int j = 0; j < abs.Count; j++)
                        {
                            Attribute attribute = abs[j];
                            a += $"{attribute}";
                            if (j != abs.Count - 1)
                                a += ", ";
                        }
                        a += "]";
                        text = a + text;
                    }
                    if (i != 0)
                        text = ", " + text;
                    stringBuilder.Append(text);
                }

                stringBuilder.Append(")");
            }

            return RefData.Attribute.Name + $"{stringBuilder}";
        }

        //Simple Wrapper
        public override void Invoke(params object[] parameters)
        {
            RefData.Invoke(parameters);
        }
    }
}