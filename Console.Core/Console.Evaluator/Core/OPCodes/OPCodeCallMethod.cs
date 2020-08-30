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
        private readonly object _baseObject;

        /// <summary>
        /// The Base Value of the Call Method OPCode Backing Field
        /// </summary>
        private IEvalValue _baseValue; // for the events only

        /// <summary>
        /// The Base Value of the Call Method OPCode
        /// </summary>
        private IEvalValue BaseValue
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _baseValue;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_baseValue != null)
                {
                    _baseValue.ValueChanged -= BaseVariableValueChanged;
                }

                _baseValue = value;
                if (_baseValue != null)
                {
                    _baseValue.ValueChanged += BaseVariableValueChanged;
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
        private readonly MemberInfo mMethod;
        /// <summary>
        /// The Parameters of the Call
        /// </summary>
        private readonly IEvalTypedValue[] mParams;
        /// <summary>
        /// The Parameter Values of the Call
        /// </summary>
        private readonly object[] mParamValues;
        /// <summary>
        /// The Call Return Type
        /// </summary>
        private readonly Type mResultSystemType;
        /// <summary>
        /// The Evaluation Type of the Result
        /// </summary>
        private readonly EvalType _resultEvalType;
        /// <summary>
        /// The Result Value Backing Field
        /// </summary>
        private IEvalValue _resultValue; // just for some

        /// <summary>
        /// The Result Value
        /// </summary>
        private IEvalValue ResultValue
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _resultValue;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_resultValue != null)
                {
                    _resultValue.ValueChanged -= ResultVariableValueChanged;
                }

                _resultValue = value;
                if (_resultValue != null)
                {
                    _resultValue.ValueChanged += ResultVariableValueChanged;
                }
            }
        }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="baseObject">The Base Object Instance</param>
        /// <param name="method">The Member Info</param>
        /// <param name="params">The Parameters of the Call</param>
        internal OPCodeCallMethod(object baseObject, MemberInfo method, IList @params)
        {
            if (@params is null)
            {
                @params = new IEvalTypedValue[] { };
            }
            IEvalTypedValue[] newParams = new IEvalTypedValue[@params.Count];
            object[] newParamValues = new object[@params.Count];
            @params.CopyTo(newParams, 0);
            foreach (IEvalTypedValue p in newParams)
            {
                p.ValueChanged += ParamsValueChanged;
            }
            mParams = newParams;
            mParamValues = newParamValues;
            this._baseObject = baseObject;
            mMethod = method;
            if (this._baseObject is IEvalValue)
            {
                if (this._baseObject is IEvalTypedValue)
                {
                    {
                        IEvalTypedValue withBlock = (IEvalTypedValue) this._baseObject;
                    }
                }
                else
                {
                }
            }
            else
            {
            }

            ParameterInfo[] paramInfo = default;
            if (method is PropertyInfo)
            {
                {
                    PropertyInfo withBlock1 = (PropertyInfo) method;
                    mResultSystemType = ((PropertyInfo) method).PropertyType;
                    paramInfo = withBlock1.GetIndexParameters();
                }

                mValueDelegate = GetProperty;
            }
            else if (method is MethodInfo)
            {
                {
                    MethodInfo withBlock2 = (MethodInfo) method;
                    mResultSystemType = withBlock2.ReturnType;
                    paramInfo = withBlock2.GetParameters();
                }

                mValueDelegate = GetMethod;
            }
            else if (method is FieldInfo)
            {
                {
                    FieldInfo withBlock3 = (FieldInfo) method;
                    mResultSystemType = withBlock3.FieldType;
                    paramInfo = new ParameterInfo[] { };
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
                ResultValue = (IEvalValue) InternalValue();
                if (ResultValue is IEvalTypedValue)
                {
                    {
                        IEvalTypedValue withBlock4 = (IEvalTypedValue) ResultValue;
                        mResultSystemType = withBlock4.SystemType;
                        _resultEvalType = withBlock4.EvalType;
                    }
                }
                else if (ResultValue is null)
                {
                    mResultSystemType = typeof(object);
                    _resultEvalType = EvalType.Object;
                }
                else
                {
                    object v = ResultValue.Value;
                    if (v is null)
                    {
                        mResultSystemType = typeof(object);
                        _resultEvalType = EvalType.Object;
                    }
                    else
                    {
                        mResultSystemType = v.GetType();
                        _resultEvalType = Globals.GetEvalType(mResultSystemType);
                    }
                }
            }
            else
            {
                mResultSystemType = SystemType;
                _resultEvalType = Globals.GetEvalType(SystemType);
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
        protected internal static OPCode GetNew(Tokenizer tokenizer, object baseObject, MemberInfo method,
            IList @params)
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
            object res = ((PropertyInfo) mMethod).GetValue(mBaseValueObject, mParamValues);
            return res;
        }

        /// <summary>
        /// Returns the Value from the Member info when it is a MethodInfo
        /// </summary>
        /// <returns>Value</returns>
        private object GetMethod()
        {
            object res = ((MethodInfo) mMethod).Invoke(mBaseValueObject, mParamValues);
            return res;
        }

        /// <summary>
        /// Returns the Value from the Member info when it is a FieldInfo
        /// </summary>
        /// <returns>Value</returns>
        private object GetField()
        {
            object res = ((FieldInfo) mMethod).GetValue(mBaseValueObject);
            return res;
        }

        /// <summary>
        /// Returns the Value of the Call
        /// </summary>
        /// <returns>Value</returns>
        private object InternalValue()
        {
            for (int i = 0, loopTo = mParams.Length - 1; i <= loopTo; i++)
            {
                mParamValues[i] = mParams[i].Value;
            }
            if (_baseObject is IEvalValue)
            {
                BaseValue = (IEvalValue) _baseObject;
                mBaseValueObject = BaseValue.Value;
            }
            else
            {
                mBaseValueObject = _baseObject;
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
                    ResultValue = (IEvalValue) res;
                    res = ResultValue.Value;
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
        public override EvalType EvalType => _resultEvalType;

        /// <summary>
        /// Gets Invoked when the paramerters to the function call changed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event Args</param>
        private void ParamsValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

        /// <summary>
        /// Gets Invoked when the baseVariable to the function call changed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event Args</param>
        private void BaseVariableValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }

        /// <summary>
        /// Gets Invoked when the resultVariable to the function call changed.
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event Args</param>
        private void ResultVariableValueChanged(object sender, EventArgs e)
        {
            RaiseEventValueChanged(sender, e);
        }
    }
}