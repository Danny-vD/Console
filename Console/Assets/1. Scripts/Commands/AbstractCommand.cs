using System.Collections.Generic;

namespace Commands
{
	public abstract class AbstractCommand
	{
		public readonly int ParametersCount;
		
		protected string Name;
		protected readonly List<string> Aliases;
        
		protected string HelpMessage = "No help message set";
		
		public abstract void Invoke(params object[] parameters);

		protected AbstractCommand(int paramsCount)
		{
			ParametersCount = paramsCount;
			Aliases = new List<string>();
		}

		public void SetName(string name)
		{
			Name = name;
		}
		
		public bool HasName(string name)
		{
			return Name == name || Aliases.Contains(name);
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
	}
}