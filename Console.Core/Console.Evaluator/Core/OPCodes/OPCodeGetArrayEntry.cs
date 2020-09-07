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
        /// The Parameters of the Array Indexer
        /// </summary>
        private readonly IEvalTypedValue[] mParams;

        /// <summary>
        /// The Result Evaluation Type Backing Field
        /// </summary>
        private readonly EvalType mResultEvalType;

        /// <summary>
        /// The Result System Type Backing Field
        /// </summary>
        private readonly Type mResultSystemType;

        /// <summary>
        /// The Values of the Parameters
        /// </summary>
        private readonly int[] mValues;

        /// <summary>
        /// The Array Backing Field
        /// </summary>
        private OPCode _array;

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
            Array = array;
            mParams = newParams;
            mValues = newValues;
            mResultSystemType = array.SystemType.GetElementType();
            mResultEvalType = Globals.GetEvalType(mResultSystemType);
        }

        /// <summary>
        /// The Array
        /// </summary>
        private OPCode Array
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _array;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_array != null)
                {
                    _array.ValueChanged -= BaseVariableValueChanged;
                }

                _array = value;
                if (_array != null)
                {
                    _array.ValueChanged += BaseVariableValueChanged;
                }
            }
        }

        /// <summary>
        /// The Object in the array at the specified index.
        /// </summary>
        public override object Value
        {
            get
            {
                object res;
                Array arr = (Array) Array.Value;
                for (int i = 0, loopTo = mValues.Length - 1; i <= loopTo; i++)
                {
                    mValues[i] = System.Convert.ToInt32(mParams[i].Value);
                }

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
        private void BaseVariableValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

    }
}