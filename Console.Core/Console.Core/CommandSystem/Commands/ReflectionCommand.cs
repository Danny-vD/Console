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
    /// Reflection Wrapper for the Console.
    /// </summary>
    public class ReflectionCommand : AbstractCommand
    {
        /// <summary>
        /// The Reflection Data
        /// </summary>
        public IInvokable RefData { get; }

        /// <summary>
        /// The Command Attriute
        /// </summary>
        public CommandAttribute Command => RefData.Attributes.FirstOrDefault(x => x is CommandAttribute) as CommandAttribute;

        /// <summary>
        /// Amount of Parameters that are decorated with the CommandFlagAttribute
        /// </summary>
        public override int FlagAttributeCount =>
            RefData.ParameterTypes.Count(x => x.Attributes.Count(y => y is CommandFlagAttribute) != 0);

        /// <summary>
        /// Amount of Parameters that are decorated with the SelectionPropertyAttribute
        /// </summary>
        public override int SelectionAttributeCount =>
            RefData.ParameterTypes.Count(x => x.Attributes.Count(y => y is SelectionPropertyAttribute) != 0);

        /// <summary>
        /// Creates a Command based on an IInvokable Instance
        /// </summary>
        /// <param name="refData">The Reflection Data</param>
        public ReflectionCommand(IInvokable refData) : base(refData.ParameterCount)
        {
            RefData = refData;
            ParametersCount = new ParameterRange(ParametersCount.Max - FlagAttributeCount, ParametersCount.Max);

            //Setting the Data from the attributes
            SetName(Command.Name ?? RefData.Name);
            SetHelpMessage(Command.HelpMessage);
            Aliases.AddRange(Command.Aliases);
        }

        /// <summary>
        /// Creates a Command Based on a Static Method Info
        /// </summary>
        /// <param name="info">Method Info used as backend</param>
        public ReflectionCommand(MethodInfo info) : this(null, info) { }
        /// <summary>
        /// Creates a Command based on a Method Info and the corresponding Object Instance.
        /// </summary>
        /// <param name="instance">Instance bound to the Method Info</param>
        /// <param name="info">Method info used as Backend</param>
        public ReflectionCommand(object instance, MethodInfo info) : this(new MethodMetaData(instance, info)) { }


        /// <summary>
        /// Returns the name, plus all the parameter types
        /// </summary>
        /// <returns>The Full Name including Signature</returns>
        public override string GetFullName(ToStringMode mode)
        {
            if (mode == ToStringMode.None) return "";
            if (mode == ToStringMode.Short) return $"{Name} Parameter Count: " + RefData.ParameterCount;
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

        /// <summary>
        /// Invokes this Command with the specified parameters.
        /// </summary>
        /// <param name="parameters">Parameters of the Command.</param>
        public override void Invoke(params object[] parameters)
        {
            RefData.Invoke(parameters);
        }
    }
}