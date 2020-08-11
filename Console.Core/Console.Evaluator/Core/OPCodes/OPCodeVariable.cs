using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    internal class OPCodeVariable : OPCode
    {
        private EvalVariable _mVariable;

        private EvalVariable mVariable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mVariable;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mVariable != null)
                {
                    _mVariable.ValueChanged -= mVariable_ValueChanged;
                }

                _mVariable = value;
                if (_mVariable != null)
                {
                    _mVariable.ValueChanged += mVariable_ValueChanged;
                }
            }
        }

        public OPCodeVariable(EvalVariable variable)
        {
            mVariable = variable;
        }

        public override object Value
        {
            get
            {
                return mVariable;
            }
        }

        public override EvalType EvalType
        {
            get
            {
                return mVariable.EvalType;
            }
        }

        private void mVariable_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }
    }
}