using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    public class OPCodeGetVariable : OPCode
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

        public OPCodeGetVariable(IEvalTypedValue value)
        {
            mParam1 = value;
        }

        public override object Value
        {
            get
            {
                return mParam1.Value;
            }
        }

        public override Type SystemType
        {
            get
            {
                return mParam1.SystemType;
            }
        }

        public override EvalType EvalType
        {
            get
            {
                return mParam1.EvalType;
            }
        }

        private void mParam1_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }
    }
}