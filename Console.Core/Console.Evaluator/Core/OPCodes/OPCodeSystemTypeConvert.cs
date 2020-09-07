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
        /// The Evaluation Type Backing Field
        /// </summary>
        private readonly EvalType mEvalType = EvalType.Unknown;

        /// <summary>
        /// The System Type Backing Field
        /// </summary>
        private readonly Type mSystemType;

        /// <summary>
        /// The Parameter Backing Field
        /// </summary>
        private IEvalTypedValue _param1;

        public OPCodeSystemTypeConvert(IEvalTypedValue param1, Type type)
        {
            Param1 = param1;
            mValueDelegate = CType;
            mSystemType = type;
            mEvalType = Globals.GetEvalType(type);
        }

        /// <summary>
        /// The Parameter
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
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mEvalType;

        /// <summary>
        /// The System Type
        /// </summary>
        public override Type SystemType => mSystemType;

        /// <summary>
        /// Converts the Parameter Value into the Specified System Type
        /// </summary>
        /// <returns>Parameter Value as Type mSystemType</returns>
        private object CType()
        {
            return System.Convert.ChangeType(Param1.Value, mSystemType);
        }

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