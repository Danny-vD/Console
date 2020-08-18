using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    internal class OPCodeBinary : OPCode
    {
        private OPCode _mParam1;

        private OPCode mParam1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mParam1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mParam1 != null)
                {
                    _mParam1.ValueChanged -= mParam1_ValueChanged;
                }

                _mParam1 = value;
                if (_mParam1 != null)
                {
                    _mParam1.ValueChanged += mParam1_ValueChanged;
                }
            }
        }

        private OPCode _mParam2;

        private OPCode mParam2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mParam2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mParam2 != null)
                {
                    _mParam2.ValueChanged -= mParam2_ValueChanged;
                }

                _mParam2 = value;
                if (_mParam2 != null)
                {
                    _mParam2.ValueChanged += mParam2_ValueChanged;
                }
            }
        }

        private EvalType mEvalType;

        public OPCodeBinary(tokenizer tokenizer, OPCode param1, TokenType tt, OPCode param2)
        {
            mParam1 = param1;
            mParam2 = param2;
            EvalType v1Type = mParam1.EvalType;
            EvalType v2Type = mParam2.EvalType;
            switch (tt)
            {
                case TokenType.OperatorPlus:
                    {
                        if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                        {
                            mValueDelegate = NUM_PLUS_NUM;
                            mEvalType = EvalType.Number;
                        }
                        else if (v1Type == EvalType.Number & v2Type == EvalType.Date)
                        {
                            OPCode argParam1 = mParam1;
                            OPCode argParam2 = mParam2;
                            SwapParams(ref argParam1, ref argParam2);
                            mParam1 = argParam1;
                            mParam2 = argParam2;
                            mValueDelegate = DATE_PLUS_NUM;
                            mEvalType = EvalType.Date;
                        }
                        else if (v1Type == EvalType.Date & v2Type == EvalType.Number)
                        {
                            mValueDelegate = DATE_PLUS_NUM;
                            mEvalType = EvalType.Date;
                        }
                        else if (mParam1.CanReturn(EvalType.String) & mParam2.CanReturn(EvalType.String))
                        {
                            Convert(tokenizer, ref param1, EvalType.String);
                            mValueDelegate = STR_CONCAT_STR;
                            mEvalType = EvalType.String;
                        }

                        break;
                    }

                case TokenType.OperatorMinus:
                    {
                        if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                        {
                            mValueDelegate = NUM_MINUS_NUM;
                            mEvalType = EvalType.Number;
                        }
                        else if (v1Type == EvalType.Date & v2Type == EvalType.Number)
                        {
                            mValueDelegate = DATE_MINUS_NUM;
                            mEvalType = EvalType.Date;
                        }
                        else if (v1Type == EvalType.Date & v2Type == EvalType.Date)
                        {
                            mValueDelegate = DATE_MINUS_DATE;
                            mEvalType = EvalType.Number;
                        }

                        break;
                    }

                case TokenType.OperatorMul:
                    {
                        if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                        {
                            mValueDelegate = NUM_MUL_NUM;
                            mEvalType = EvalType.Number;
                        }

                        break;
                    }

                case TokenType.OperatorDiv:
                    {
                        if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                        {
                            mValueDelegate = NUM_DIV_NUM;
                            mEvalType = EvalType.Number;
                        }

                        break;
                    }

                case TokenType.OperatorPercent:
                    {
                        if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                        {
                            mValueDelegate = NUM_PERCENT_NUM;
                            mEvalType = EvalType.Number;
                        }

                        break;
                    }

                case TokenType.OperatorAnd:
                case TokenType.OperatorOr:
                    {
                        OPCode argparam1 = mParam1;
                        Convert(tokenizer, ref argparam1, EvalType.Boolean);
                        mParam1 = argparam1;
                        OPCode argparam11 = mParam2;
                        Convert(tokenizer, ref argparam11, EvalType.Boolean);
                        mParam2 = argparam11;
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
                tokenizer.RaiseError("Cannot apply the operator " + tt.ToString().Replace("operator_", "") + " on " + v1Type.ToString() + " and " + v2Type.ToString());


            }
        }

        private object BOOL_AND_BOOL()
        {
            return (bool)mParam1.Value & (bool)mParam2.Value;
        }

        private object BOOL_OR_BOOL()
        {
            return (bool)mParam1.Value | (bool)mParam2.Value;
        }

        private object BOOL_XOR_BOOL()
        {
            return (bool)mParam1.Value ^ (bool)mParam2.Value;
        }

        private object BOOL_EQ_BOOL()
        {
            return (bool)mParam1.Value == (bool)mParam2.Value;
        }

        private object BOOL_NE_BOOL()
        {
            return (bool)mParam1.Value != (bool)mParam2.Value;
        }

        private object NUM_EQ_NUM()
        {
            return (double)mParam1.Value == (double)mParam2.Value;
        }

        private object NUM_LT_NUM()
        {
            return (double)mParam1.Value < (double)mParam2.Value;
        }

        private object NUM_GT_NUM()
        {
            return (double)mParam1.Value > (double)mParam2.Value;
        }

        private object NUM_GE_NUM()
        {
            return (double)mParam1.Value >= (double)mParam2.Value;
        }

        private object NUM_LE_NUM()
        {
            return (double)mParam1.Value <= (double)mParam2.Value;
        }

        private object NUM_NE_NUM()
        {
            return (double)mParam1.Value != (double)mParam2.Value;
        }

        private object NUM_PLUS_NUM()
        {
            return (double)mParam1.Value + (double)mParam2.Value;
        }

        private object NUM_MUL_NUM()
        {
            return (double)mParam1.Value * (double)mParam2.Value;
        }

        private object NUM_MINUS_NUM()
        {
            return (double)mParam1.Value - (double)mParam2.Value;
        }

        private object DATE_PLUS_NUM()
        {
            return ((DateTime)mParam1.Value).AddDays((double)mParam2.Value);
        }

        private object DATE_MINUS_DATE()
        {
            return ((DateTime)mParam1.Value).Subtract((DateTime)mParam2.Value).TotalDays;
        }

        private object DATE_MINUS_NUM()
        {
            return ((DateTime)mParam1.Value).AddDays(-(double)mParam2.Value);
        }

        private object STR_CONCAT_STR()
        {
            return mParam1.Value.ToString() + mParam2.Value.ToString();
        }

        private object NUM_DIV_NUM()
        {
            return (double)mParam1.Value / (double)mParam2.Value;
        }

        private object NUM_PERCENT_NUM()
        {
            return (double)mParam2.Value * ((double)mParam1.Value / 100);
        }

        public override EvalType EvalType
        {
            get
            {
                return mEvalType;
            }
        }

        private void mParam1_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }

        private void mParam2_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }
    }
}