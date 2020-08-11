using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core.OPCodes
{
    public class OPCodeCallMethod : OPCode
    {
        private object mBaseObject;
        private Type mBaseSystemType;
        private EvalType mBaseEvalType;
        private IEvalValue _mBaseValue;  // for the events only

        private IEvalValue mBaseValue
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mBaseValue;
            }

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

        private object mBaseValueObject;
        private System.Reflection.MemberInfo mMethod;
        private IEvalTypedValue[] mParams;
        private object[] mParamValues;
        private Type mResultSystemType;
        private EvalType mResultEvalType;
        private IEvalValue _mResultValue;  // just for some

        private IEvalValue mResultValue
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mResultValue;
            }

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
                        mBaseEvalType = withBlock.EvalType;
                    }
                }
                else
                {
                    mBaseSystemType = mBaseObject.GetType();
                    mBaseEvalType = Globals.GetEvalType(mBaseSystemType);
                }
            }
            else
            {
                mBaseSystemType = mBaseObject.GetType();
                mBaseEvalType = Globals.GetEvalType(mBaseSystemType);
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

        protected internal static OPCode GetNew(tokenizer tokenizer, object baseObject, System.Reflection.MemberInfo method, IList @params)
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

        private object GetProperty()
        {
            object res = ((System.Reflection.PropertyInfo)mMethod).GetValue(mBaseValueObject, mParamValues);
            return res;
        }

        private object GetMethod()
        {
            object res = ((System.Reflection.MethodInfo)mMethod).Invoke(mBaseValueObject, mParamValues);
            return res;
        }

        private object GetField()
        {
            object res = ((System.Reflection.FieldInfo)mMethod).GetValue(mBaseValueObject);
            return res;
        }

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

        public override Type SystemType
        {
            get
            {
                return mResultSystemType;
            }
        }

        public override EvalType EvalType
        {
            get
            {
                return mResultEvalType;
            }
        }

        private void mParamsValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }

        private void mBaseVariable_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }

        private void mResultVariable_ValueChanged(object Sender, EventArgs e)
        {
            RaiseEventValueChanged(Sender, e);
        }
    }
}