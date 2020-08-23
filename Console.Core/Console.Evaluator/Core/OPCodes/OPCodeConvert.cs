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
        /// The First parameter Backing Field
        /// </summary>
        private IEvalTypedValue _mParam1;
        
        /// <summary>
        /// The First Parameter
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
        /// The Evaluation Type of the Parameter Backing Field
        /// </summary>
        private EvalType mEvalType = EvalType.Unknown;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="tokenizer">Tokenizer Instance</param>
        /// <param name="param1">The First Parameter</param>
        /// <param name="evalType">The Evaluator Type</param>
        public OPCodeConvert(Tokenizer tokenizer, IEvalTypedValue param1, EvalType evalType)
        {
            mParam1 = param1;
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
                    tokenizer.RaiseError("Cannot convert " + param1.SystemType.Name + " to " + ((int)evalType).ToString());
                    break;
                }
            }
        }

        /// <summary>
        /// returns the Parameter as Boolean
        /// </summary>
        /// <returns>Boolean Value</returns>
        private object TBool()
        {
            return Globals.Bool(mParam1);
        }

        /// <summary>
        /// returns the Parameter as DateTime
        /// </summary>
        /// <returns>DateTime Value</returns>
        private object TDate()
        {
            return Globals.Date(mParam1);
        }

        /// <summary>
        /// returns the Parameter as Double
        /// </summary>
        /// <returns>Double Value</returns>
        private object TNum()
        {
            return Globals.Num(mParam1);
        }

        /// <summary>
        /// returns the Parameter as String
        /// </summary>
        /// <returns>String Value</returns>
        private object TStr()
        {
            return Globals.Str(mParam1);
        }


        /// <summary>
        /// The Evaluation Type of the Parameter
        /// </summary>
        public override EvalType EvalType => mEvalType;


        /// <summary>
        /// Gets Invoked when the parameter Value changes.
        /// </summary>
        /// <param name="sender">The Sender of the Event</param>
        /// <param name="e">The Event Args</param>
        private void mParam1_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }
    }
}