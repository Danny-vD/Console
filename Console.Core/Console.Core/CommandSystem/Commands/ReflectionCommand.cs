using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Console.Core.ReflectionSystem;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.CommandSystem.Commands
{
    /// <summary>
    /// Wrapper for the Console.
    /// </summary>
    public class ReflectionCommand : AbstractCommand
    {
        public IInvokable RefData { get; }
        public CommandAttribute Command => RefData.Attributes.FirstOrDefault(x => x is CommandAttribute) as CommandAttribute;
        public int FlagAttributeCount =>
            RefData.ParameterTypes.Count(x => x.Attributes.Count(y => y is CommandFlagAttribute) != 0);
        public int SelectionAttributeCount =>
            RefData.ParameterTypes.Count(x => x.Attributes.Count(y => y is SelectionPropertyAttribute) != 0);
        public ReflectionCommand(IInvokable refData) : base(refData.ParameterCount)
        {
            RefData = refData;
            ParametersCount = new ParameterRange(ParametersCount.Max - FlagAttributeCount, ParametersCount.Max);

            //Setting the Data from the attributes
            SetName(Command.Name ?? RefData.Name);
            SetHelpMessage(Command.HelpMessage);
            Aliases.AddRange(Command.Aliases);
        }

        public ReflectionCommand(MethodInfo info) : this(null, info) { }
        public ReflectionCommand(object instance, MethodInfo info) : this(new MethodMetaData(instance, info)) { }

        public override string GetFullName()
        {
            ParameterMetaData[] parameters = RefData.ParameterTypes;

            StringBuilder stringBuilder = new StringBuilder();

            if (parameters.Length != 0)
            {
                stringBuilder.Append(" (");

                for (int i = 0; i < parameters.Length; i++)
                {
                    string text = parameters[i].ParameterType.Name + " " + parameters[i].ReflectedInfo.Name;
                    List<Attribute> abs = parameters[i].Attributes;
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

            return Name + $"{stringBuilder}";
        }

        //Simple Wrapper
        public override void Invoke(params object[] parameters)
        {
            RefData.Invoke(parameters);
        }
    }
}