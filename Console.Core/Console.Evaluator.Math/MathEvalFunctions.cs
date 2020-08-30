using Console.Evaluator.Core.Interfaces;

/// <summary>
/// The Evaluator.Math extension provides the complete System.Math library to the evaluator.
/// This allows the Evaluator to Evaluate Expressions like: $eval(sin(PI))
/// </summary>
namespace Console.Evaluator.Math
{
    /// <summary>
    /// Implements the System.Math api as usable class for the Evaluator Extension
    /// </summary>
    public class MathEvalFunctions : IEvalFunctions, IVariableBag
    {
        /// <summary>
        /// Returns the Functions inherited from this class.
        /// </summary>
        /// <returns></returns>
        public IEvalFunctions InheritedFunctions()
        {
            return null;
        }

        /// <summary>
        /// Constant Value of PI
        /// </summary>
        public double PI = System.Math.PI;
        /// <summary>
        /// Constant Value of E
        /// </summary>
        public double E = System.Math.E;

        /// <summary>
        /// Returns a Variable with the specified Name
        /// </summary>
        /// <param name="v">Variable Name</param>
        /// <returns>Variable Value</returns>
        public IEvalTypedValue GetVariable(string v)
        {
            return null;
        }

        /// <summary>
        /// Ensures that a Number is between min and max.
        /// </summary>
        /// <param name="a">Value to Clamp</param>
        /// <param name="min">Minimal Value</param>
        /// <param name="max">Maximum Value</param>
        /// <returns>Clamped Value</returns>
        public double Clamp(double a, double min, double max)
        {
            return a < min ? min : a > max ? max : a;
        }

        /// <summary>
        /// Ensures that a Number is between 0 and 1.
        /// </summary>
        /// <param name="a">Value to Clamp</param>
        /// <returns>Clamped Value</returns>
        public double Clamp01(double a)
        {
            return Clamp(a, 0, 1);
        }

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        /// <param name="d">A number representing a cosine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
        /// <returns>An angle, θ, measured in radians, such that 0 ≤θ≤π   -or- System.Double.NaN if <paramref name="d">d</paramref> &lt; -1 or <paramref name="d">d</paramref> &gt; 1 or <paramref name="d">d</paramref> equals System.Double.NaN.</returns>
        public double Acos(double d)
        {
            return System.Math.Acos(d);
        }

        /// <summary>Returns the angle whose sine is the specified number.</summary>
        /// <param name="d">A number representing a sine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
        /// <returns>An angle, θ, measured in radians, such that -π/2 ≤θ≤π/2   -or-  System.Double.NaN if <paramref name="d">d</paramref> &lt; -1 or <paramref name="d">d</paramref> &gt; 1 or <paramref name="d">d</paramref> equals System.Double.NaN.</returns>
        public double Asin(double d)
        {
            return System.Math.Asin(d);
        }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        /// <param name="d">A number representing a tangent.</param>
        /// <returns>An angle, θ, measured in radians, such that -π/2 ≤θ≤π/2.   -or-  System.Double.NaN if <paramref name="d">d</paramref> equals System.Double.NaN, -π/2 rounded to double precision (-1.5707963267949) if <paramref name="d">d</paramref> equals System.Double.NegativeInfinity, or π/2 rounded to double precision (1.5707963267949) if <paramref name="d">d</paramref> equals System.Double.PositiveInfinity.</returns>
        public double Atan(double d)
        {
            return System.Math.Atan(d);
        }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>An angle, θ, measured in radians, such that -π≤θ≤π, and tan(θ) = <paramref name="y">y</paramref> / <paramref name="x">x</paramref>, where (<paramref name="x">x</paramref>, <paramref name="y">y</paramref>) is a point in the Cartesian plane. Observe the following:  For (<paramref name="x">x</paramref>, <paramref name="y">y</paramref>) in quadrant 1, 0 &lt; θ &lt; π/2.  For (<paramref name="x">x</paramref>, <paramref name="y">y</paramref>) in quadrant 2, π/2 &lt; θ≤π.  For (<paramref name="x">x</paramref>, <paramref name="y">y</paramref>) in quadrant 3, -π &lt; θ &lt; -π/2.  For (<paramref name="x">x</paramref>, <paramref name="y">y</paramref>) in quadrant 4, -π/2 &lt; θ &lt; 0.   For points on the boundaries of the quadrants, the return value is the following:  If y is 0 and x is not negative, θ = 0.  If y is 0 and x is negative, θ = π.  If y is positive and x is 0, θ = π/2.  If y is negative and x is 0, θ = -π/2.  If y is 0 and x is 0, θ = 0.   If <paramref name="x">x</paramref> or <paramref name="y">y</paramref> is System.Double.NaN, or if <paramref name="x">x</paramref> and <paramref name="y">y</paramref> are either System.Double.PositiveInfinity or System.Double.NegativeInfinity, the method returns System.Double.NaN.</returns>
        public double Atan2(double y, double x)
        {
            return System.Math.Atan2(y, x);
        }

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.</summary>
        /// <param name="a">A double-precision floating-point number.</param>
        /// <returns>The smallest integral value that is greater than or equal to <paramref name="a">a</paramref>. If <paramref name="a">a</paramref> is equal to System.Double.NaN, System.Double.NegativeInfinity, or System.Double.PositiveInfinity, that value is returned. Note that this method returns a T:System.Double instead of an integral type.</returns>
        public double Ceiling(double a)
        {
            return System.Math.Ceiling(a);
        }

        /// <summary>Returns the cosine of the specified angle.</summary>
        /// <param name="d">An angle, measured in radians.</param>
        /// <returns>The cosine of <paramref name="d">d</paramref>. If <paramref name="d">d</paramref> is equal to System.Double.NaN, System.Double.NegativeInfinity, or System.Double.PositiveInfinity, this method returns System.Double.NaN.</returns>
        public double Cos(double d)
        {
            return System.Math.Cos(d);
        }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic cosine of <paramref name="value">value</paramref>. If <paramref name="value">value</paramref> is equal to System.Double.NegativeInfinity or System.Double.PositiveInfinity, System.Double.PositiveInfinity is returned. If <paramref name="value">value</paramref> is equal to System.Double.NaN, System.Double.NaN is returned.</returns>
        public double Cosh(double value)
        {
            return System.Math.Cosh(value);
        }
        /// <summary>Returns the largest integer less than or equal to the specified double-precision floating-point number.</summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>The largest integer less than or equal to <paramref name="d">d</paramref>. If <paramref name="d">d</paramref> is equal to System.Double.NaN, System.Double.NegativeInfinity, or System.Double.PositiveInfinity, that value is returned.</returns>
        public double Floor(double d)
        {
            return System.Math.Floor(d);
        }

        /// <summary>Returns the sine of the specified angle.</summary>
        /// <param name="a">An angle, measured in radians.</param>
        /// <returns>The sine of <paramref name="a">a</paramref>. If <paramref name="a">a</paramref> is equal to System.Double.NaN, System.Double.NegativeInfinity, or System.Double.PositiveInfinity, this method returns System.Double.NaN.</returns>
        public double Sin(double a)
        {
            return System.Math.Sin(a);
        }

        /// <summary>Returns the tangent of the specified angle.</summary>
        /// <param name="a">An angle, measured in radians.</param>
        /// <returns>The tangent of <paramref name="a">a</paramref>. If <paramref name="a">a</paramref> is equal to System.Double.NaN, System.Double.NegativeInfinity, or System.Double.PositiveInfinity, this method returns System.Double.NaN.</returns>
        public double Tan(double a)
        {
            return System.Math.Tan(a);
        }

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic sine of <paramref name="value">value</paramref>. If <paramref name="value">value</paramref> is equal to System.Double.NegativeInfinity, System.Double.PositiveInfinity, or System.Double.NaN, this method returns a T:System.Double equal to <paramref name="value">value</paramref>.</returns>
        public double Sinh(double value)
        {
            return System.Math.Sinh(value);
        }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic tangent of <paramref name="value">value</paramref>. If <paramref name="value">value</paramref> is equal to System.Double.NegativeInfinity, this method returns -1. If value is equal to System.Double.PositiveInfinity, this method returns 1. If <paramref name="value">value</paramref> is equal to System.Double.NaN, this method returns System.Double.NaN.</returns>
        public double Tanh(double value)
        {
            return System.Math.Tanh(value);
        }

        /// <summary>Rounds a double-precision floating-point value to the nearest integral value.</summary>
        /// <param name="a">A double-precision floating-point number to be rounded.</param>
        /// <returns>The integer nearest <paramref name="a">a</paramref>. If the fractional component of <paramref name="a">a</paramref> is halfway between two integers, one of which is even and the other odd, then the even number is returned. Note that this method returns a T:System.Double instead of an integral type.</returns>
        public double Round(double a)
        {
            return System.Math.Round(a);
        }

        /// <summary>Rounds a double-precision floating-point value to a specified number of fractional digits.</summary>
        /// <param name="a">A double-precision floating-point number to be rounded.</param>
        /// <param name="decimals">The number of fractional digits in the return value.</param>
        /// <returns>The number nearest to <paramref name="a">value</paramref> that contains a number of fractional digits equal to <paramref name="decimals">digits</paramref>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="decimals">digits</paramref> is less than 0 or greater than 15.</exception>
        public double Round(double a, int decimals)
        {
            return System.Math.Round(a, decimals);
        }

        /// <summary>Calculates the integral part of a specified decimal number.</summary>
        /// <param name="d">A number to truncate.</param>
        /// <returns>The integral part of <paramref name="d">d</paramref>; that is, the number that remains after any fractional digits have been discarded.</returns>
        public double Truncate(double d)
        {
            return System.Math.Truncate(d);
        }

        /// <summary>
        /// Returns the Square Root of <paramref name="d"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Square Root</returns>
        public double Sqrt(double d)
        {
            return System.Math.Sqrt(d);
        }

        /// <summary>
        /// Returns the Natural Logarithm of <paramref name="d"/>
        /// </summary>
        /// <param name="d">Input</param>
        /// <returns>Natural Logarithm</returns>
        public double Log(double d)
        {
            return System.Math.Log(d);
        }
        /// <summary>
        /// Returns the Logarithm with base 10 of <paramref name="d"/>
        /// </summary>
        /// <param name="d">Input</param>
        /// <returns>Logarithm of base 10</returns>
        public double Log10(double d)
        {
            return System.Math.Log10(d);
        }

        /// <summary>Returns e raised to the specified power.</summary>
        /// <param name="d">A number specifying a power.</param>
        /// <returns>The number e raised to the power <paramref name="d">d</paramref>. If <paramref name="d">d</paramref> equals System.Double.NaN or System.Double.PositiveInfinity, that value is returned. If <paramref name="d">d</paramref> equals System.Double.NegativeInfinity, 0 is returned.</returns>
        public double Exp(double d)
        {
            return System.Math.Exp(d);
        }

        /// <summary>Returns a specified number raised to the specified power.</summary>
        /// <param name="x">A double-precision floating-point number to be raised to a power.</param>
        /// <param name="y">A double-precision floating-point number that specifies a power.</param>
        /// <returns>The number <paramref name="x">x</paramref> raised to the power <paramref name="y">y</paramref>.</returns>
        public double Pow(double x, double y)
        {
            return System.Math.Pow(x, y);
        }

        /// <summary>Returns the remainder resulting from the division of a specified number by another specified number.</summary>
        /// <param name="x">A dividend.</param>
        /// <param name="y">A divisor.</param>
        /// <returns>A number equal to <paramref name="x">x</paramref> - (<paramref name="y">y</paramref> Q), where Q is the quotient of <paramref name="x">x</paramref> / <paramref name="y">y</paramref> rounded to the nearest integer (if <paramref name="x">x</paramref> / <paramref name="y">y</paramref> falls halfway between two integers, the even integer is returned).   If <paramref name="x">x</paramref> - (<paramref name="y">y</paramref> Q) is zero, the value +0 is returned if <paramref name="x">x</paramref> is positive, or -0 if <paramref name="x">x</paramref> is negative.   If <paramref name="y">y</paramref> = 0, System.Double.NaN is returned.</returns>
        public double IEEERemainder(double x, double y)
        {
            return System.Math.IEEERemainder(x, y);
        }

        /// <summary>Returns the absolute value of a double-precision floating-point number.</summary>
        /// <param name="value">A number that is greater than or equal to System.Double.MinValue, but less than or equal to System.Double.MaxValue.</param>
        /// <returns>A double-precision floating-point number, x, such that 0 ≤ x ≤System.Double.MaxValue.</returns>
        public double Abs(double value)
        {
            return System.Math.Abs(value);
        }

        /// <summary>Returns the larger of two double-precision floating-point numbers.</summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        /// <returns>Parameter <paramref name="val1">val1</paramref> or <paramref name="val2">val2</paramref>, whichever is larger. If <paramref name="val1">val1</paramref>, <paramref name="val2">val2</paramref>, or both <paramref name="val1">val1</paramref> and <paramref name="val2">val2</paramref> are equal to System.Double.NaN, System.Double.NaN is returned.</returns>
        public double Max(double val1, double val2)
        {
            return System.Math.Max(val1, val2);
        }

        /// <summary>Returns the smaller of two double-precision floating-point numbers.</summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        /// <returns>Parameter <paramref name="val1">val1</paramref> or <paramref name="val2">val2</paramref>, whichever is smaller. If <paramref name="val1">val1</paramref>, <paramref name="val2">val2</paramref>, or both <paramref name="val1">val1</paramref> and <paramref name="val2">val2</paramref> are equal to System.Double.NaN, System.Double.NaN is returned.</returns>
        public double Min(double val1, double val2)
        {
            return System.Math.Min(val1, val2);
        }

        /// <summary>
        /// Returns the Logarithm with base <paramref name="newBase"/> of <paramref name="a"/>
        /// </summary>
        /// <param name="a">Input</param>
        /// <param name="newBase">The Logarithmic Base</param>
        /// <returns>Logarithm of base newBase</returns>
        public double Log(double a, double newBase)
        {
            return System.Math.Log(a, newBase);
        }


        /// <summary>
        /// Returns the <paramref name="basis"/> raised to the power of <paramref name="exp"/> using Integer Multiplication
        /// </summary>
        /// <param name="basis">Basis</param>
        /// <param name="exp">Exponent</param>
        /// <returns>Basis to the Power of Exponent</returns>
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

        /// <summary>
        /// Returns the <paramref name="basis"/> raised to the power of <paramref name="exp"/> using Integer Multiplication
        /// </summary>
        /// <param name="basis">Basis</param>
        /// <param name="exp">Exponent</param>
        /// <returns>Basis to the Power of Exponent</returns>
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