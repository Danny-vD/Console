using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    internal class OPCodeUnary : OPCode
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

        private EvalType mEvalType;

        public OPCodeUnary(TokenType tt, OPCode param1)
        {
            mParam1 = param1;
            EvalType v1Type = mParam1.EvalType;
            switch (tt)
            {
                case TokenType.OperatorNot:
                {
                    if (v1Type == EvalType.Boolean)
                    {
                        mValueDelegate = BOOLEAN_NOT;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorMinus:
                {
                    if (v1Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_CHGSIGN;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }
            }
        }

        private object BOOLEAN_NOT()
        {
            return !(bool)mParam1.Value;
        }

        private object NUM_CHGSIGN()
        {
            return -(double)mParam1.Value;
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
    }
}