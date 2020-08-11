using System;
using System.Collections;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator.Core
{
    public class Evaluator
    {
        internal ArrayList mEnvironmentFunctionsList;
        public bool RaiseVariableNotFoundException;
        public readonly ParserSyntax Syntax;
        public readonly bool CaseSensitive;

        public Evaluator(ParserSyntax syntax = ParserSyntax.VisualBasic, bool caseSensitive = false)
        {
            Syntax = syntax;
            CaseSensitive = caseSensitive;
            mEnvironmentFunctionsList = new ArrayList();
        }

        public void AddEnvironmentFunctions(object obj)
        {
            if (obj is null)
                return;
            if (!mEnvironmentFunctionsList.Contains(obj))
            {
                mEnvironmentFunctionsList.Add(obj);
            }
        }

        public void RemoveEnvironmentFunctions(object obj)
        {
            if (mEnvironmentFunctionsList.Contains(obj))
            {
                mEnvironmentFunctionsList.Remove(obj);
            }
        }

        public OPCode Parse(string str)
        {
            return new parser(this).Parse(str);
        }

        public static string ConvertToString(object value)
        {
            if (value is string)
            {
                return (string)value;
            }
            else if (value is null)
            {
                return string.Empty;
            }
            else if (value is DateTime)
            {
                DateTime d = (DateTime)value;
                if (d.TimeOfDay.TotalMilliseconds > 0)
                {
                    return d.ToString();
                }
                else
                {
                    return d.ToShortDateString();
                }
            }
            else if (value is decimal)
            {
                decimal d = (decimal)value;
                if (d % 1 != 0)
                {
                    return d.ToString("#,##0.00");
                }
                else
                {
                    return d.ToString("#,##0");
                }
            }
            else if (value is double)
            {
                double d = (double)value;
                if (d % 1 != 0)
                {
                    return d.ToString("#,##0.00");
                }
                else
                {
                    return d.ToString("#,##0");
                }
            }
            else if (value is object)
            {
                return value.ToString();
            }

            return default;
        }

        public class parserException : Exception
        {
            public readonly string formula;
            public readonly int pos;

            internal parserException(string str, string formula, int pos) : base(str)
            {
                this.formula = formula;
                this.pos = pos;
            }
        }
    }
}