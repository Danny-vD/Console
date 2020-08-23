using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Opcode that implements ArrayEntries
    /// </summary>
    public class OPCodeGetArrayEntry : OPCode
    {
        /// <summary>
        /// The Array Backing Field
        /// </summary>
        private OPCode _mArray;

        /// <summary>
        /// The Array
        /// </summary>
        private OPCode mArray
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _mArray;

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

        /// <summary>
        /// The Parameters of the Array Indexer
        /// </summary>
        private IEvalTypedValue[] mParams;
        /// <summary>
        /// The Values of the Parameters
        /// </summary>
        private int[] mValues;
        /// <summary>
        /// The Result Evaluation Type Backing Field
        /// </summary>
        private EvalType mResultEvalType;
        /// <summary>
        /// The Result System Type Backing Field
        /// </summary>
        private Type mResultSystemType;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="array">Array to Index</param>
        /// <param name="params">The Parameter of the Index Operation</param>
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

        /// <summary>
        /// The Object in the array at the specified index.
        /// </summary>
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
        /// <summary>
        /// The System Type
        /// </summary>
        public override Type SystemType => mResultSystemType;

        /// <summary>
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mResultEvalType;


        /// <summary>
        /// Gets Invoked when the baseVariable changes.
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void mBaseVariable_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }
    }
}