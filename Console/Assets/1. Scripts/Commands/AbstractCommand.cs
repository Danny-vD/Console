using System.Collections.Generic;

namespace Commands
{
	public abstract class AbstractCommand
	{
		public readonly int ParametersCount;
		
		protected string Name;
		protected List<string> Aliases;
        
		protected string HelpMessage = "No help message set";
		
		public abstract void Invoke(params object[] parameters);

		protected AbstractCommand(int paramsCount)
		{
			ParametersCount = paramsCount;
			Aliases = new List<string>();
		}

		public bool HasName(string name)
		{
			return Name == name || Aliases.Contains(name);
		}
	}
}