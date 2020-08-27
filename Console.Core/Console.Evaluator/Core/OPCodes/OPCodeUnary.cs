using System;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Implements Unary OPCodes like NOT and UNARY_MINUS
    /// </summary>
    internal class OPCodeUnary : OPCode
    {
        /// <summary>
        /// The Parameter Backing Field
        /// </summary>
        private OPCode _mParam1;

        /// <summary>
        /// The Parameter
        /// </summary>
        private OPCode mParam1
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
        private EvalType mEvalType;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="tt">The Token Type</param>
        /// <param name="param1">The Parameter</param>
        public OPCodeUnary(TokenType tt, OPCode param1)
        {
            mParam1 = param1;
            EvalType v1Type = mParam1.EvalType;
            switch (tt)
            {
                case TokenType.OperatorNot:
                {
                    if (v1Type == EvalType.Boolean)
                    {
                        mValueDelegate = BOOLEAN_NOT;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

                case TokenType.OperatorMinus:
                {
                    if (v1Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_CHGSIGN;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Returns the Inverse of the Parameter(Boolean)
        /// </summary>
        /// <returns>Inverse of the Parameter Value</returns>
        private object BOOLEAN_NOT()
        {
            return !(bool) mParam1.Value;
        }

        /// <summary>
        /// Returns the Negative Value of the Parameter(Double)
        /// </summary>
        /// <returns>The Negative Value of the Parameter</returns>
        private object NUM_CHGSIGN()
        {
            return -(double) mParam1.Value;
        }


        /// <summary>
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mEvalType;

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