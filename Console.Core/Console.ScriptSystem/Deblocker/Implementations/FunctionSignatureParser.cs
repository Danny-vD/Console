using System;
using System.Linq;
using System.Text;

namespace Console.ScriptSystem.Deblocker.Implementations
{
    public static class FunctionSignatureParser
    {
        public struct FunctionSignature
        {
            public string[] ParameterNames;


            public FunctionSignature(string[] parameterNames)
            {
                ParameterNames = parameterNames.Select(x => x.Trim()).ToArray();
            }

            public override string ToString()
            {
                if (ParameterNames.Length == 0)
                    return $"{DeblockerSettings.OpenFunctionBracket}{DeblockerSettings.CloseFunctionBracket}";
                StringBuilder sb = new StringBuilder($"{DeblockerSettings.OpenFunctionBracket}{ParameterNames[0]}");
                for (int i = 1; i < ParameterNames.Length; i++)
                {
                    string parameterName = ParameterNames[i];
                    sb.Append(", " + parameterName);
                }
                sb.Append(DeblockerSettings.CloseFunctionBracket);
                return sb.ToString();
            }
        }

        public static FunctionSignature ParseFunctionSignature(Line line, out int sigStart, out int sigLength)
        {

            int open = line.CleanedLine.IndexOf(DeblockerSettings.OpenFunctionBracket);
            int close = line.CleanedLine.IndexOf(DeblockerSettings.CloseFunctionBracket, open + 1);
            if (open != -1 && close != -1 && open < close)
            {
                string sigP = line.CleanedLine.Substring(open + 1, close - open - 1);
                sigStart = open;
                sigLength = close - open + 1;
                FunctionSignature sig =
                    new FunctionSignature(sigP.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                return sig;
            }
            if (open == -1 && close == -1)
            {
                sigStart = sigLength = 0;
                return new FunctionSignature(new string[0]);
            }

            throw new FunctionSignatureException("Invalid Function Signature: " + line.CleanedLine +
                                                 "\n\t Original Line: " + line.OriginalLine);

        }

        public class FunctionSignatureException : Exception
        {
            public FunctionSignatureException(string message) : base(message)
            {
            }
        }
    }
}