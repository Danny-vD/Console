using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.OPCodes
{
    internal class OPCodeImmediate : OPCode
    {
        private object mValue;
        private EvalType mEvalType;

        public OPCodeImmediate(EvalType EvalType, object value)
        {
            mEvalType = EvalType;
            mValue = value;
        }

        public override object Value
        {
            get
            {
                return mValue;
            }
        }

        public override EvalType EvalType
        {
            get
            {
                return mEvalType;
            }
        }
    }
}