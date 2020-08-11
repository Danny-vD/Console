using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    internal class OPCodeSystemTypeConvert : OPCode
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
        private Type mSystemType;

        public OPCodeSystemTypeConvert(IEvalTypedValue param1, Type Type)
        {
            mParam1 = param1;
            mValueDelegate = CType;
            mSystemType = Type;
            mEvalType = Globals.GetEvalType(Type);
        }

        private object CType()
        {
            return System.Convert.ChangeType(mParam1.Value, mSystemType);
        }

        public override EvalType EvalType
        {
            get
            {
                return mEvalType;
            }
        }

        public override Type SystemType
        {
            get
            {
                return mSystemType;
            }
        }

        private void mParam1_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }
    }
}