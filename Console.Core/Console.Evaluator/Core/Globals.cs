using System;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Core
{
    /// <summary>
    /// Implements Static Helper Functions
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Returns true if the two stings are equal
        /// </summary>
        /// <param name="v1">First String</param>
        /// <param name="v2">Second String</param>
        /// <returns></returns>
        internal static bool VarEq(string v1, string v2)
        {
            int lv1, lv2;
            if (v1 is null)
                lv1 = 0;
            else
                lv1 = v1.Length;
            if (v2 is null)
                lv2 = 0;
            else
                lv2 = v2.Length;
            if (lv1 != lv2)
                return false;
            if (lv1 == 0)
                return true;
            char c1, c2;
            for (int i = 0, loopTo = lv1 - 1; i <= loopTo; i++)
            {
                c1 = v1[i];
                c2 = v2[i];
                switch (c1)
                {
                    case var @case when 'a' <= @case && @case <= 'z':
                        {
                            if (c2 != c1 && c2 != c1 - 32)
                            {
                                return false;
                            }

                            break;
                        }

                    case var case1 when 'A' <= case1 && case1 <= 'Z':
                        {
                            if (c2 != c1 && c2 != c1 + 32)
                            {
                                return false;
                            }

                            break;
                        }

                    case '-':
                    case '_':
                    case '.':
                        {
                            if (c2 != c1 && c2 != '_' && c2 != '.')
                            {
                                return false;
                            }

                            break;
                        }

                    case var case2 when case2 == '_':
                        {
                            if (c2 != c1 && c2 != '-')
                            {
                                return false;
                            }

                            break;
                        }

                    default:
                        {
                            if (c2 != c1)
                                return false;
                            break;
                        }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the EvalType of the Object
        /// </summary>
        /// <param name="o">Object Instance</param>
        /// <returns>The EvalType of the Object</returns>
        internal static EvalType GetObjectType(object o)
        {
            if (o is null)
            {
                return EvalType.Unknown;
            }
            else
            {
                Type t = o.GetType();
                return GetEvalType(t);
            }
        }

        /// <summary>
        /// Returns the EvalType of the specified Type
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>The Eval Type for this Type</returns>
        internal static EvalType GetEvalType(Type t)
        {
            if (ReferenceEquals(t, typeof(float)) | ReferenceEquals(t, typeof(double)) | ReferenceEquals(t, typeof(decimal)) | ReferenceEquals(t, typeof(short)) | ReferenceEquals(t, typeof(int)) | ReferenceEquals(t, typeof(long)) | ReferenceEquals(t, typeof(byte)) | ReferenceEquals(t, typeof(ushort)) | ReferenceEquals(t, typeof(uint)) | ReferenceEquals(t, typeof(ulong)))
            {
                return EvalType.Number;
            }
            else if (ReferenceEquals(t, typeof(DateTime)))
            {
                return EvalType.Date;
            }
            else if (ReferenceEquals(t, typeof(bool)))
            {
                return EvalType.Boolean;
            }
            else if (ReferenceEquals(t, typeof(string)))
            {
                return EvalType.String;
            }
            else
            {
                return EvalType.Object;
            }
        }

        /// <summary>
        /// Returns the System Type from the Specified EvalType
        /// </summary>
        /// <param name="t">The Eval Type</param>
        /// <returns>System Type associated to the EvalType</returns>
        internal static Type GetSystemType(EvalType t)
        {
            switch (t)
            {
                case EvalType.Boolean:
                    {
                        return typeof(bool);
                    }

                case EvalType.Date:
                    {
                        return typeof(DateTime);
                    }

                case EvalType.Number:
                    {
                        return typeof(double);
                    }

                case EvalType.String:
                    {
                        return typeof(string);
                    }

                default:
                    {
                        return typeof(object);
                    }
            }
        }

        /// <summary>
        /// Returns the Boolean Value from the IEvalTypedValue Object
        /// </summary>
        /// <param name="o">The IEvalTypedValue Object</param>
        /// <returns>Boolean Value</returns>
        public static bool Bool(IEvalTypedValue o)
        {
            return Convert.ToBoolean(o.Value);
        }

        /// <summary>
        /// Returns the DateTime Value from the IEvalTypedValue Object
        /// </summary>
        /// <param name="o">The IEvalTypedValue Object</param>
        /// <returns>DateTime Value</returns>
        public static DateTime Date(IEvalTypedValue o)
        {
            return Convert.ToDateTime(o.Value);
        }

        /// <summary>
        /// Returns the Number Value from the IEvalTypedValue Object
        /// </summary>
        /// <param name="o">The IEvalTypedValue Object</param>
        /// <returns>Number Value</returns>
        public static double Num(IEvalTypedValue o)
        {
            return Convert.ToDouble(o.Value);
        }

        /// <summary>
        /// Returns the String Value from the IEvalTypedValue Object
        /// </summary>
        /// <param name="o">The IEvalTypedValue Object</param>
        /// <returns>String Value</returns>
        public static string Str(IEvalTypedValue o)
        {
            return Convert.ToString(o.Value);
        }
    }
}