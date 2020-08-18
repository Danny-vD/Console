using System;
using System.Collections;
using System.Reflection;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator.Core
{
    internal class parser
    {
        private tokenizer mTokenizer;
        private Evaluator mEvaluator;

        public parser(Evaluator evaluator)
        {
            mEvaluator = evaluator;
        }

        public OPCode Parse(string str)
        {
            if (str is null)
                str = string.Empty;
            mTokenizer = new tokenizer(this, str);
            mTokenizer.NextToken();
            OPCode res = ParseExpr(null, Priority.None);
            if (mTokenizer.type == TokenType.EndOfFormula)
            {
                if (res is null)
                    res = new OPCodeImmediate(EvalType.String, string.Empty);
                return res;
            }
            else
            {
                mTokenizer.RaiseUnexpectedToken();
            }

            return default;
        }

        private OPCode ParseExpr(OPCode Acc, Priority priority)
        {
            OPCode ValueLeft = default, valueRight;
            do
            {
                switch (mTokenizer.type)
                {
                    case TokenType.OperatorMinus:
                        {
                            // unary minus operator
                            mTokenizer.NextToken();
                            ValueLeft = ParseExpr(null, Priority.Unaryminus);
                            ValueLeft = new OPCodeUnary(TokenType.OperatorMinus, ValueLeft);
                            break;
                        }

                    case TokenType.OperatorPlus:
                        {
                            // unary minus operator
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.OperatorNot:
                        {
                            mTokenizer.NextToken();
                            ValueLeft = ParseExpr(null, Priority.Not);
                            ValueLeft = new OPCodeUnary(TokenType.OperatorNot, ValueLeft);
                            break;
                        }

                    case TokenType.ValueIdentifier:
                        {
                            ParseIdentifier(ref ValueLeft);
                            break;
                        }

                    case TokenType.ValueTrue:
                        {
                            ValueLeft = new OPCodeImmediate(EvalType.Boolean, true);
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueFalse:
                        {
                            ValueLeft = new OPCodeImmediate(EvalType.Boolean, false);
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueString:
                        {
                            ValueLeft = new OPCodeImmediate(EvalType.String, mTokenizer.value.ToString());
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueNumber:
                        {
                            try
                            {
                                ValueLeft = new OPCodeImmediate(EvalType.Number, double.Parse(mTokenizer.value.ToString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture));


                            }
                            catch (Exception ex)
                            {
                                mTokenizer.RaiseError(string.Format("Invalid number {0}", mTokenizer.value.ToString()));
                            }

                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueDate:
                        {
                            try
                            {
                                ValueLeft = new OPCodeImmediate(EvalType.Date, mTokenizer.value.ToString());
                            }
                            catch (Exception ex)
                            {
                                mTokenizer.RaiseError(string.Format("Invalid date {0}, it should be #DD/MM/YYYY hh:mm:ss#", mTokenizer.value.ToString()));
                            }

                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.OpenParenthesis:
                        {
                            mTokenizer.NextToken();
                            ValueLeft = ParseExpr(null, Priority.None);
                            if (mTokenizer.type == TokenType.CloseParenthesis)
                            {
                                // good we eat the end parenthesis and continue ...
                                mTokenizer.NextToken();
                                break;
                            }
                            else
                            {
                                mTokenizer.RaiseUnexpectedToken("End parenthesis not found");
                            }

                            break;
                        }

                    case TokenType.OperatorIf:
                        {
                            // first check functions
                            ArrayList parameters = new ArrayList();  // parameters... 
                            mTokenizer.NextToken();
                            bool argbrackets = false;
                            parameters = ParseParameters(ref argbrackets);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                break;
            }
            while (true);
            if (ValueLeft is null)
            {
                mTokenizer.RaiseUnexpectedToken("No Expression found");
            }

            ParseDot(ref ValueLeft);
            do
            {
                TokenType tt;
                tt = mTokenizer.type;
                switch (tt)
                {
                    case TokenType.EndOfFormula:
                        {
                            // end of line
                            return ValueLeft;
                        }

                    case TokenType.ValueNumber:
                        {
                            mTokenizer.RaiseUnexpectedToken("Unexpected number without previous opterator");
                            break;
                        }

                    case TokenType.OperatorPlus:
                        {
                            if (priority < Priority.Plusminus)
                            {
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.Plusminus);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorMinus:
                        {
                            if (priority < Priority.Plusminus)
                            {
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.Plusminus);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorConcat:
                        {
                            if (priority < Priority.Concat)
                            {
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.Concat);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorMul:
                    case TokenType.OperatorDiv:
                        {
                            if (priority < Priority.Muldiv)
                            {
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.Muldiv);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorPercent:
                        {
                            if (priority < Priority.Percent)
                            {
                                mTokenizer.NextToken();
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, Acc);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorOr:
                        {
                            if (priority < Priority.Or)
                            {
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.Or);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorAnd:
                        {
                            if (priority < Priority.And)
                            {
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.And);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    case TokenType.OperatorNe:
                    case TokenType.OperatorGt:
                    case TokenType.OperatorGe:
                    case TokenType.OperatorEq:
                    case TokenType.OperatorLe:
                    case TokenType.OperatorLt:
                        {
                            if (priority < Priority.Equality)
                            {
                                tt = mTokenizer.type;
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(ValueLeft, Priority.Equality);
                                ValueLeft = new OPCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                            }
                            else
                            {
                                break;
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                break;
            }
            while (true);
            return ValueLeft;
        }

        [Flags()]
        private enum eCallType
        {
            field = 1,
            method = 2,
            property = 4,
            all = 7
        }

        private bool EmitCallFunction(ref OPCode valueLeft, string funcName, ArrayList parameters, eCallType CallType, bool ErrorIfNotFound)
        {
            OPCode newOpcode = default(OPCode);
            if (valueLeft is null)
            {
                for (int i = 0; i < mEvaluator.mEnvironmentFunctionsList.Count; i++)
                {
                    object functions = mEvaluator.mEnvironmentFunctionsList[i];
                    while (functions != null)
                    {
                        newOpcode = GetLocalFunction(functions, functions.GetType(), funcName, parameters, CallType);
                        if (newOpcode != null)
                            break;
                        if (functions is IEvalFunctions evalFunctions)
                        {
                            functions = evalFunctions.InheritedFunctions();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                newOpcode = GetLocalFunction(valueLeft, valueLeft.SystemType, funcName, parameters, CallType);
            }

            if (newOpcode is object)
            {
                valueLeft = newOpcode;
                return true;
            }
            else
            {
                if (ErrorIfNotFound)
                    mTokenizer.RaiseError("Variable or method " + funcName + " was not found");
                return false;
            }
        }

        private OPCode GetLocalFunction(object @base, Type baseType, string funcName, ArrayList parameters, eCallType CallType)
        {
            MemberInfo mi;
            IEvalTypedValue var;
            mi = GetMemberInfo(baseType, funcName, parameters);
            if (mi != null)
            {
                switch (mi.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            if ((CallType & eCallType.field) == 0)
                                mTokenizer.RaiseError("Unexpected Field");
                            break;
                        }

                    case MemberTypes.Method:
                        {
                            if ((CallType & eCallType.method) == 0)
                                mTokenizer.RaiseError("Unexpected Method");
                            break;
                        }

                    case MemberTypes.Property:
                        {
                            if ((CallType & eCallType.property) == 0)
                                mTokenizer.RaiseError("Unexpected Property");
                            break;
                        }

                    default:
                        {
                            mTokenizer.RaiseUnexpectedToken(mi.MemberType.ToString() + " members are not supported");
                            break;
                        }
                }

                return OPCodeCallMethod.GetNew(mTokenizer, @base, mi, parameters);
            }

            if (@base is IVariableBag)
            {
                IEvalTypedValue val = ((IVariableBag)@base).GetVariable(funcName);
                if (val != null)
                {
                    return new OPCodeGetVariable(val);
                }
            }

            return null;
        }

        private MemberInfo GetMemberInfo(Type objType, string func, ArrayList parameters)
        {
            BindingFlags bindingAttr;
            bindingAttr = BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Static;




            if (mEvaluator.CaseSensitive == false)
            {
                bindingAttr = bindingAttr | BindingFlags.IgnoreCase;
            }

            MemberInfo[] mis;
            if (func == null)
            {
                mis = objType.GetDefaultMembers();
            }
            else
            {
                mis = objType.GetMember(func, bindingAttr);
            }


            // There is a bit of cooking here...
            // lets find the most acceptable Member
            int Score, BestScore = default;
            MemberInfo BestMember = default(MemberInfo);
            ParameterInfo[] plist = default(ParameterInfo[]);
            int idx;
            MemberInfo mi;
            for (int i = 0, loopTo = mis.Length - 1; i <= loopTo; i++)
            {
                mi = mis[i];
                if (mi is MethodInfo)
                {
                    plist = ((MethodInfo)mi).GetParameters();
                }
                else if (mi is PropertyInfo)
                {
                    plist = ((PropertyInfo)mi).GetIndexParameters();
                }
                else if (mi is FieldInfo)
                {
                    plist = null;
                }

                Score = 10; // by default
                idx = 0;
                if (plist is null)
                    plist = new ParameterInfo[] { };
                if (parameters is null)
                    parameters = new ArrayList();
                ParameterInfo pi;
                if (parameters.Count > plist.Length)
                {
                    Score = 0;
                }
                else
                {
                    for (int index = 0, loopTo1 = plist.Length - 1; index <= loopTo1; index++)
                    {
                        pi = plist[index];
                        // For Each pi As Reflection.ParameterInfo In plist
                        if (idx < parameters.Count)
                        {
                            Score += ParamCompatibility(parameters[idx], pi.ParameterType);
                        }
                        else if (pi.IsOptional)
                        {
                            Score += 10;
                        }
                        else
                        {
                            // unknown parameter
                            Score = 0;
                        }

                        idx += 1;
                    }
                }

                if (Score > BestScore)
                {
                    BestScore = Score;
                    BestMember = mi;
                }
            }

            return BestMember;
        }

        private static int ParamCompatibility(object value, Type type)
        {
            // This function returns a score 1 to 10 to this question
            // Can this value fit into this type ?
            if (value is null)
            {
                if (ReferenceEquals(type, typeof(object)))
                    return 10;
                if (ReferenceEquals(type, typeof(string)))
                    return 8;
                return 5;
            }
            else if (ReferenceEquals(type, value.GetType()))
            {
                return 10;
            }
            else
            {
                return 5;
            }
        }

        private void ParseDot(ref OPCode ValueLeft)
        {
            do
            {
                switch (mTokenizer.type)
                {
                    case TokenType.Dot:
                        {
                            mTokenizer.NextToken();
                            break;
                        }
                    // fine this is either an array or a default property
                    case TokenType.OpenParenthesis:
                        {
                            break;
                        }

                    default:
                        {
                            return;
                        }
                }

                ParseIdentifier(ref ValueLeft);
            }
            while (true);
        }

        private void ParseIdentifier(ref OPCode ValueLeft)
        {
            // first check functions
            ArrayList parameters;   // parameters... 
                                    // Dim types As New ArrayList
            string func = mTokenizer.value.ToString();
            mTokenizer.NextToken();
            bool isBrackets = default(bool);
            parameters = ParseParameters(ref isBrackets);
            if (parameters != null)
            {
                ArrayList EmptyParameters = new ArrayList();
                bool ParamsNotUsed = default(bool);
                if (mEvaluator.Syntax == ParserSyntax.VisualBasic)
                {
                    // in vb we don't know if it is array or not as we have only parenthesis
                    // so we try with parameters first
                    if (!EmitCallFunction(ref ValueLeft, func, parameters, eCallType.all, ErrorIfNotFound: false))
                    {
                        // and if not found we try as array or default member
                        EmitCallFunction(ref ValueLeft, func, EmptyParameters, eCallType.all, ErrorIfNotFound: true);
                        ParamsNotUsed = true;
                    }
                }
                else if (isBrackets)
                {
                    if (!EmitCallFunction(ref ValueLeft, func, parameters, eCallType.property, ErrorIfNotFound: false))
                    {
                        EmitCallFunction(ref ValueLeft, func, EmptyParameters, eCallType.all, ErrorIfNotFound: true);
                        ParamsNotUsed = true;
                    }
                }
                else if (!EmitCallFunction(ref ValueLeft, func, parameters, eCallType.field | eCallType.method, ErrorIfNotFound: false))
                {
                    EmitCallFunction(ref ValueLeft, func, EmptyParameters, eCallType.all, ErrorIfNotFound: true);
                    ParamsNotUsed = true;
                }
                // we found a function without parameters 
                // so our parameters must be default property or an array
                Type t = ValueLeft.SystemType;
                if (ParamsNotUsed)
                {
                    if (t.IsArray)
                    {
                        if (parameters.Count == t.GetArrayRank())
                        {
                            ValueLeft = new OPCodeGetArrayEntry(ValueLeft, parameters);
                        }
                        else
                        {
                            mTokenizer.RaiseError("This array has " + t.GetArrayRank() + " dimensions");
                        }
                    }
                    else
                    {
                        MemberInfo mi;
                        mi = GetMemberInfo(t, null, parameters);
                        if (!(mi is null))
                        {
                            ValueLeft = OPCodeCallMethod.GetNew(mTokenizer, ValueLeft, mi, parameters);
                        }
                        else
                        {
                            mTokenizer.RaiseError("Parameters not supported here");
                        }
                    }
                }
            }
            else
            {
                EmitCallFunction(ref ValueLeft, func, parameters, eCallType.all, ErrorIfNotFound: true);
            }
        }

        private ArrayList ParseParameters(ref bool brackets)
        {
            ArrayList parameters = default(ArrayList);
            OPCode valueleft;
            TokenType lClosing = default(TokenType);
            if (mTokenizer.type == TokenType.OpenParenthesis || mTokenizer.type == TokenType.OpenBracket & mEvaluator.Syntax == ParserSyntax.CSharp)
            {
                switch (mTokenizer.type)
                {
                    case TokenType.OpenBracket:
                        {
                            lClosing = TokenType.CloseBracket;
                            brackets = true;
                            break;
                        }

                    case TokenType.OpenParenthesis:
                        {
                            lClosing = TokenType.CloseParenthesis;
                            break;
                        }
                }

                parameters = new ArrayList();
                mTokenizer.NextToken(); // eat the parenthesis
                do
                {
                    if (mTokenizer.type == lClosing)
                    {
                        // good we eat the end parenthesis and continue ...
                        mTokenizer.NextToken();
                        break;
                    }

                    valueleft = ParseExpr(null, Priority.None);
                    parameters.Add(valueleft);
                    if (mTokenizer.type == lClosing)
                    {
                        // good we eat the end parenthesis and continue ...
                        mTokenizer.NextToken();
                        break;
                    }
                    else if (mTokenizer.type == TokenType.Comma)
                    {
                        mTokenizer.NextToken();
                    }
                    else
                    {
                        mTokenizer.RaiseUnexpectedToken(lClosing.ToString() + " not found");
                    }
                }
                while (true);
            }

            return parameters;
        }
    }
}