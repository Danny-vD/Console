using System;
using System.Text;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core
{
    public class tokenizer
    {
        private string mString;
        private int mLen;
        private int mPos;
        private char mCurChar;
        private parser mParser;
        private ParserSyntax mSyntax;
        public int startpos;
        public TokenType type;
        public System.Text.StringBuilder value = new System.Text.StringBuilder();

        internal tokenizer(parser Parser, string str, ParserSyntax syntax = ParserSyntax.VisualBasic)
        {
            mString = str;
            mLen = str.Length;
            mSyntax = syntax;
            mPos = 0;
            mParser = Parser;
            NextChar();   // start the machine
        }

        internal void RaiseError(string msg, Exception ex = null)
        {
            if (ex is Evaluator.parserException)
            {
                msg += ". " + ex.Message;
            }
            else
            {
                msg += " " + " at position " + startpos;
                if (ex is object)
                {
                    msg += ". " + ex.Message;
                }
            }

            throw new Evaluator.parserException(msg, mString, mPos);
        }

        internal void RaiseUnexpectedToken(string msg = null)
        {
            if (msg.Length == 0)
            {
                msg = "";
            }
            else
            {
                msg += "; ";
            }

            RaiseError(msg + "Unexpected " + type.ToString().Replace('_', ' ') + " : " + value.ToString());
        }

        internal void RaiseWrongOperator(TokenType tt, object ValueLeft, object valueRight, string msg = null)
        {
            if (msg.Length > 0)
            {
                msg.Replace("[op]", tt.GetType().ToString());
                msg += ". ";
            }

            msg = "Cannot apply the operator " + tt.ToString();
            if (ValueLeft is null)
            {
                msg += " on nothing";
            }
            else
            {
                msg += " on a " + ValueLeft.GetType().ToString();
            }

            if (valueRight is object)
            {
                msg += " and a " + valueRight.GetType().ToString();
            }

            RaiseError(msg);
        }

        private bool IsOp()
        {
            return mCurChar == '+' | mCurChar == '-' | mCurChar == '–' | mCurChar == '%' | mCurChar == '/' | mCurChar == '(' | mCurChar == ')' | mCurChar == '.';






        }

        public void NextToken()
        {
            value.Length = 0;
            type = TokenType.None;
            do
            {
                startpos = mPos;
                switch (mCurChar)
                {
                    case default(char):
                        {
                            type = TokenType.EndOfFormula;
                            break;
                        }

                    case var @case when '0' <= @case && @case <= '9':
                        {
                            ParseNumber();
                            break;
                        }

                    case '-':
                    case '–':
                        {
                            NextChar();
                            type = TokenType.OperatorMinus;
                            break;
                        }

                    case '+':
                        {
                            NextChar();
                            type = TokenType.OperatorPlus;
                            break;
                        }

                    case '*':
                        {
                            NextChar();
                            type = TokenType.OperatorMul;
                            break;
                        }

                    case '/':
                        {
                            NextChar();
                            type = TokenType.OperatorDiv;
                            break;
                        }

                    case '%':
                        {
                            NextChar();
                            type = TokenType.OperatorPercent;
                            break;
                        }

                    case '(':
                        {
                            NextChar();
                            type = TokenType.OpenParenthesis;
                            break;
                        }

                    case ')':
                        {
                            NextChar();
                            type = TokenType.CloseParenthesis;
                            break;
                        }

                    case '<':
                        {
                            NextChar();
                            if (mCurChar == '=')
                            {
                                NextChar();
                                type = TokenType.OperatorLe;
                            }
                            else if (mCurChar == '>')
                            {
                                NextChar();
                                type = TokenType.OperatorNe;
                            }
                            else
                            {
                                type = TokenType.OperatorLt;
                            }

                            break;
                        }

                    case '>':
                        {
                            NextChar();
                            if (mCurChar == '=')
                            {
                                NextChar();
                                type = TokenType.OperatorGe;
                            }
                            else
                            {
                                type = TokenType.OperatorGt;
                            }

                            break;
                        }

                    case ',':
                        {
                            NextChar();
                            type = TokenType.Comma;
                            break;
                        }

                    case '=':
                        {
                            NextChar();
                            type = TokenType.OperatorEq;
                            break;
                        }

                    case '.':
                        {
                            NextChar();
                            type = TokenType.Dot;
                            break;
                        }

                    case '\'':
                    case '"':
                        {
                            ParseString(true);
                            type = TokenType.ValueString;
                            break;
                        }

                    case '#':
                        {
                            ParseDate();
                            break;
                        }

                    case '&':
                        {
                            NextChar();
                            type = TokenType.OperatorConcat;
                            break;
                        }

                    case '[':
                        {
                            NextChar();
                            type = TokenType.OpenBracket;
                            break;
                        }

                    case ']':
                        {
                            NextChar();
                            type = TokenType.CloseBracket;
                            break;
                        }
                    // do nothing
                    case var case1 when '\0' <= case1 && case1 <= ' ':
                        {
                            break;
                        }

                    default:
                        {
                            ParseIdentifier();
                            break;
                        }
                }

                if (type != TokenType.None)
                    break;
                NextChar();
            }
            while (true);
        }

        private void NextChar()
        {
            if (mPos < mLen)
            {
                mCurChar = mString[mPos];
                if (mCurChar == '\u0093' | mCurChar == '\u0094')
                {
                    mCurChar = '"';
                }

                if (mCurChar == '\u0091' | mCurChar == '\u0092')
                {
                    mCurChar = '\'';
                }

                mPos += 1;
            }
            else
            {
                mCurChar = default;
            }
        }

        private void ParseNumber()
        {
            type = TokenType.ValueNumber;
            while (mCurChar >= '0' & mCurChar <= '9')
            {
                value.Append(mCurChar);
                NextChar();
            }

            if (mCurChar == '.')
            {
                value.Append(mCurChar);
                NextChar();
                while (mCurChar >= '0' & mCurChar <= '9')
                {
                    value.Append(mCurChar);
                    NextChar();
                }
            }
        }

        private void ParseIdentifier()
        {
            while (mCurChar >= '0' & mCurChar <= '9' | mCurChar >= 'a' & mCurChar <= 'z' | mCurChar >= 'A' & mCurChar <= 'Z' | mCurChar >= 'A' & mCurChar <= 'Z' | mCurChar >= '\u0080' | mCurChar == '_')




            {
                value.Append(mCurChar);
                NextChar();
            }

            switch (value.ToString() ?? "")
            {
                case "and":
                    {
                        type = TokenType.OperatorAnd;
                        break;
                    }

                case "or":
                    {
                        type = TokenType.OperatorOr;
                        break;
                    }

                case "not":
                    {
                        type = TokenType.OperatorNot;
                        break;
                    }

                case "true":
                case "yes":
                    {
                        type = TokenType.ValueTrue;
                        break;
                    }

                case "if":
                    {
                        type = TokenType.OperatorIf;
                        break;
                    }

                case "false":
                case "no":
                    {
                        type = TokenType.ValueFalse;
                        break;
                    }

                default:
                    {
                        type = TokenType.ValueIdentifier;
                        break;
                    }
            }
        }

        private void ParseString(bool InQuote)
        {
            char OriginalChar = default(char);
            if (InQuote)
            {
                OriginalChar = mCurChar;
                NextChar();
            }

            char PreviousChar;
            while (mCurChar != default(char))
            {
                if (InQuote && mCurChar == OriginalChar)
                {
                    NextChar();
                    if (mCurChar == OriginalChar)
                    {
                        value.Append(mCurChar);
                    }
                    else
                    {
                        // End of String
                        return;
                    }
                }
                else if (mCurChar == '%')
                {
                    NextChar();
                    if (mCurChar == '[')
                    {
                        NextChar();
                        StringBuilder SaveValue = value;
                        int SaveStartPos = startpos;
                        value = new System.Text.StringBuilder();
                        NextToken(); // restart the tokenizer for the subExpr
                        object subExpr = default(object);
                        try
                        {
                            // subExpr = mParser.ParseExpr(0, ePriority.none)
                            if (subExpr is null)
                            {
                                value.Append("<nothing>");
                            }
                            else
                            {
                                value.Append(Evaluator.ConvertToString(subExpr));
                            }
                        }
                        catch (Exception ex)
                        {
                            // XML don't like < and >
                            value.Append("[Error " + ex.Message + "]");
                        }

                        SaveValue.Append(value.ToString());
                        value = SaveValue;
                        startpos = SaveStartPos;
                    }
                    else
                    {
                        value.Append('%');
                    }
                }
                else
                {
                    value.Append(mCurChar);
                    NextChar();
                }
            }

            if (InQuote)
            {
                RaiseError("Incomplete string, missing " + OriginalChar + "; String started");
            }
        }

        private void ParseDate()
        {
            NextChar(); // eat the #
            int zone = 0;
            while (mCurChar >= '0' & mCurChar <= '9' | mCurChar == '/' | mCurChar == ':' | mCurChar == ' ')
            {
                value.Append(mCurChar);
                NextChar();
            }

            if (mCurChar.ToString() != "#")
            {
                RaiseError("Invalid date should be #dd/mm/yyyy#");
            }
            else
            {
                NextChar();
            }

            type = TokenType.ValueDate;
        }
    }
}