using System;
using System.Collections.Generic;
using System.Text;
using Console.Core.CommandSystem.Commands;

/// <summary>
/// The Console.Core.CommandSystem namespace contains the Parsing Logic as well as BuiltIn Commands and Classes Related to Command Execution.
/// </summary>
namespace Console.Core.CommandSystem
{
    /// <summary>
    /// Command Parser does the Parsing and Invocation of the Input Command
    /// </summary>
	public class CommandParser
    {
        /// <summary>
        /// Parses and Processes the passed command.
        /// </summary>
        /// <param name="command">Command String to be Parsed.</param>
		public void OnSubmitCommand(string command)
        {
            if (ConsoleCoreConfig.ConsolePrefix != string.Empty && !command.StartsWith(ConsoleCoreConfig.ConsolePrefix))
            {
                AConsoleManager.Instance.LogWarning(
                    $"Command does not start with prefix: " + ConsoleCoreConfig.ConsolePrefix);
                return;
            }

            if (ConsoleCoreConfig.ConsolePrefix != string.Empty)
            {
                command = command.Remove(0, ConsoleCoreConfig.ConsolePrefix.Length);
            }

            InnerParseAndInvoke(command);
        }

        /// <summary>
        /// Parses the Command String and Invokes the correct Command with the Correct Parameters
        /// </summary>
        /// <param name="arguments">The Argument Part of the Command.</param>
		private static void InnerParseAndInvoke(string arguments)
        {
            string[] commandArguments = Split(arguments, ' ');
            if (commandArguments.Length == 0) return;
            string commandName = commandArguments[0];

            if (commandArguments.Length == 1 && //No Arguments
                AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0)
            {
                CommandManager.Invoke(commandName);
                return;
            }

            arguments = arguments.Remove(0, commandName.Length);
            arguments = arguments.Trim();

            if (arguments.Contains(ConsoleCoreConfig.StringChar.ToString()))
            {
                CommandManager.Invoke(commandName, ParseStringBlocks(arguments));
            }
            else
            {
                CommandManager.Invoke(commandName, Split(arguments, ' '));
            }
        }

        /// <summary>
        /// Splits the string at the specified character.
        /// Does remove empty entries.
        /// </summary>
        /// <param name="arguments">The Argument Part of the Command</param>
        /// <param name="split">The Split Char</param>
        /// <returns>Result of the Split Operation</returns>
        private static string[] Split(string arguments, char split)
        {
            return arguments.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Parses the Arguments with respect to the string character
        /// </summary>
        /// <param name="commandArguments">The Argument Part of the Command.</param>
        /// <returns>Correctly Parsed Array ofString Blocks</returns>
		private static string[] ParseStringBlocks(string commandArguments)
        {
            string[] sections = commandArguments.Split(' ');

            List<string> arguments = InnerParseStringBlocks(sections);

            return arguments.ToArray();
        }


        /// <summary>
        /// Removes all Non-Escaped chars from the string
        /// </summary>
        /// <param name="part">String with escaped chars</param>
        /// <param name="chars">The Escaped Chars</param>
        /// <returns>Un escaped string</returns>
        private static string RemoveNonEscapedChars(string part, params char[] chars)
        {
            string ret = "";
            for (int i = 0; i < part.Length; i++)
            {
                bool isChar = false;
                for (int j = 0; j < chars.Length; j++)
                {
                    if (chars[j] == part[i])
                    {
                        isChar = true;
                        break;
                    }
                }
                bool esc = IsEscaped(part, i);
                if (!isChar || esc)
                {
                    ret += part[i];
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns true if the Character at idx is escaped by a previous character
        /// </summary>
        /// <param name="part">Containing Part</param>
        /// <param name="idx">Index of the character to check</param>
        /// <returns>True if the Character is escaped.</returns>
        private static bool IsEscaped(string part, int idx)
        {
            if (idx == 0) return false;
            if (part[idx - 1] == ConsoleCoreConfig.EscapeChar) //Previous Char is Escape Char
            {
                //If the previous char is not escaped this char is escaped
                return !IsEscaped(part, idx - 1);
            }

            return false;
        }

        /// <summary>
        /// Splits the Arguments based on the Position of the String Character
        /// </summary>
        /// <param name="parts">THe Argument Parts of the Command</param>
        /// <returns>Correctly Parsed Array ofString Blocks</returns>
		private static List<string> InnerParseStringBlocks(string[] parts)
        {
            bool append = false;
            StringBuilder stringBuilder = null;

            string stringSplit = ConsoleCoreConfig.StringChar.ToString();

            List<string> arguments = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                int strCharIdx = parts[i].IndexOf(ConsoleCoreConfig.StringChar);
                bool containsStringChar = strCharIdx != -1;
                if (containsStringChar && strCharIdx > 0 && parts[i][strCharIdx - 1] == '\\')
                {
                    containsStringChar = false;
                    //append = true;
                }
                if (!append && containsStringChar)
                {
                    append = true;
                    stringBuilder = new StringBuilder(parts[i]);

                    if (parts[i].EndsWith(stringSplit))
                    {
                        string argumentString = stringBuilder.ToString();
                        stringBuilder.Clear();
                        arguments.Add(RemoveNonEscapedChars(argumentString, ConsoleCoreConfig.EscapableChars));
                        append = false;
                    }

                    continue;
                }

                if (append)
                {
                    stringBuilder.Append($" {parts[i]}");

                    if (containsStringChar)
                    {
                        string argumentString = stringBuilder.ToString();
                        stringBuilder.Clear();
                        arguments.Add(RemoveNonEscapedChars(argumentString, ConsoleCoreConfig.EscapableChars));
                        append = false;
                    }

                    continue;
                }

                arguments.Add(RemoveNonEscapedChars(parts[i], ConsoleCoreConfig.EscapableChars));
            }

            // ReSharper disable once PossibleNullReferenceException
            // Reason: false positive (it's guaranteed to be initialised)
            if (stringBuilder.Length != 0)
            {
                string argumentString = stringBuilder.ToString();
                arguments.Add(RemoveNonEscapedChars(argumentString, ConsoleCoreConfig.EscapableChars));
            }

            return arguments;
        }
    }
}