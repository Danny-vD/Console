using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    /// <summary>
    /// The OPCode Implementation that gets used when the Tokenizer Parsed a Function/Variable Call
    /// </summary>
    public class OPCodeCallMethod : OPCode
    {
        /// <summary>
        /// The Object Instance Backing Field
        /// </summary>
        private object mBaseObject;
        /// <summary>
        /// The BaseObject System Type Backing Field
        /// </summary>
        private Type mBaseSystemType;
        
        /// <summary>
        /// The Base Value of the Call Method OPCode Backing Field
        /// </summary>
        private IEvalValue _mBaseValue;  // for the events only

        /// <summary>
        /// The Base Value of the Call Method OPCode
        /// </summary>
        private IEvalValue mBaseValue
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _mBaseValue;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mBaseValue != null)
                {
                    _mBaseValue.ValueChanged -= mBaseVariable_ValueChanged;
                }

                _mBaseValue = value;
                if (_mBaseValue != null)
                {
                    _mBaseValue.ValueChanged += mBaseVariable_ValueChanged;
                }
            }
        }
        /// <summary>
        /// The Base Value Object
        /// </summary>
        private object mBaseValueObject;
        /// <summary>
        /// The Member Info of the Call Method
        /// </summary>
        private System.Reflection.MemberInfo mMethod;
        /// <summary>
        /// The Parameters of the Call
        /// </summary>
        private IEvalTypedValue[] mParams;
        /// <summary>
        /// The Parameter Values of the Call
        /// </summary>
        private object[] mParamValues;
        /// <summary>
        /// The Call Return Type
        /// </summary>
        private Type mResultSystemType;
        /// <summary>
        /// The Evaluation Type of the Result
        /// </summary>
        private EvalType mResultEvalType;
        /// <summary>
        /// The Result Value Backing Field
        /// </summary>
        private IEvalValue _mResultValue;  // just for some

        /// <summary>
        /// The Result Value
        /// </summary>
        private IEvalValue mResultValue
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _mResultValue;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mResultValue != null)
                {
                    _mResultValue.ValueChanged -= mResultVariable_ValueChanged;
                }

                _mResultValue = value;
                if (_mResultValue != null)
                {
                    _mResultValue.ValueChanged += mResultVariable_ValueChanged;
                }
            }
        }
        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="baseObject">The Base Object Instance</param>
        /// <param name="method">The Member Info</param>
        /// <param name="params">The Parameters of the Call</param>
        internal OPCodeCallMethod(object baseObject, System.Reflection.MemberInfo method, IList @params)
        {
            if (@params is null)
                @params = new IEvalTypedValue[] { };
            IEvalTypedValue[] newParams = new IEvalTypedValue[@params.Count];
            object[] newParamValues = new object[@params.Count];
            @params.CopyTo(newParams, 0);
            foreach (IEvalTypedValue p in newParams)
                p.ValueChanged += mParamsValueChanged;
            mParams = newParams;
            mParamValues = newParamValues;
            mBaseObject = baseObject;
            mMethod = method;
            if (mBaseObject is IEvalValue)
            {
                if (mBaseObject is IEvalTypedValue)
                {
                    {
                        IEvalTypedValue withBlock = (IEvalTypedValue)mBaseObject;
                        mBaseSystemType = withBlock.SystemType;
                    }
                }
                else
                {
                    mBaseSystemType = mBaseObject.GetType();
                }
            }
            else
            {
                mBaseSystemType = mBaseObject.GetType();
            }

            ParameterInfo[] paramInfo = default(System.Reflection.ParameterInfo[]);
            if (method is System.Reflection.PropertyInfo)
            {
                {
                    PropertyInfo withBlock1 = (System.Reflection.PropertyInfo)method;
                    mResultSystemType = ((System.Reflection.PropertyInfo)method).PropertyType;
                    paramInfo = withBlock1.GetIndexParameters();
                }

                mValueDelegate = GetProperty;
            }
            else if (method is System.Reflection.MethodInfo)
            {
                {
                    MethodInfo withBlock2 = (System.Reflection.MethodInfo)method;
                    mResultSystemType = withBlock2.ReturnType;
                    paramInfo = withBlock2.GetParameters();
                }

                mValueDelegate = GetMethod;
            }
            else if (method is System.Reflection.FieldInfo)
            {
                {
                    FieldInfo withBlock3 = (System.Reflection.FieldInfo)method;
                    mResultSystemType = withBlock3.FieldType;
                    paramInfo = new System.Reflection.ParameterInfo[] { };
                }

                mValueDelegate = GetField;
            }

            for (int i = 0, loopTo = mParams.Length - 1; i <= loopTo; i++)
            {
                if (i < paramInfo.Length)
                {
                    ConvertToSystemType(ref mParams[i], paramInfo[i].ParameterType);
                }
            }

            if (typeof(IEvalValue).IsAssignableFrom(mResultSystemType))
            {
                mResultValue = (IEvalValue)InternalValue();
                if (mResultValue is IEvalTypedValue)
                {
                    {
                        IEvalTypedValue withBlock4 = (IEvalTypedValue)mResultValue;
                        mResultSystemType = withBlock4.SystemType;
                        mResultEvalType = withBlock4.EvalType;
                    }
                }
                else if (mResultValue is null)
                {
                    mResultSystemType = typeof(object);
                    mResultEvalType = EvalType.Object;
                }
                else
                {
                    object v = mResultValue.Value;
                    if (v is null)
                    {
                        mResultSystemType = typeof(object);
                        mResultEvalType = EvalType.Object;
                    }
                    else
                    {
                        mResultSystemType = v.GetType();
                        mResultEvalType = Globals.GetEvalType(mResultSystemType);
                    }
                }
            }
            else
            {
                mResultSystemType = SystemType;
                mResultEvalType = Globals.GetEvalType(SystemType);
            }
        }

        /// <summary>
        /// Creates a New OPCodeCallMethod Instance
        /// </summary>
        /// <param name="tokenizer">Tokenizer Instance</param>
        /// <param name="baseObject">The Base Object Instance</param>
        /// <param name="method">The Member Info</param>
        /// <param name="params">The Parameters</param>
        /// <returns>New OPCodeCallMethod Instance</returns>
        protected internal static OPCode GetNew(Tokenizer tokenizer, object baseObject, MemberInfo method, IList @params)
        {
            OPCode o;
            o = new OPCodeCallMethod(baseObject, method, @params);
            if (o.EvalType != EvalType.Object && !ReferenceEquals(o.SystemType, Globals.GetSystemType(o.EvalType)))
            {
                return new OPCodeConvert(tokenizer, o, o.EvalType);
            }
            else
            {
                return o;
            }
        }

        /// <summary>
        /// Returns the Value from the Member info when it is a PropertyInfo
        /// </summary>
        /// <returns>Value</returns>
        private object GetProperty()
        {
            object res = ((System.Reflection.PropertyInfo)mMethod).GetValue(mBaseValueObject, mParamValues);
            return res;
        }

        /// <summary>
        /// Returns the Value from the Member info when it is a MethodInfo
        /// </summary>
        /// <returns>Value</returns>
        private object GetMethod()
        {
            object res = ((System.Reflection.MethodInfo)mMethod).Invoke(mBaseValueObject, mParamValues);
            return res;
        }

        /// <summary>
        /// Returns the Value from the Member info when it is a FieldInfo
        /// </summary>
        /// <returns>Value</returns>
        private object GetField()
        {
            object res = ((System.Reflection.FieldInfo)mMethod).GetValue(mBaseValueObject);
            return res;
        }

        /// <summary>
        /// Returns the Value of the Call
        /// </summary>
        /// <returns>Value</returns>
        private object InternalValue()
        {
            for (int i = 0, loopTo = mParams.Length - 1; i <= loopTo; i++)
                mParamValues[i] = mParams[i].Value;
            if (mBaseObject is IEvalValue)
            {
                mBaseValue = (IEvalValue)mBaseObject;
                mBaseValueObject = mBaseValue.Value;
            }
            else
            {
                mBaseValueObject = mBaseObject;
            }

            return mValueDelegate();
        }

        /// <summary>
        /// Value Property.
        /// </summary>
        public override object Value
        {
            get
            {
                object res = InternalValue();
                if (res is IEvalValue)
                {
                    mResultValue = (IEvalValue)res;
                    res = mResultValue.Value;
                }

                return res;
            }
        }

        /// <summary>
        /// The Result System Type
        /// </summary>
        public override Type SystemType => mResultSystemType;

        /// <summary>
        /// The Result Evaluator Type
        /// </summary>
        public override EvalType EvalType => mResultEvalType;

        /// <summary>
        /// Gets Invoked when the paramerters to the function call changed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event Args</param>
        private void mParamsValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

        /// <summary>
        /// Gets Invoked when the baseVariable to the function call changed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event Args</param>
        private void mBaseVariable_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

        /// <summary>
        /// Gets Invoked when the resultVariable to the function call changed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event Args</param>
        private void mResultVariable_ValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }
    }
}