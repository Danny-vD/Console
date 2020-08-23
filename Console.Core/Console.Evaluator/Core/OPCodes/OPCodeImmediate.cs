using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// Implements the Immediate Return OPCode that is used to directly return a known value.
    /// </summary>
    internal class OPCodeImmediate : OPCode
    {
        /// <summary>
        /// The Known Value Backing Field
        /// </summary>
        private object mValue;
        /// <summary>
        /// The Evaluation Type Backing Field
        /// </summary>
        private EvalType mEvalType;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="evalType">The Evaluation Type</param>
        /// <param name="value">The Known Value</param>
        public OPCodeImmediate(EvalType evalType, object value)
        {
            mEvalType = evalType;
            mValue = value;
        }

        /// <summary>
        /// The Known Value
        /// </summary>
        public override object Value => mValue;

        /// <summary>
        /// The Evaluation Type
        /// </summary>
        public override EvalType EvalType => mEvalType;
    }
}