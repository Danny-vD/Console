using System;
using System.Text;

namespace Console.EnvironmentVariables
{
    public class RangeVariableProvider : VariableProvider
    {
        public override string FunctionName => "range";
        public override string GetValue(string parameter)
        {
            string[] parts = parameter.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "";
            if (parts.Length == 1) return CreateRange(0, double.Parse(parts[0]), 1);
            if (parts.Length == 2) return CreateRange(double.Parse(parts[0]), double.Parse(parts[1]), 1);
            if (parts.Length == 3) return CreateRange(double.Parse(parts[0]), double.Parse(parts[1]), double.Parse(parts[2]));
            return "";
        }

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