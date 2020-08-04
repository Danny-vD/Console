using System.Reflection;
using System.Text;

namespace Console.Commands
{
    /// <summary>
    /// Wrapper for the Console.
    /// </summary>
    public class ReflectionCommand : AbstractCommand
    {
        private readonly CommandReflectionData refData;

        public ReflectionCommand(CommandReflectionData refData) : base(refData.AllowedParameterTypes.Length)
        {
            this.refData = refData;

            //Setting the Data from the attributes
            SetName(refData.Attribute.Name);
            SetHelpMessage(refData.Attribute.HelpMessage);
            Aliases.AddRange(refData.Attribute.Aliases);
        }

        public override string GetFullName()
        {
            ParameterInfo[] parameters = refData.Info.GetParameters();

            StringBuilder stringBuilder = new StringBuilder();

            if (parameters.Length != 0)
            {
                stringBuilder.Append(" (");
                stringBuilder.Append(parameters[0].ParameterType.Name);

                for (int i = 1; i < parameters.Length; i++)
                {
                    stringBuilder.Append($", {parameters[i].ParameterType.Name}");
                }

                stringBuilder.Append(")");
            }

            return refData.Attribute.Name + $"{stringBuilder}";
        }

        //Simple Wrapper
        public override void Invoke(params object[] parameters)
        {
            refData.Invoke(parameters);
        }
    }
}