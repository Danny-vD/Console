using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Implements Conversions from System Type to System Type by using Reflection
    /// </summary>
    internal class OPCodeSystemTypeConvert : OPCode
    {
        /// <summary>
        /// The Parameter Backing Field
        /// </summary>
        private IEvalTypedValue _mParam1;

        /// <summary>
        /// The Parameter
        /// </summary>
        private IEvalTypedValue mParam1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _mParam1;

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
        /// <summary>
        /// The Evaluation Type Backing Field
        /// </summary>
        private EvalType mEvalType = EvalType.Unknown;
        /// <summary>
        /// The System Type Backing Field
        /// </summary>
        private Type mSystemType;

        public OPCodeSystemTypeConvert(IEvalTypedValue param1, Type type)
        {
            mParam1 = param1;
            mValueDelegate = CType;
            mSystemType = type;
            mEvalType = Globals.GetEvalType(type);
        }

        /// <summary>
        /// Converts the Parameter Value into the Specified System Type
        /// </summary>
        /// <returns>Parameter Value as Type mSystemType</returns>
        private object CType()
        {
            return System.Convert.ChangeType(mParam1.Value, mSystemType);
        }

        /// <summary>
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mEvalType;

        /// <summary>
        /// The System Type
        /// </summary>
        public override Type SystemType => mSystemType;

        /// <summary>
        /// Gets Invoked when the parameter value changes.
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void mParam1_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }
    }
}