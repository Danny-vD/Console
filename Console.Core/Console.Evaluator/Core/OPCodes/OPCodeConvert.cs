using System;
using System.Runtime.CompilerServices;

using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

/// <summary>
/// Namespace of All OPCodes.
/// </summary>
namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Implements Conversions between different Types
    /// </summary>
    internal class OPCodeConvert : OPCode
    {

        /// <summary>
        /// The Evaluation Type of the Parameter Backing Field
        /// </summary>
        private readonly EvalType mEvalType = EvalType.Unknown;

        /// <summary>
        /// The First parameter Backing Field
        /// </summary>
        private IEvalTypedValue _param1;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="tokenizer">Tokenizer Instance</param>
        /// <param name="param1">The First Parameter</param>
        /// <param name="evalType">The Evaluator Type</param>
        public OPCodeConvert(Tokenizer tokenizer, IEvalTypedValue param1, EvalType evalType)
        {
            Param1 = param1;
            switch (evalType)
            {
                case EvalType.Boolean:
                {
                    mValueDelegate = TBool;
                    mEvalType = EvalType.Boolean;
                    break;
                }

                case EvalType.Date:
                {
                    mValueDelegate = TDate;
                    mEvalType = EvalType.Date;
                    break;
                }

                case EvalType.Number:
                {
                    mValueDelegate = TNum;
                    mEvalType = EvalType.Number;
                    break;
                }

                case EvalType.String:
                {
                    mValueDelegate = TStr;
                    mEvalType = EvalType.String;
                    break;
                }

                default:
                {
                    tokenizer.RaiseError(
                                         "Cannot convert " +
                                         param1.SystemType.Name +
                                         " to " +
                                         (int) evalType
                                        );
                    break;
                }
            }
        }

        /// <summary>
        /// The First Parameter
        /// </summary>
        private IEvalTypedValue Param1
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
        /// The Evaluation Type of the Parameter
        /// </summary>
        public override EvalType EvalType => mEvalType;

        /// <summary>
        /// returns the Parameter as Boolean
        /// </summary>
        /// <returns>Boolean Value</returns>
        private object TBool()
        {
            return Globals.Bool(Param1);
        }

        /// <summary>
        /// returns the Parameter as DateTime
        /// </summary>
        /// <returns>DateTime Value</returns>
        private object TDate()
        {
            return Globals.Date(Param1);
        }

        /// <summary>
        /// returns the Parameter as Double
        /// </summary>
        /// <returns>Double Value</returns>
        private object TNum()
        {
            return Globals.Num(Param1);
        }

        /// <summary>
        /// returns the Parameter as String
        /// </summary>
        /// <returns>String Value</returns>
        private object TStr()
        {
            return Globals.Str(Param1);
        }


        /// <summary>
        /// Gets Invoked when the parameter Value changes.
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void Param1ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

    }
}