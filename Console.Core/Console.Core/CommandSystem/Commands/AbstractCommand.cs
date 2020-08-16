using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Core.ConverterSystem;

namespace Console.Core.CommandSystem.Commands
{
    /// <summary>
    /// Abstract Command Base Class.
    /// </summary>
    public abstract class AbstractCommand
    {
        /// <summary>
        /// The Parameter Range of the Command
        /// </summary>
        public ParameterRange ParametersCount { get; protected set; }

        /// <summary>
        /// Command Name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// List of Alternative Command Names
        /// </summary>
        protected readonly List<string> Aliases;

        /// <summary>
        /// The Help Message of this Command.
        /// </summary>
        protected string HelpMessage = "No help message set.";

        /// <summary>
        /// Invokes this Command with the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        public abstract void Invoke(params object[] parameters);

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="paramsCount">Amount of Parameters</param>
        protected AbstractCommand(int paramsCount)
        {
            ParametersCount = new ParameterRange(paramsCount);
            Aliases = new List<string>();
        }

        /// <summary>
        /// Returns true when the Parameter is the correct type for the command.
        /// </summary>
        /// <typeparam name="TType">Target Type</typeparam>
        /// <param name="parameter">The Value to Test</param>
        /// <returns>True if the Cast is Possible/Valid</returns>
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

        /// <summary>
        /// Converts the Specified Parameter to Type TNewType.
        /// </summary>
        /// <typeparam name="TNewType">Target Type</typeparam>
        /// <param name="parameter">Value to Convert</param>
        /// <returns>The Changed Parameter</returns>
        protected static TNewType ConvertTo<TNewType>(object parameter)
        {

            if (CustomConvertManager.CanConvert(parameter, typeof(TNewType)))
                return (TNewType)CustomConvertManager.Convert(parameter, typeof(TNewType));

            return parameter.ConvertTo<TNewType>();
        }

        /// <summary>
        /// Sets the Command Name
        /// </summary>
        /// <param name="name">New Name</param>
        public void SetName(string name)
        {
            Name = name;
        }
        

        /// <summary>
        /// Returns all Command Names including Aliases.
        /// </summary>
        /// <returns>All Names and Aliases in one Array</returns>
        public List<string> GetAllNames()
        {
            List<string> names = new List<string>() { Name };

            Aliases.ForEach(names.Add);

            return names;
        }

        /// <summary>
        /// Returns the name, plus all the parameter types
        /// </summary>
        /// <returns>The Full Name including Signature</returns>
        public abstract string GetFullName();

        /// <summary>
        /// Returns true if the Command has this Name or Alias.
        /// </summary>
        /// <param name="name">Name to test against.</param>
        /// <returns>True if the Name does match this Commands Name or one of its Aliases.</returns>
        public bool HasName(string name)
        {
            return Name == name || Aliases.Contains(name);
        }

        /// <summary>
        /// Returns true if any of the names is contained in this Command.
        /// </summary>
        /// <param name="names">Names to test against.</param>
        /// <returns>True if any Name does match this Commands Name or one of its Aliases.</returns>
        public bool HasName(IEnumerable<string> names)
        {
            return names.Any(HasName);
        }

        /// <summary>
        /// Adds an Alias to this Command.
        /// </summary>
        /// <param name="alias">Alias to add</param>
        public void AddAlias(string alias)
        {
            if (Aliases.Contains(alias))
            {
                return;
            }

            Aliases.Add(alias);
        }

        /// <summary>
        /// Removes an Alias from this Command.
        /// </summary>
        /// <param name="name">Alias to Remove</param>
        public void RemoveAlias(string name)
        {
            Aliases.Remove(name);
        }

        /// <summary>
        /// Sets the Help Message of this Command.
        /// </summary>
        /// <param name="message">New Help Message</param>
        public void SetHelpMessage(string message)
        {
            HelpMessage = message;
        }
        
        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns>String Representation of a Command.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(ConsoleCoreConfig.ConsolePrefix);

            stringBuilder.Append(GetFullName());
            stringBuilder.Append(": \n");
            stringBuilder.AppendLine("\t" + HelpMessage);

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Aliases: ");

            if (Aliases.Count == 0)
            {
                stringBuilder.AppendLine("\tNone");
            }

            foreach (string alias in Aliases)
            {
                stringBuilder.AppendLine("\t" + alias);
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