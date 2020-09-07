using System;
using System.Collections;

using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator.Core
{
    /// <summary>
    /// The Main Class of the Evaluator.
    /// </summary>
    public class Evaluator
    {

        /// <summary>
        /// If True all Variables and Functions will be matched in case sensitive
        /// </summary>
        public readonly bool CaseSensitive;

        /// <summary>
        /// Defines the Parser Syntax of the Evaluator
        /// </summary>
        public readonly ParserSyntax Syntax;

        /// <summary>
        /// The List of Classes that Inherit IVariableBack and/or IEvalFunctions
        /// </summary>
        internal ArrayList mEnvironmentFunctionsList;

        /// <summary>
        /// If true will throw an exception when a parameter/function is not found.
        /// </summary>
        public bool RaiseVariableNotFoundException;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="syntax">Evaluator Syntax</param>
        /// <param name="caseSensitive">If True all Variables and Functions will be matched in case sensitive</param>
        public Evaluator(ParserSyntax syntax = ParserSyntax.VisualBasic, bool caseSensitive = false)
        {
            Syntax = syntax;
            CaseSensitive = caseSensitive;
            mEnvironmentFunctionsList = new ArrayList();
        }

        /// <summary>
        /// Adds an Object to the List of Environment Functions/Variables.
        /// </summary>
        /// <param name="obj">Object to Add</param>
        public void AddEnvironmentFunctions(object obj)
        {
            if (obj is null)
            {
                return;
            }

            if (!mEnvironmentFunctionsList.Contains(obj))
            {
                mEnvironmentFunctionsList.Add(obj);
            }
        }

        /// <summary>
        /// Removes an Object from the List of Environment Functions/Variables.
        /// </summary>
        /// <param name="obj">Object to Remove</param>
        public void RemoveEnvironmentFunctions(object obj)
        {
            if (mEnvironmentFunctionsList.Contains(obj))
            {
                mEnvironmentFunctionsList.Remove(obj);
            }
        }

        /// <summary>
        /// Parses the Specifed Expression
        /// </summary>
        /// <param name="str">The Expression to Parse</param>
        /// <returns>Parsed Expression</returns>
        public OPCode Parse(string str)
        {
            return new Parser(this).Parse(str);
        }

        /// <summary>
        /// Converts an Object to its string representation
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns>Converted Value</returns>
        public static string ConvertToString(object value)
        {
            if (value is string)
            {
                return (string) value;
            }

            if (value is null)
            {
                return string.Empty;
            }

            if (value is DateTime)
            {
                DateTime d = (DateTime) value;
                if (d.TimeOfDay.TotalMilliseconds > 0)
                {
                    return d.ToString();
                }

                return d.ToShortDateString();
            }

            if (value is decimal)
            {
                decimal d = (decimal) value;
                if (d % 1 != 0)
                {
                    return d.ToString("#,##0.00");
                }

                return d.ToString("#,##0");
            }

            if (value is double)
            {
                double d = (double) value;
                if (d % 1 != 0)
                {
                    return d.ToString("#,##0.00");
                }

                return d.ToString("#,##0");
            }

            if (value is object)
            {
                return value.ToString();
            }

            return default;
        }

    }
}