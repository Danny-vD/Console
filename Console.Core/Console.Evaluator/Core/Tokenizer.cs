using System;
using System.Text;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core
{

    /// <summary>
    /// The Tokenizer parses the Single Parts of the Expression to usable Tokens
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// The Current Expression
        /// </summary>
        private string mString;
        /// <summary>
        /// Length of mString
        /// </summary>
        private int mLen;
        /// <summary>
        /// The Current Position in the mString Expression
        /// </summary>
        private int mPos;
        /// <summary>
        /// The Current Character that is beeing tokenized
        /// </summary>
        private char mCurChar;
        /// <summary>
        /// The Start Position of the next Tokenization
        /// </summary>
        public int StartPos;
        /// <summary>
        /// The Current Token Type
        /// </summary>
        public TokenType TokenType;

        /// <summary>
        /// The Value of the Current Token
        /// </summary>
        public StringBuilder Value = new StringBuilder();

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="str">Expression</param>
        internal Tokenizer(string str)
        {
            mString = str;
            mLen = str.Length;
            mPos = 0;
            NextChar();   // start the machine
        }

        /// <summary>
        /// Raises an Exception 
        /// </summary>
        /// <param name="msg">The Error Message</param>
        /// <param name="ex">The Exception</param>
        internal void RaiseError(string msg, Exception ex = null)
        {
            if (ex is ParserException)
            {
                msg += ". " + ex.Message;
            }
            else
            {
                msg += " " + " at position " + StartPos;
                if (ex is object)
                {
                    msg += ". " + ex.Message;
                }
            }

            throw new ParserException(msg, mString, mPos);
        }

        /// <summary>
        /// Gets Invoked when the Tokenizer Encounters an Unexpected Token in the Expression
        /// </summary>
        /// <param name="msg">The Error Message</param>
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

            RaiseError(msg + "Unexpected " + TokenType.ToString().Replace('_', ' ') + " : " + Value.ToString());
        }

        /// <summary>
        /// Gets Invoked when the Tokenizer encounters an Unexpected Operator
        /// </summary>
        /// <param name="tt">The Token Type</param>
        /// <param name="valueLeft">The Value on the Left Side of the Operator</param>
        /// <param name="valueRight">The Value on the Rght Side of the Operator</param>
        /// <param name="msg">The Error Message</param>
        internal void RaiseWrongOperator(TokenType tt, object valueLeft, object valueRight, string msg = null)
        {
            if (msg.Length > 0)
            {
                msg.Replace("[op]", tt.GetType().ToString());
                msg += ". ";
            }

            msg = "Cannot apply the operator " + tt.ToString();
            if (valueLeft is null)
            {
                msg += " on nothing";
            }
            else
            {
                msg += " on a " + valueLeft.GetType().ToString();
            }

            if (valueRight != null)
            {
                msg += " and a " + valueRight.GetType().ToString();
            }

            RaiseError(msg);
        }

        /// <summary>
        /// Returns True if the current Character is an Operator
        /// </summary>
        /// <returns></returns>
        private bool IsOp()
        {
            return (mCurChar == '+') | (mCurChar == '-') | (mCurChar == '–') | (mCurChar == '%') | (mCurChar == '/') |
                   (mCurChar == '(') | (mCurChar == ')') | (mCurChar == '.');
        }

        /// <summary>
        /// Moves the Tokenizer to the Next Token
        /// </summary>
        public void NextToken()
        {
            Value.Length = 0;
            TokenType = TokenType.None;
            do
            {
                StartPos = mPos;
                switch (mCurChar)
                {
                    case default(char):
                        {
                            TokenType = TokenType.EndOfFormula;
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
                            TokenType = TokenType.OperatorMinus;
                            break;
                        }

                    case '+':
                        {
                            NextChar();
                            TokenType = TokenType.OperatorPlus;
                            break;
                        }

                    case '*':
                        {
                            NextChar();
                            TokenType = TokenType.OperatorMul;
                            break;
                        }

                    case '/':
                        {
                            NextChar();
                            TokenType = TokenType.OperatorDiv;
                            break;
                        }

                    case '%':
                        {
                            NextChar();
                            TokenType = TokenType.OperatorPercent;
                            break;
                        }

                    case '(':
                        {
                            NextChar();
                            TokenType = TokenType.OpenParenthesis;
                            break;
                        }

                    case ')':
                        {
                            NextChar();
                            TokenType = TokenType.CloseParenthesis;
                            break;
                        }

                    case '<':
                        {
                            NextChar();
                            if (mCurChar == '=')
                            {
                                NextChar();
                                TokenType = TokenType.OperatorLe;
                            }
                            else if (mCurChar == '>')
                            {
                                NextChar();
                                TokenType = TokenType.OperatorNe;
                            }
                            else
                            {
                                TokenType = TokenType.OperatorLt;
                            }

                            break;
                        }

                    case '>':
                        {
                            NextChar();
                            if (mCurChar == '=')
                            {
                                NextChar();
                                TokenType = TokenType.OperatorGe;
                            }
                            else
                            {
                                TokenType = TokenType.OperatorGt;
                            }

                            break;
                        }

                    case ',':
                        {
                            NextChar();
                            TokenType = TokenType.Comma;
                            break;
                        }

                    case '=':
                        {
                            NextChar();
                            TokenType = TokenType.OperatorEq;
                            break;
                        }

                    case '.':
                        {
                            NextChar();
                            TokenType = TokenType.Dot;
                            break;
                        }

                    case '\'':
                    case '"':
                        {
                            ParseString(true);
                            TokenType = TokenType.ValueString;
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
                            TokenType = TokenType.OperatorConcat;
                            break;
                        }

                    case '[':
                        {
                            NextChar();
                            TokenType = TokenType.OpenBracket;
                            break;
                        }

                    case ']':
                        {
                            NextChar();
                            TokenType = TokenType.CloseBracket;
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

                if (TokenType != TokenType.None)
                    break;
                NextChar();
            }
            while (true);
        }

        /// <summary>
        /// Moves the Tokenizer to the Next Character
        /// </summary>
        private void NextChar()
        {
            if (mPos < mLen)
            {
                mCurChar = mString[mPos];
                if ((mCurChar == '\u0093') | (mCurChar == '\u0094'))
                {
                    mCurChar = '"';
                }

                if ((mCurChar == '\u0091') | (mCurChar == '\u0092'))
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
        
        /// <summary>
        /// Parses a Number from the Current Character
        /// </summary>
        private void ParseNumber()
        {
            TokenType = TokenType.ValueNumber;
            while ((mCurChar >= '0') & (mCurChar <= '9'))
            {
                Value.Append(mCurChar);
                NextChar();
            }

            if (mCurChar == '.')
            {
                Value.Append(mCurChar);
                NextChar();
                while ((mCurChar >= '0') & (mCurChar <= '9'))
                {
                    Value.Append(mCurChar);
                    NextChar();
                }
            }
        }

        /// <summary>
        /// Parses an Identifier
        /// </summary>
        private void ParseIdentifier()
        {
            while (((mCurChar >= '0') & (mCurChar <= '9')) | ((mCurChar >= 'a') & (mCurChar <= 'z')) |
                   ((mCurChar >= 'A') & (mCurChar <= 'Z')) | ((mCurChar >= 'A') & (mCurChar <= 'Z')) |
                   (mCurChar >= '\u0080') | (mCurChar == '_')) 
            {
                Value.Append(mCurChar);
                NextChar();
            }

            switch (Value.ToString().ToLower() ?? "")
            {
                case "and":
                    {
                        TokenType = TokenType.OperatorAnd;
                        break;
                    }

                case "or":
                    {
                        TokenType = TokenType.OperatorOr;
                        break;
                    }

                case "not":
                    {
                        TokenType = TokenType.OperatorNot;
                        break;
                    }

                case "true":
                case "yes":
                    {
                        TokenType = TokenType.ValueTrue;
                        break;
                    }

                case "if":
                    {
                        TokenType = TokenType.OperatorIf;
                        break;
                    }

                case "false":
                case "no":
                    {
                        TokenType = TokenType.ValueFalse;
                        break;
                    }

                default:
                    {
                        TokenType = TokenType.ValueIdentifier;
                        break;
                    }
            }
        }

        /// <summary>
        /// Parses a String Value
        /// </summary>
        /// <param name="inQuote">True if the String is Enclosed in Quotation Marks</param>
        private void ParseString(bool inQuote)
        {
            char originalChar = default(char);
            if (inQuote)
            {
                originalChar = mCurChar;
                NextChar();
            }
            
            while (mCurChar != default(char))
            {
                if (inQuote && mCurChar == originalChar)
                {
                    NextChar();
                    if (mCurChar == originalChar)
                    {
                        Value.Append(mCurChar);
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
                        StringBuilder saveValue = Value;
                        int saveStartPos = StartPos;
                        Value = new System.Text.StringBuilder();
                        NextToken(); // restart the tokenizer for the subExpr
                        object subExpr = default(object);
                        try
                        {
                            // subExpr = mParser.ParseExpr(0, ePriority.none)
                            if (subExpr is null)
                            {
                                Value.Append("<nothing>");
                            }
                            else
                            {
                                Value.Append(Evaluator.ConvertToString(subExpr));
                            }
                        }
                        catch (Exception ex)
                        {
                            // XML don't like < and >
                            Value.Append("[Error " + ex.Message + "]");
                        }

                        saveValue.Append(Value.ToString());
                        Value = saveValue;
                        StartPos = saveStartPos;
                    }
                    else
                    {
                        Value.Append('%');
                    }
                }
                else
                {
                    Value.Append(mCurChar);
                    NextChar();
                }
            }

            if (inQuote)
            {
                RaiseError("Incomplete string, missing " + originalChar + "; String started");
            }
        }

        /// <summary>
        /// Parses a DateTime Value
        /// </summary>
        private void ParseDate()
        {
            NextChar(); // eat the #
            int zone = 0;
            while (((mCurChar >= '0') & (mCurChar <= '9')) | (mCurChar == '/') | (mCurChar == ':') | (mCurChar == ' '))
            {
                Value.Append(mCurChar);
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

            TokenType = TokenType.ValueDate;
        }
    }
}