using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Implements Returning a Variable from a IEvalTypedValue interface
    /// </summary>
    public class OPCodeGetVariable : OPCode
    {
        /// <summary>
        /// The TypedValue Backing Field
        /// </summary>
        private IEvalTypedValue _mParam1;

        /// <summary>
        /// The TypedValue
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
        /// Public Constructor
        /// </summary>
        /// <param name="value">IEvalTypedValue to Get the Variable From</param>
        public OPCodeGetVariable(IEvalTypedValue value)
        {
            mParam1 = value;
        }

        /// <summary>
        /// The Variable Value
        /// </summary>
        public override object Value => mParam1.Value;

        /// <summary>
        /// The Value System Type
        /// </summary>
        public override Type SystemType => mParam1.SystemType;

        /// <summary>
        /// The Value Evaluator Type
        /// </summary>
        public override EvalType EvalType => mParam1.EvalType;

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