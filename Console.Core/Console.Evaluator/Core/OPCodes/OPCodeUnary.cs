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
        private OPCode _param1;

        /// <summary>
        /// The Parameter
        /// </summary>
        private OPCode Param1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _param1;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_param1 != null)
                {
                    _param1.ValueChanged -= Param1ValueChanged;
                }

                _param1 = value;
                if (_param1 != null)
                {
                    _param1.ValueChanged += Param1ValueChanged;
                }
            }
        }

        /// <summary>
        /// The Evaluation Type Backing Field
        /// </summary>
        private readonly EvalType mEvalType;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="tt">The Token Type</param>
        /// <param name="param1">The Parameter</param>
        public OPCodeUnary(TokenType tt, OPCode param1)
        {
            Param1 = param1;
            EvalType v1Type = Param1.EvalType;
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
            return !(bool) Param1.Value;
        }

        /// <summary>
        /// Returns the Negative Value of the Parameter(Double)
        /// </summary>
        /// <returns>The Negative Value of the Parameter</returns>
        private object NUM_CHGSIGN()
        {
            return -(double) Param1.Value;
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
        private void Param1ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }
    }
}