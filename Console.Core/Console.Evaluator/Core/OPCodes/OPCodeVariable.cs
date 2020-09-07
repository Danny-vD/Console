using System;
using System.Runtime.CompilerServices;

using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Implements the OPCode for returning a Value from an EvalVariable class.
    /// </summary>
    internal class OPCodeVariable : OPCode
    {

        /// <summary>
        /// The Variable Backing Field
        /// </summary>
        private EvalVariable _mVariable;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="variable">The Variable</param>
        public OPCodeVariable(EvalVariable variable)
        {
            mVariable = variable;
        }

        /// <summary>
        /// The Variable
        /// </summary>
        private EvalVariable mVariable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _mVariable;

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

        /// <summary>
        /// The Variable 
        /// </summary>
        public override object Value => mVariable;

        /// <summary>
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mVariable.EvalType;

        /// <summary>
        /// Gets Invoked when the variable value changes.
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void mVariable_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

    }
}