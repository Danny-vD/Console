using System;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    public abstract class OPCode : IEvalTypedValue, IEvalHasDescription
    {
        protected ValueDelegate mValueDelegate;

        protected delegate object ValueDelegate();

        public delegate void RunDelegate();

        protected OPCode()
        {
        }

        protected void RaiseEventValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        public abstract EvalType EvalType { get; }

        public bool CanReturn(EvalType type)
        {
            return true;
        }

        public virtual string Description
        {
            get
            {
                return "OPCode " + GetType().Name;
            }
        }

        public virtual string Name
        {
            get
            {
                return "OPCode " + GetType().Name;
            }
        }

        public virtual object Value
        {
            get
            {
                return mValueDelegate();
            }
        }

        public virtual Type SystemType
        {
            get
            {
                return Globals.GetSystemType(EvalType);
            }
        }

        protected internal void Convert(tokenizer tokenizer, ref OPCode param1, EvalType EvalType)
        {
            if (param1.EvalType != EvalType)
            {
                if (param1.CanReturn(EvalType))
                {
                    param1 = new OPCodeConvert(tokenizer, param1, EvalType);
                }
                else
                {
                    tokenizer.RaiseError("Cannot convert " + param1.Name + " into " + ((int)EvalType).ToString());
                }
            }
        }

        protected static void ConvertToSystemType(ref IEvalTypedValue param1, Type SystemType)
        {
            if (!ReferenceEquals(param1.SystemType, SystemType))
            {
                if (ReferenceEquals(SystemType, typeof(object)))
                {
                }
                // ignore
                else
                {
                    param1 = new OPCodeSystemTypeConvert(param1, SystemType);
                }
            }
        }

        protected void SwapParams(ref OPCode Param1, ref OPCode Param2)
        {
            OPCode swp = Param1;
            Param1 = Param2;
            Param2 = swp;
        }

        public event ValueChangedEventHandler ValueChanged;
        
    }
}