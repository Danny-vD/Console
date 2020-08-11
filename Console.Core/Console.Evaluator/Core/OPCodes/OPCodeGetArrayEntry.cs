using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    public class OPCodeGetArrayEntry : OPCode
    {
        private OPCode _mArray;

        private OPCode mArray
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mArray;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mArray != null)
                {
                    _mArray.ValueChanged -= mBaseVariable_ValueChanged;
                }

                _mArray = value;
                if (_mArray != null)
                {
                    _mArray.ValueChanged += mBaseVariable_ValueChanged;
                }
            }
        }

        private IEvalTypedValue[] mParams;
        private int[] mValues;
        private EvalType mResultEvalType;
        private Type mResultSystemType;

        public OPCodeGetArrayEntry(OPCode array, IList @params)
        {
            IEvalTypedValue[] newParams = new IEvalTypedValue[@params.Count];
            int[] newValues = new int[@params.Count];
            @params.CopyTo(newParams, 0);
            mArray = array;
            mParams = newParams;
            mValues = newValues;
            mResultSystemType = array.SystemType.GetElementType();
            mResultEvalType = Globals.GetEvalType(mResultSystemType);
        }

        public override object Value
        {
            get
            {
                object res;
                Array arr = (Array)mArray.Value;
                for (int i = 0, loopTo = mValues.Length - 1; i <= loopTo; i++)
                    mValues[i] = System.Convert.ToInt32(mParams[i].Value);
                res = arr.GetValue(mValues);
                return res;
            }
        }

        public override Type SystemType
        {
            get
            {
                return mResultSystemType;
            }
        }

        public override EvalType EvalType
        {
            get
            {
                return mResultEvalType;
            }
        }

        private void mBaseVariable_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }
    }
}