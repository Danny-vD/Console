using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Core.Commands.ExpandSystem;
using Console.Core.Console;
using VDFramework.Standard.SharedClasses.Extensions;

namespace Console.Core.Commands
{
    public abstract class AbstractCommand
    {
        public readonly int ParametersCount;

        protected string Name;
        protected readonly List<string> Aliases;

        protected string HelpMessage = "No help message set.";

        public abstract void Invoke(params object[] parameters);

        protected AbstractCommand(int paramsCount)
        {
            ParametersCount = paramsCount;
            Aliases = new List<string>();
        }

        protected static bool IsValidCast<TType>(object parameter)
        {
            string newTypeName = typeof(TType).Name;

            try
            {

                if (CustomConvertManager.CanConvert(parameter, typeof(TType))) return true;
                

                parameter.ConvertTo<TType>();
                // Convert to check if it will throw an error

                return true;
            }
            catch
            {
                AConsoleManager.Instance.LogError(
                    $"{parameter} ({parameter.GetType().Name}) can not be converted to type {newTypeName}!");
                return false;
            }
        }

        protected static TNewType ConvertTo<TNewType>(object parameter)
        {

            if (CustomConvertManager.CanConvert(parameter, typeof(TNewType)))
                return (TNewType)CustomConvertManager.Convert(parameter, typeof(TNewType));
            
            return parameter.ConvertTo<TNewType>();
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public string GetName()
        {
            return Name;
        }

        public List<string> GetAllNames()
        {
            List<string> names = new List<string>() { Name };

            Aliases.ForEach(names.Add);

            return names;
        }

        /// <summary>
        /// Returns the name, plus all the parameter types
        /// </summary>
        public abstract string GetFullName();

        public bool HasName(string name)
        {
            return Name == name || Aliases.Contains(name);
        }

        public bool HasName(IEnumerable<string> names)
        {
            if (names.Any(HasName))
            {
                return true;
            }

            return false;
        }

        public void AddAlias(string alias)
        {
            if (Aliases.Contains(alias))
            {
                return;
            }

            Aliases.Add(alias);
        }

        public void RemoveAlias(string name)
        {
            Aliases.Remove(name);
        }

        public void SetHelpMessage(string message)
        {
            HelpMessage = message;
        }

        public string GetHelpMessage()
        {
            return HelpMessage;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(ConsoleCoreConfig.ConsolePrefix);

            stringBuilder.Append(GetFullName());
            stringBuilder.Append(": ");
            stringBuilder.AppendLine(HelpMessage);

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Aliases: ");

            if (Aliases.Count == 0)
            {
                stringBuilder.AppendLine("None");
            }

            foreach (string alias in Aliases)
            {
                stringBuilder.AppendLine(alias);
            }

            return stringBuilder.ToString();

            // {Name}: {help}

            // Aliases:
            // Alias1
            // Alias2
            // etc.
        }
    }
}