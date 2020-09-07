using System;

using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core
{
    /// <summary>
    /// Implements IEvalTypedValue and IEvalHasDescription
    /// Is Used as Possible Variable Provider.
    /// </summary>
    public class EvalVariable : IEvalTypedValue, IEvalHasDescription
    {

        /// <summary>
        /// The Backing Field Description of the Variable
        /// </summary>
        private readonly string mDescription;

        /// <summary>
        /// The Backing Field Evaluation Type of the Variable
        /// </summary>
        private readonly EvalType mEvalType;

        /// <summary>
        /// The Backing Field Variable Name
        /// </summary>
        private readonly string mName;

        /// <summary>
        /// The Backing Field System Type of the Variable
        /// </summary>
        private readonly Type mSystemType;

        /// <summary>
        /// The Value of the Variable
        /// </summary>
        private object mValue;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="name">Name of the Variable</param>
        /// <param name="originalValue">The Original Value of the Variable</param>
        /// <param name="description">The Variable Description</param>
        /// <param name="systemType">The Type of the Variable</param>
        public EvalVariable(string name, object originalValue, string description, Type systemType)
        {
            mName = name;
            mValue = originalValue;
            mDescription = description;
            mSystemType = systemType;
            mEvalType = Globals.GetEvalType(systemType);
        }

        /// <summary>
        /// The Description of the Variable
        /// </summary>
        public string Description => mDescription;

        /// <summary>
        /// The Variable Name
        /// </summary>
        public string Name => mName;

        /// <summary>
        /// The Evaluation Type of the Variable
        /// </summary>
        public EvalType EvalType => mEvalType;

        /// <summary>
        /// The System Type of the Variable
        /// </summary>
        public Type SystemType => mSystemType;

        /// <summary>
        /// The Value of the Variable
        /// </summary>
        public object Value
        {
            get => mValue;

            set
            {
                if (!ReferenceEquals(value, mValue))
                {
                    mValue = value;
                    ValueChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Event Handler that gets invoked when the Variable Value gets set.
        /// </summary>
        public event ValueChangedEventHandler ValueChanged;

    }
}