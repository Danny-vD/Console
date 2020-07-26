using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console;
using VDFramework.Extensions;

namespace Commands
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
			if (parameter.HasValidCastTo<TType>())
			{
				return true;
			}
			
			ConsoleManager.Instance.LogError($"{parameter} ({parameter.GetType().Name}) is not of type {typeof(TType).Name}");
			return false;
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
			List<string> names = new List<string>() {Name};
			
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
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append(GetFullName());
			stringBuilder.Append(": ");
			stringBuilder.AppendLine(HelpMessage);

			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Aliases: ");

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