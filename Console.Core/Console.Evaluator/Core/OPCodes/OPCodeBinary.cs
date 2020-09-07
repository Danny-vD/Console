using System;
using System.Runtime.CompilerServices;

using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// OPCode that is Returned when the OPCode does Resolve to a Binary Result(Boolean)
    /// </summary>
    internal class OPCodeBinary : OPCode
    {

        /// <summary>
        /// The Evaluation Type Backing Field
        /// </summary>
        private readonly EvalType mEvalType;

        /// <summary>
        /// The Backing Field First Inner OPCode Parameter
        /// </summary>
        private OPCode _param1;

        /// <summary>
        /// The Backing Field Second Inner OPCode Parameter
        /// </summary>
        private OPCode _param2;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="tokenizer">The Tokenizer Instance</param>
        /// <param name="param1">First Parameter</param>
        /// <param name="tt">The Token Type</param>
        /// <param name="param2">Second Parameter</param>
        public OPCodeBinary(Tokenizer tokenizer, OPCode param1, TokenType tt, OPCode param2)
        {
            Param1 = param1;
            Param2 = param2;
            EvalType v1Type = Param1.EvalType;
            EvalType v2Type = Param2.EvalType;
            switch (tt)
            {
                case TokenType.OperatorPlus:
                {
                    if ((v1Type == EvalType.Number) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = NUM_PLUS_NUM;
                        mEvalType = EvalType.Number;
                    }
                    else if ((v1Type == EvalType.Number) & (v2Type == EvalType.Date))
                    {
                        OPCode argParam1 = Param1;
                        OPCode argParam2 = Param2;
                        SwapParams(ref argParam1, ref argParam2);
                        Param1 = argParam1;
                        Param2 = argParam2;
                        mValueDelegate = DATE_PLUS_NUM;
                        mEvalType = EvalType.Date;
                    }
                    else if ((v1Type == EvalType.Date) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = DATE_PLUS_NUM;
                        mEvalType = EvalType.Date;
                    }
                    else if (Param1.CanReturn(EvalType.String) & Param2.CanReturn(EvalType.String))
                    {
                        Convert(tokenizer, ref param1, EvalType.String);
                        mValueDelegate = STR_CONCAT_STR;
                        mEvalType = EvalType.String;
                    }

                    break;
                }

                case TokenType.OperatorMinus:
                {
                    if ((v1Type == EvalType.Number) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = NUM_MINUS_NUM;
                        mEvalType = EvalType.Number;
                    }
                    else if ((v1Type == EvalType.Date) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = DATE_MINUS_NUM;
                        mEvalType = EvalType.Date;
                    }
                    else if ((v1Type == EvalType.Date) & (v2Type == EvalType.Date))
                    {
                        mValueDelegate = DATE_MINUS_DATE;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

                case TokenType.OperatorMul:
                {
                    if ((v1Type == EvalType.Number) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = NUM_MUL_NUM;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

                case TokenType.OperatorDiv:
                {
                    if ((v1Type == EvalType.Number) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = NUM_DIV_NUM;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

                case TokenType.OperatorPercent:
                {
                    if ((v1Type == EvalType.Number) & (v2Type == EvalType.Number))
                    {
                        mValueDelegate = NUM_PERCENT_NUM;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

                case TokenType.OperatorAnd:
                case TokenType.OperatorOr:
                {
                    OPCode argparam1 = Param1;
                    Convert(tokenizer, ref argparam1, EvalType.Boolean);
                    Param1 = argparam1;
                    OPCode argparam11 = Param2;
                    Convert(tokenizer, ref argparam11, EvalType.Boolean);
                    Param2 = argparam11;
                    switch (tt)
                    {
                        case TokenType.OperatorOr:
                        {
                            mValueDelegate = BOOL_OR_BOOL;
                            mEvalType = EvalType.Boolean;
                            break;
                        }

                        case TokenType.OperatorAnd:
                        {
                            mValueDelegate = BOOL_AND_BOOL;
                            mEvalType = EvalType.Boolean;
                            break;
                        }
                    }

                    break;
                }

                case TokenType.OperatorNe:
                {
                    if (v1Type == EvalType.Boolean && v2Type == EvalType.Boolean)
                    {
                        mValueDelegate = BOOL_NE_BOOL;
                        mEvalType = EvalType.Boolean;
                    }
                    else if (v1Type == EvalType.Number && v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_NE_NUM;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorGt:
                {
                    if (v1Type == EvalType.Number && v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_GT_NUM;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorGe:
                {
                    if (v1Type == EvalType.Number && v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_GE_NUM;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorEq:
                {
                    if (v1Type == EvalType.Boolean && v2Type == EvalType.Boolean)
                    {
                        mValueDelegate = BOOL_EQ_BOOL;
                        mEvalType = EvalType.Boolean;
                    }
                    else if (v1Type == EvalType.Number && v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_EQ_NUM;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorLe:
                {
                    if (v1Type == EvalType.Number && v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_LE_NUM;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorLt:
                {
                    if (v1Type == EvalType.Number && v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_LT_NUM;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }
            }

            if (mValueDelegate is null)
            {
                tokenizer.RaiseError(
                                     "Cannot apply the operator " +
                                     tt.ToString().Replace("operator_", "") +
                                     " on " +
                                     v1Type +
                                     " and " +
                                     v2Type
                                    );
            }
        }

        /// <summary>
        /// The First Inner OPCode Parameter
        /// </summary>
        private OPCode Param1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _param1;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_param1 != null)
                {
                    _param1.ValueChanged -= Param1ValueChanged;
                }

                _param1 = value;
                if (_param1 != null)
                {
                    _param1.ValueChanged += Param1ValueChanged;
                }
            }
        }

        /// <summary>
        /// The Second Inner OPCode Parameter
        /// </summary>
        private OPCode Param2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _param2;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_param2 != null)
                {
                    _param2.ValueChanged -= Param2_ValueChanged;
                }

                _param2 = value;
                if (_param2 != null)
                {
                    _param2.ValueChanged += Param2_ValueChanged;
                }
            }
        }

        /// <summary>
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mEvalType;

        /// <summary>
        /// Returns Param1 AND Param2
        /// </summary>
        /// <returns>True if both Values are True</returns>
        private object BOOL_AND_BOOL()
        {
            return (bool) Param1.Value & (bool) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 OR Param2
        /// </summary>
        /// <returns>True if at least one Value is True</returns>
        private object BOOL_OR_BOOL()
        {
            return (bool) Param1.Value | (bool) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 XOR Param2
        /// </summary>
        /// <returns>True if only one value True</returns>
        private object BOOL_XOR_BOOL()
        {
            return (bool) Param1.Value ^ (bool) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 == Param2
        /// </summary>
        /// <returns>True if both values are equal</returns>
        private object BOOL_EQ_BOOL()
        {
            return (bool) Param1.Value == (bool) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 != Param2
        /// </summary>
        /// <returns>True if both values are not equal</returns>
        private object BOOL_NE_BOOL()
        {
            return (bool) Param1.Value != (bool) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 == Param2 (DOUBLE)
        /// </summary>
        /// <returns>True if both values are equal</returns>
        private object NUM_EQ_NUM()
        {
            return (double) Param1.Value == (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 &lt; Param2 (DOUBLE)
        /// </summary>
        /// <returns>True if the first value is smaller than the second value</returns>
        private object NUM_LT_NUM()
        {
            return (double) Param1.Value < (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 &gt; Param2 (DOUBLE)
        /// </summary>
        /// <returns>True if the first value is greater than the second value</returns>
        private object NUM_GT_NUM()
        {
            return (double) Param1.Value > (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 >= Param2 (DOUBLE)
        /// </summary>
        /// <returns>True if both the first value is greater than or equal to the second value</returns>
        private object NUM_GE_NUM()
        {
            return (double) Param1.Value >= (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 &lt;= Param2 (DOUBLE)
        /// </summary>
        /// <returns>True if both the first value is lower than or equal to the second value</returns>
        private object NUM_LE_NUM()
        {
            return (double) Param1.Value <= (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 != Param2 (DOUBLE)
        /// </summary>
        /// <returns>True if both the first value is not equal to the second value</returns>
        private object NUM_NE_NUM()
        {
            return (double) Param1.Value != (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 + Param2 (DOUBLE)
        /// </summary>
        /// <returns>The Addition of the first and second value</returns>
        private object NUM_PLUS_NUM()
        {
            return (double) Param1.Value + (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 * Param2 (DOUBLE)
        /// </summary>
        /// <returns>The Multiplication of the first and second value</returns>
        private object NUM_MUL_NUM()
        {
            return (double) Param1.Value * (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 - Param2 (DOUBLE)
        /// </summary>
        /// <returns>The Subtraction of the first and second value</returns>
        private object NUM_MINUS_NUM()
        {
            return (double) Param1.Value - (double) Param2.Value;
        }


        /// <summary>
        /// Returns Param1 + Param2 (DATETIME)
        /// </summary>
        /// <returns>The Addition of the first and second value</returns>
        private object DATE_PLUS_NUM()
        {
            return ((DateTime) Param1.Value).AddDays((double) Param2.Value);
        }

        /// <summary>
        /// Returns Param1 - Param2 (DATETIME)
        /// </summary>
        /// <returns>The Subtraction of the first and second value</returns>
        private object DATE_MINUS_DATE()
        {
            return ((DateTime) Param1.Value).Subtract((DateTime) Param2.Value).TotalDays;
        }

        /// <summary>
        /// Returns Param1(DATETIME) - Param2 (DOUBLE)
        /// </summary>
        /// <returns>The Subtraction of the first and second value</returns>
        private object DATE_MINUS_NUM()
        {
            return ((DateTime) Param1.Value).AddDays(-(double) Param2.Value);
        }

        /// <summary>
        /// Returns the Concatenated Result of two string values
        /// </summary>
        /// <returns>Concatenated String</returns>
        private object STR_CONCAT_STR()
        {
            return Param1.Value + Param2.Value.ToString();
        }

        /// <summary>
        /// Returns Param1 / Param2 (DOUBLE)
        /// </summary>
        /// <returns>The Division of the first and second value</returns>
        private object NUM_DIV_NUM()
        {
            return (double) Param1.Value / (double) Param2.Value;
        }

        /// <summary>
        /// Returns Param1 * (Param2/100) (DOUBLE)
        /// </summary>
        /// <returns>The Percentage of the First Value</returns>
        private object NUM_PERCENT_NUM()
        {
            return (double) Param2.Value * ((double) Param1.Value / 100);
        }

        /// <summary>
        /// Gets Invoked when the first value was changed
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void Param1ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

        /// <summary>
        /// Gets Invoked when the second value was changed
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void Param2_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

    }
}