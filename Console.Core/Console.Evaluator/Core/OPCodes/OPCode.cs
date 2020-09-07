using System;

using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Abstract OPCode Class.
    /// Does Implement IEvalTypedValue and IEvalHasDescription Interfaces
    /// </summary>
    public abstract class OPCode : IEvalTypedValue, IEvalHasDescription
    {

        /// <summary>
        /// The RunDelegate Definition
        /// </summary>
        public delegate void RunDelegate();

        /// <summary>
        /// The ValueDelegate used to Compute the Value
        /// </summary>
        protected ValueDelegate mValueDelegate;

        /// <summary>
        /// The Description of the OPCode
        /// </summary>
        public virtual string Description => "OPCode " + GetType().Name;

        /// <summary>
        /// The OPCode Name
        /// </summary>
        public virtual string Name => "OPCode " + GetType().Name;

        /// <summary>
        /// The Evaluation Type of the OPCode
        /// </summary>
        public abstract EvalType EvalType { get; }

        /// <summary>
        /// The Value of the OPCode.
        /// Invoked the ValueDelegate
        /// </summary>
        public virtual object Value => mValueDelegate();

        /// <summary>
        /// The System Type of the OPCode Value
        /// </summary>
        public virtual Type SystemType => Globals.GetSystemType(EvalType);

        /// <summary>
        /// ValueChanged EventHandler
        /// </summary>
        public event ValueChangedEventHandler ValueChanged;

        /// <summary>
        /// Raises the Event ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseEventValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Returns true if the OPCode is able to return a Value of the Specified Type
        /// </summary>
        /// <param name="type">The Type</param>
        /// <returns>True if the OPCode can return this Value Type</returns>
        public bool CanReturn(EvalType type)
        {
            return true;
        }

        /// <summary>
        /// Converts the Specified OPCode to the Specified EvalType
        /// </summary>
        /// <param name="tokenizer">The Tokenizer Instance</param>
        /// <param name="param1">The OPCode to Convert</param>
        /// <param name="evalType">Target Type</param>
        protected internal void Convert(Tokenizer tokenizer, ref OPCode param1, EvalType evalType)
        {
            if (param1.EvalType != evalType)
            {
                if (param1.CanReturn(evalType))
                {
                    param1 = new OPCodeConvert(tokenizer, param1, evalType);
                }
                else
                {
                    tokenizer.RaiseError("Cannot convert " + param1.Name + " into " + (int) evalType);
                }
            }
        }

        /// <summary>
        /// Converts an IEvalTypedValue to the Specified System Type
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="systemType"></param>
        protected static void ConvertToSystemType(ref IEvalTypedValue param1, Type systemType)
        {
            if (!ReferenceEquals(param1.SystemType, systemType))
            {
                if (ReferenceEquals(systemType, typeof(object)))
                {
                }

                // ignore
                else
                {
                    param1 = new OPCodeSystemTypeConvert(param1, systemType);
                }
            }
        }

        /// <summary>
        /// Swaps the two Parameters
        /// </summary>
        /// <param name="param1">New parameter 2</param>
        /// <param name="param2">New parameter 1</param>
        protected void SwapParams(ref OPCode param1, ref OPCode param2)
        {
            OPCode swp = param1;
            param1 = param2;
            param2 = swp;
        }

        /// <summary>
        /// The ValueDelegate Definition
        /// </summary>
        /// <returns></returns>
        protected delegate object ValueDelegate();

    }
}