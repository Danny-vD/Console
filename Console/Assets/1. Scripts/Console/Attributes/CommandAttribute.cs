using System;

namespace Console.Attributes
{
	/// <summary>
	/// Command Attribute that only works on methods.
	/// Allows multiple Attributes on the same methods(this is a fancy way to create aliases without using the alias system :D)
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		public readonly string Name;
		public readonly string HelpMessage;
		public readonly string[] Aliases;

		public CommandAttribute(string name, string helpMessage, params string[] alias)
		{
			Name        = name;
			HelpMessage = helpMessage;
			Aliases     = alias;
		}
	}
}