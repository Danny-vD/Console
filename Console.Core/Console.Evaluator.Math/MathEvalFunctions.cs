using Console.Evaluator.Core.Interfaces;

namespace Console.Evaluator.Math
{
    public class MathEvalFunctions : IEvalFunctions, IVariableBag
    {
        public IEvalFunctions InheritedFunctions() => null;

        public double PI = System.Math.PI;
        public double E = System.Math.E;

        public IEvalTypedValue GetVariable(string v)
        {
            return null;
        }

        public double Clamp(double a, double min, double max)
        {
            return a < min ? min : a > max ? max : a;
        }

        public double Clamp01(double a)
        {
            return Clamp(a, 0, 1);
        }

        public double Acos(double d)
        {
            return System.Math.Acos(d);
        }

        public double Asin(double d)
        {
            return System.Math.Asin(d);
        }

        public double Atan(double d)
        {
            return System.Math.Atan(d);
        }

        public double Atan2(double y, double x)
        {
            return System.Math.Atan2(y, x);
        }

        public double Ceiling(double a)
        {
            return System.Math.Ceiling(a);
        }

        public double Cos(double d)
        {
            return System.Math.Cos(d);
        }

        public double Cosh(double value)
        {
            return System.Math.Cosh(value);
        }

        public double Floor(double d)
        {
            return System.Math.Floor(d);
        }

        public double Sin(double a)
        {
            return System.Math.Sin(a);
        }

        public double Tan(double a)
        {
            return System.Math.Tan(a);
        }

        public double Sinh(double value)
        {
            return System.Math.Sinh(value);
        }

        public double Tanh(double value)
        {
            return System.Math.Tanh(value);
        }

        public double Round(double a)
        {
            return System.Math.Round(a);
        }

        public double Round(double a, int decimals)
        {
            return System.Math.Round(a, decimals);
        }

        public double Truncate(double d)
        {
            return System.Math.Truncate(d);
        }

        public double Sqrt(double d)
        {
            return System.Math.Sqrt(d);
        }

        public double Log(double d)
        {
            return System.Math.Log(d);
        }

        public double Log10(double d)
        {
            return System.Math.Log10(d);
        }

        public double Exp(double d)
        {
            return System.Math.Exp(d);
        }

        public double Pow(double x, double y)
        {
            return System.Math.Pow(x, y);
        }

        public double IEEERemainder(double x, double y)
        {
            return System.Math.IEEERemainder(x, y);
        }

        public double Abs(double value)
        {
            return System.Math.Abs(value);
        }

        public double Max(double val1, double val2)
        {
            return System.Math.Max(val1, val2);
        }

        public double Min(double val1, double val2)
        {
            return System.Math.Min(val1, val2);
        }

        public double Log(double a, double newBase)
        {
            return System.Math.Log(a, newBase);
        }

        public int IntPow(int basis, int exp)
        {
            if (exp == 0)
            {
                return 1;
            }

            int ret = basis;
            for (int i = 1; i < exp; i++)
            {
                ret *= basis;
            }

            return ret;
        }

        public double IntPow(double basis, int exp)
        {
            if (exp == 0)
            {
                return 1;
            }

            double ret = basis;
            for (int i = 1; i < exp; i++)
            {
                ret *= basis;
            }

            return ret;
        }
    }
}
