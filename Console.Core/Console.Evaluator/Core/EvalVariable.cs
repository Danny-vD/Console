using System;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core
{
    public class EvalVariable : IEvalTypedValue, IEvalHasDescription
    {
        private object mValue;
        private string mDescription;
        private string mName;
        private Type mSystemType;
        private EvalType mEvalType;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public string Description
        {
            get
            {
                return mDescription;
            }
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }
        

        public EvalType EvalType
        {
            get
            {
                return mEvalType;
            }
        }

        public Type SystemType
        {
            get
            {
                return mSystemType;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public EvalVariable(string name, object originalValue, string description, Type systemType)
        {
            mName = name;
            mValue = originalValue;
            mDescription = description;
            mSystemType = systemType;
            mEvalType = Globals.GetEvalType(systemType);
        }

        public object Value
        {
            get
            {
                return mValue;
            }

            set
            {
                if (!ReferenceEquals(value, mValue))
                {
                    mValue = value;
                    ValueChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event ValueChangedEventHandler ValueChanged;
        
    }
}