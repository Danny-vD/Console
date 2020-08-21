using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console.ScriptSystem.Deblocker.Implementations
{
    public static class FunctionSignatureParser
    {
        public struct FunctionSignature
        {
            public string OriginalSignature;
            public List<string> ParameterNames;


            public FunctionSignature(string parameterNames)
            {
                OriginalSignature = parameterNames;
                ParameterNames = parameterNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
            }

            public override string ToString()
            {
                if (ParameterNames.Count == 0)
                    return $"{DeblockerSettings.OpenFunctionBracket}{DeblockerSettings.CloseFunctionBracket}";
                StringBuilder sb = new StringBuilder($"{DeblockerSettings.OpenFunctionBracket}{ParameterNames[0]}");
                for (int i = 1; i < ParameterNames.Count; i++)
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

            int open = line.OriginalLine.IndexOf(DeblockerSettings.OpenFunctionBracket);
            int close = line.OriginalLine.IndexOf(DeblockerSettings.CloseFunctionBracket, open + 1);
            if (open != -1 && close != -1 && open < close)
            {
                string sigP = line.OriginalLine.Substring(open + 1, close - open - 1);
                sigStart = open;
                sigLength = sigP.Length+2;
                string s = line.OriginalLine.Substring(sigStart, sigLength);
                FunctionSignature sig =
                    new FunctionSignature(sigP);
                if (open != 0 && line.OriginalLine[open - 1] == ' ' &&
                    close!=line.OriginalLine.Length-1 && line.OriginalLine[close+1] ==' ')
                {
                    sigLength++; //Removing one of the Spaces if there are 2
                }
                return sig;
            }
            if (open == -1 && close == -1)
            {
                sigStart = sigLength = 0;
                return new FunctionSignature("");
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