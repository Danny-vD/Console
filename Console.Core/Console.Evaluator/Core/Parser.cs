using System;
using System.Collections;
using System.Reflection;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;
using Console.Evaluator.Core.OPCodes;

/// <summary>
/// The Namespace of the Evaluator Core.
/// </summary>
namespace Console.Evaluator.Core
{

    /// <summary>
    /// The Internal Parser.
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// The Tokenizer that is used for the Parsing Process
        /// </summary>
        private Tokenizer mTokenizer;
        /// <summary>
        /// The Evaluator Instance
        /// </summary>
        private Evaluator mEvaluator;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="evaluator">The Evaluator Instance</param>
        public Parser(Evaluator evaluator)
        {
            mEvaluator = evaluator;
        }

        /// <summary>
        /// Parses an Expression
        /// </summary>
        /// <param name="str">The Expression</param>
        /// <returns>Parsed OpCode</returns>
        public OPCode Parse(string str)
        {
            if (str is null)
                str = string.Empty;
            mTokenizer = new Tokenizer(str);
            mTokenizer.NextToken();
            OPCode res = ParseExpr(null, Priority.None);
            if (mTokenizer.TokenType == TokenType.EndOfFormula)
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

        /// <summary>
        /// Parses an Expression from an 
        /// </summary>
        /// <param name="acc">The "Parent" OPCode</param>
        /// <param name="priority">Priority of the OPCode</param>
        /// <returns>The Parsed OPCode</returns>
        private OPCode ParseExpr(OPCode acc, Priority priority)
        {
            OPCode valueLeft = default, valueRight;
            do
            {
                switch (mTokenizer.TokenType)
                {
                    case TokenType.OperatorMinus:
                        {
                            // unary minus operator
                            mTokenizer.NextToken();
                            valueLeft = ParseExpr(null, Priority.Unaryminus);
                            valueLeft = new OPCodeUnary(TokenType.OperatorMinus, valueLeft);
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
                            valueLeft = ParseExpr(null, Priority.Not);
                            valueLeft = new OPCodeUnary(TokenType.OperatorNot, valueLeft);
                            break;
                        }

                    case TokenType.ValueIdentifier:
                        {
                            ParseIdentifier(ref valueLeft);
                            break;
                        }

                    case TokenType.ValueTrue:
                        {
                            valueLeft = new OPCodeImmediate(EvalType.Boolean, true);
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueFalse:
                        {
                            valueLeft = new OPCodeImmediate(EvalType.Boolean, false);
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueString:
                        {
                            valueLeft = new OPCodeImmediate(EvalType.String, mTokenizer.Value.ToString());
                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueNumber:
                        {
                            try
                            {
                                valueLeft = new OPCodeImmediate(EvalType.Number, double.Parse(mTokenizer.Value.ToString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture));


                            }
                            catch (Exception ex)
                            {
                                mTokenizer.RaiseError(string.Format("Invalid number {0}", mTokenizer.Value.ToString()));
                            }

                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.ValueDate:
                        {
                            try
                            {
                                valueLeft = new OPCodeImmediate(EvalType.Date, mTokenizer.Value.ToString());
                            }
                            catch (Exception ex)
                            {
                                mTokenizer.RaiseError(string.Format("Invalid date {0}, it should be #DD/MM/YYYY hh:mm:ss#", mTokenizer.Value.ToString()));
                            }

                            mTokenizer.NextToken();
                            break;
                        }

                    case TokenType.OpenParenthesis:
                        {
                            mTokenizer.NextToken();
                            valueLeft = ParseExpr(null, Priority.None);
                            if (mTokenizer.TokenType == TokenType.CloseParenthesis)
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
            if (valueLeft is null)
            {
                mTokenizer.RaiseUnexpectedToken("No Expression found");
            }

            ParseDot(ref valueLeft);
            do
            {
                TokenType tt;
                tt = mTokenizer.TokenType;
                switch (tt)
                {
                    case TokenType.EndOfFormula:
                        {
                            // end of line
                            return valueLeft;
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
                                valueRight = ParseExpr(valueLeft, Priority.Plusminus);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
                                valueRight = ParseExpr(valueLeft, Priority.Plusminus);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
                                valueRight = ParseExpr(valueLeft, Priority.Concat);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
                                valueRight = ParseExpr(valueLeft, Priority.Muldiv);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, acc);
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
                                valueRight = ParseExpr(valueLeft, Priority.Or);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
                                valueRight = ParseExpr(valueLeft, Priority.And);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
                                tt = mTokenizer.TokenType;
                                mTokenizer.NextToken();
                                valueRight = ParseExpr(valueLeft, Priority.Equality);
                                valueLeft = new OPCodeBinary(mTokenizer, valueLeft, tt, valueRight);
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
            return valueLeft;
        }


        /// <summary>
        /// Runs a Function by name
        /// </summary>
        /// <param name="valueLeft">The Left Side value of the Call</param>
        /// <param name="funcName">The Function Name</param>
        /// <param name="parameters">The Parameters used in the Function Call</param>
        /// <param name="callType">The CallType of the Method</param>
        /// <param name="errorIfNotFound">If True will throw an exception when the function with name funcName is not found</param>
        /// <returns></returns>
        private bool EmitCallFunction(ref OPCode valueLeft, string funcName, ArrayList parameters, CallType callType, bool errorIfNotFound)
        {
            OPCode newOpcode = default(OPCode);
            if (valueLeft is null)
            {
                for (int i = 0; i < mEvaluator.mEnvironmentFunctionsList.Count; i++)
                {
                    object functions = mEvaluator.mEnvironmentFunctionsList[i];
                    while (functions != null)
                    {
                        newOpcode = GetLocalFunction(functions, functions.GetType(), funcName, parameters, callType);
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
                newOpcode = GetLocalFunction(valueLeft, valueLeft.SystemType, funcName, parameters, callType);
            }

            if (newOpcode != null)
            {
                valueLeft = newOpcode;
                return true;
            }
            else
            {
                if (errorIfNotFound)
                    mTokenizer.RaiseError("Variable or method " + funcName + " was not found");
                return false;
            }
        }

        /// <summary>
        /// Returns the OPCodeCallMethod of the Object 
        /// </summary>
        /// <param name="base">The Object Instance</param>
        /// <param name="baseType">The Type of the Object</param>
        /// <param name="funcName">The Function Name</param>
        /// <param name="parameters">The Parameters used in the Function</param>
        /// <param name="callType">The CallType of the Function</param>
        /// <returns>The Local Function OPCode</returns>
        private OPCode GetLocalFunction(object @base, Type baseType, string funcName, ArrayList parameters, CallType callType)
        {
            MemberInfo mi;
            mi = GetMemberInfo(baseType, funcName, parameters);
            if (mi != null)
            {
                switch (mi.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            if ((callType & CallType.Field) == 0)
                                mTokenizer.RaiseError("Unexpected Field");
                            break;
                        }

                    case MemberTypes.Method:
                        {
                            if ((callType & CallType.Method) == 0)
                                mTokenizer.RaiseError("Unexpected Method");
                            break;
                        }

                    case MemberTypes.Property:
                        {
                            if ((callType & CallType.Property) == 0)
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


        /// <summary>
        /// Returns the Member Info of the Function inside the Type
        /// </summary>
        /// <param name="objType">The Type to Extract the MemberInfo From</param>
        /// <param name="func">The Function Name</param>
        /// <param name="parameters">The Parameter List</param>
        /// <returns>The Member Info of the Specified Function</returns>
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
            int score, bestScore = default;
            MemberInfo bestMember = default(MemberInfo);
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

                score = 10; // by default
                idx = 0;
                if (plist is null)
                    plist = new ParameterInfo[] { };
                if (parameters is null)
                    parameters = new ArrayList();
                ParameterInfo pi;
                if (parameters.Count > plist.Length)
                {
                    score = 0;
                }
                else
                {
                    for (int index = 0, loopTo1 = plist.Length - 1; index <= loopTo1; index++)
                    {
                        pi = plist[index];
                        // For Each pi As Reflection.ParameterInfo In plist
                        if (idx < parameters.Count)
                        {
                            score += ParamCompatibility(parameters[idx], pi.ParameterType);
                        }
                        else if (pi.IsOptional)
                        {
                            score += 10;
                        }
                        else
                        {
                            // unknown parameter
                            score = 0;
                        }

                        idx += 1;
                    }
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMember = mi;
                }
            }

            return bestMember;
        }


        /// <summary>
        /// This function returns a score 1 to 10 depending on how compatible the object instance and the type are
        /// </summary>
        /// <param name="value">Object Instance</param>
        /// <param name="type">The Type</param>
        /// <returns></returns>
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
            if (ReferenceEquals(type, value.GetType()))
            {
                return 10;
            }

            return 5;
        }

        /// <summary>
        /// Parses the "." From the specified OPCode.
        /// </summary>
        /// <param name="valueLeft">The OPCode</param>
        private void ParseDot(ref OPCode valueLeft)
        {
            do
            {
                switch (mTokenizer.TokenType)
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

                ParseIdentifier(ref valueLeft);
            }
            while (true);
        }

        /// <summary>
        /// Parses an Identifier of the Specified OPCode
        /// </summary>
        /// <param name="valueLeft">The OPCode</param>
        private void ParseIdentifier(ref OPCode valueLeft)
        {
            // first check functions
            ArrayList parameters;   // parameters... 
                                    // Dim types As New ArrayList
            string func = mTokenizer.Value.ToString();
            mTokenizer.NextToken();
            bool isBrackets = default(bool);
            parameters = ParseParameters(ref isBrackets);
            if (parameters != null)
            {
                ArrayList emptyParameters = new ArrayList();
                bool paramsNotUsed = default(bool);
                if (mEvaluator.Syntax == ParserSyntax.VisualBasic)
                {
                    // in vb we don't know if it is array or not as we have only parenthesis
                    // so we try with parameters first
                    if (!EmitCallFunction(ref valueLeft, func, parameters, CallType.All, false))
                    {
                        // and if not found we try as array or default member
                        EmitCallFunction(ref valueLeft, func, emptyParameters, CallType.All, true);
                        paramsNotUsed = true;
                    }
                }
                else if (isBrackets)
                {
                    if (!EmitCallFunction(ref valueLeft, func, parameters, CallType.Property, false))
                    {
                        EmitCallFunction(ref valueLeft, func, emptyParameters, CallType.All, true);
                        paramsNotUsed = true;
                    }
                }
                else if (!EmitCallFunction(ref valueLeft, func, parameters, CallType.Field | CallType.Method, false))
                {
                    EmitCallFunction(ref valueLeft, func, emptyParameters, CallType.All, true);
                    paramsNotUsed = true;
                }
                // we found a function without parameters 
                // so our parameters must be default property or an array
                Type t = valueLeft.SystemType;
                if (paramsNotUsed)
                {
                    if (t.IsArray)
                    {
                        if (parameters.Count == t.GetArrayRank())
                        {
                            valueLeft = new OPCodeGetArrayEntry(valueLeft, parameters);
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
                            valueLeft = OPCodeCallMethod.GetNew(mTokenizer, valueLeft, mi, parameters);
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
                EmitCallFunction(ref valueLeft, func, parameters, CallType.All, true);
            }
        }


        /// <summary>
        /// Parses the Parameters to a Function Call.
        /// </summary>
        /// <param name="brackets">Is True if the Parameters are specified in brackets</param>
        /// <returns>The List of Parameters</returns>
        private ArrayList ParseParameters(ref bool brackets)
        {
            ArrayList parameters = default(ArrayList);
            OPCode valueleft;
            TokenType lClosing = default(TokenType);
            if (mTokenizer.TokenType == TokenType.OpenParenthesis || (mTokenizer.TokenType == TokenType.OpenBracket) & (mEvaluator.Syntax == ParserSyntax.CSharp))
            {
                switch (mTokenizer.TokenType)
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
                    if (mTokenizer.TokenType == lClosing)
                    {
                        // good we eat the end parenthesis and continue ...
                        mTokenizer.NextToken();
                        break;
                    }

                    valueleft = ParseExpr(null, Priority.None);
                    parameters.Add(valueleft);
                    if (mTokenizer.TokenType == lClosing)
                    {
                        // good we eat the end parenthesis and continue ...
                        mTokenizer.NextToken();
                        break;
                    }
                    else if (mTokenizer.TokenType == TokenType.Comma)
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