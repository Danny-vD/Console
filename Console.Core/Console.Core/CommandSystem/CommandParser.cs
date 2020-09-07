using System;
using System.Collections.Generic;
using System.Text;

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
                ConsoleCoreConfig.CoreLogger.LogWarning(
                                                        "Command does not start with prefix: " +
                                                        ConsoleCoreConfig.ConsolePrefix
                                                       );
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
            string[] commandArguments = Split(arguments, ConsoleCoreConfig.CommandInputSeparator);
            if (commandArguments.Length == 0)
            {
                return;
            }

            string commandName = commandArguments[0].Trim();
            if (string.IsNullOrEmpty(commandName))
            {
                return;
            }

            if (commandArguments.Length == 1 && //No Arguments
                AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0)
            {
                CommandManager.Invoke(commandName);
                return;
            }

            //System.Console.WriteLine("Next Command: " + arguments);

            arguments = arguments.Remove(0, commandName.Length);
            arguments = arguments.Trim();


            //System.Console.ReadLine();

            if (arguments.Contains(ConsoleCoreConfig.StringChar.ToString()))
            {
                CommandManager.Invoke(commandName, ParseStringBlocks(arguments));
            }
            else
            {
                CommandManager.Invoke(commandName, Split(arguments, ConsoleCoreConfig.CommandInputSeparator));
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
        public static string[] ParseStringBlocks(string commandArguments)
        {
            if (commandArguments == null)
            {
                return new string[0];
            }

            string[] sections = commandArguments.Split(ConsoleCoreConfig.CommandInputSeparator);

            List<string> arguments = InnerParseStringBlocks(sections);

            return arguments.ToArray();
        }

        /// <summary>
        /// Removes all String Characters that are not escaped.
        /// </summary>
        /// <param name="content">Content to Check</param>
        /// <returns>Cleaned Content</returns>
        public static string CleanContent(string content)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == ConsoleCoreConfig.StringChar && !IsEscaped(content, i))
                {
                    continue;
                }

                sb.Append(content[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// UnEscape is reversing the Escape Encoding
        /// </summary>
        /// <param name="content">Content to Unescape</param>
        /// <param name="escChar">The Character that is used to escape other characters</param>
        /// <param name="escapeChars">The Characters that need to be Escaped.</param>
        /// <returns>UnEscaped String</returns>
        public static string UnEscape(string content, char escChar, params char[] escapeChars)
        {
            string ret = content;
            for (int i = 0; i < escapeChars.Length; i++)
            {
                ret = ret.Replace(escChar + escapeChars[i].ToString(), escapeChars[i].ToString());
            }

            ret = ret.Replace(escChar.ToString() + escChar, escChar.ToString());
            return ret;
        }

        /// <summary>
        /// Escape is Masking the specified escapeChars with the escChar character.
        /// </summary>
        /// <param name="content">Content to Escape</param>
        /// <param name="escChar">The Character that is used to escape other characters</param>
        /// <param name="escapeChars">The Characters that need to be Escaped.</param>
        /// <returns>Escaped String</returns>
        public static string Escape(string content, char escChar, params char[] escapeChars)
        {
            string ret = content;

            ret = ret.Replace(escChar.ToString(), escChar + escChar.ToString());
            for (int i = 0; i < escapeChars.Length; i++)
            {
                ret = ret.Replace(escapeChars[i].ToString(), escChar.ToString() + escapeChars[i]);
            }

            return ret;
        }

        /// <summary>
        /// Returns true if the Character at idx is escaped by a previous character
        /// </summary>
        /// <param name="part">Containing Part</param>
        /// <param name="idx">Index of the character to check</param>
        /// <returns>True if the Character is escaped.</returns>
        public static bool IsEscaped(string part, int idx)
        {
            if (idx <= 0)
            {
                return false;
            }

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
                bool containsStringChar = false;
                int strCharIdx = -1;
                while ((strCharIdx = parts[i].IndexOf(ConsoleCoreConfig.StringChar, strCharIdx + 1)) != -1)
                {
                    if (!IsEscaped(parts[i], strCharIdx))
                    {
                        containsStringChar = true;
                        break;
                    }
                }

                if (!append && containsStringChar)
                {
                    append = true;
                    stringBuilder = new StringBuilder(parts[i]);

                    if (parts[i].EndsWith(stringSplit))
                    {
                        string argumentString = stringBuilder.ToString();
                        stringBuilder.Clear();
                        arguments.Add(
                                      UnEscape(
                                               CleanContent(argumentString),
                                               ConsoleCoreConfig.EscapeChar,
                                               ConsoleCoreConfig.EscapableChars
                                              )
                                     );
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
                        arguments.Add(
                                      UnEscape(
                                               CleanContent(argumentString),
                                               ConsoleCoreConfig.EscapeChar,
                                               ConsoleCoreConfig.EscapableChars
                                              )
                                     );
                        append = false;
                    }

                    continue;
                }

                arguments.Add(CleanContent(parts[i]));
            }

            if (stringBuilder != null && stringBuilder.Length != 0)
            {
                string argumentString = stringBuilder.ToString();
                arguments.Add(CleanContent(argumentString));
            }

            return arguments;
        }

    }
}