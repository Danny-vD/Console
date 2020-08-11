using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    internal class OPCodeConvert : OPCode
    {
        private IEvalTypedValue _mParam1;

        private IEvalTypedValue mParam1
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

        private EvalType mEvalType = EvalType.Unknown;

        public OPCodeConvert(tokenizer tokenizer, IEvalTypedValue param1, EvalType EvalType)
        {
            mParam1 = param1;
            switch (EvalType)
            {
                case EvalType.Boolean:
                {
                    mValueDelegate = TBool;
                    mEvalType = EvalType.Boolean;
                    break;
                }

                case EvalType.Date:
                {
                    mValueDelegate = TDate;
                    mEvalType = EvalType.Date;
                    break;
                }

                case EvalType.Number:
                {
                    mValueDelegate = TNum;
                    mEvalType = EvalType.Number;
                    break;
                }

                case EvalType.String:
                {
                    mValueDelegate = TStr;
                    mEvalType = EvalType.String;
                    break;
                }

                default:
                {
                    tokenizer.RaiseError("Cannot convert " + param1.SystemType.Name + " to " + ((int)EvalType).ToString());
                    break;
                }
            }
        }

        private object TBool()
        {
            return Globals.Bool(mParam1);
        }

        private object TDate()
        {
            return Globals.Date(mParam1);
        }

        private object TNum()
        {
            return Globals.Num(mParam1);
        }

        private object TStr()
        {
            return Globals.Str(mParam1);
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