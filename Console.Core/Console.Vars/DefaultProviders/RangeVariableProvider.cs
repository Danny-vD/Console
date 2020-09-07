using System;
using System.Text;

namespace Console.Vars.DefaultProviders
{
    /// <summary>
    /// Creates a ; Seperated List of numbers in a specfied range
    /// </summary>
    public class RangeVariableProvider : VariableProvider
    {

        /// <summary>
        /// The Function Name
        /// </summary>
        public override string FunctionName => "range";

        /// <summary>
        /// Parameter Layouts:
        ///     end
        ///     start;end
        ///     start;end;step
        /// If Step is not specified the step will be 1
        /// </summary>
        /// <param name="parameter">Input Parameter</param>
        /// <returns>; Seperated List of Numbers</returns>
        public override string GetValue(string parameter)
        {
            string[] parts = parameter.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return "";
            }

            if (parts.Length == 1)
            {
                return CreateRange(0, double.Parse(parts[0]), 1);
            }

            if (parts.Length == 2)
            {
                return CreateRange(double.Parse(parts[0]), double.Parse(parts[1]), 1);
            }

            if (parts.Length == 3)
            {
                return CreateRange(double.Parse(parts[0]), double.Parse(parts[1]), double.Parse(parts[2]));
            }

            return "";
        }

        /// <summary>
        /// Creates the Range String
        /// </summary>
        /// <param name="start">Start Number</param>
        /// <param name="end">End Number</param>
        /// <param name="step">Amount of change each step.</param>
        /// <returns>; Seperated List of Numbers</returns>
        private string CreateRange(double start, double end, double step)
        {
            StringBuilder sb = new StringBuilder();

            if (end <= start && step < 0)
            {
                for (double i = start; i > end; i += step)
                {
                    sb.Append(i + ";");
                }
            }
            else if (start <= end && step > 0)
            {
                for (double i = start; i < end; i += step)
                {
                    sb.Append(i + ";");
                }
            }

            return sb.ToString();
        }

    }
}