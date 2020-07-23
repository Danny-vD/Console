using System.Collections.Generic;
using System.Linq;
using VDFramework.Singleton;

namespace Commands
{
	public class CommandManager : Singleton<CommandManager>
	{
		private readonly List<AbstractCommand> commands = new List<AbstractCommand>();

		private AbstractCommand GetCommand(string commandName)
		{
			AbstractCommand command = commands.FirstOrDefault(item => item.HasName(name));

			if (command == null)
			{
				// log to the console that it does not exist
			}

			return command;
		}
	}
}