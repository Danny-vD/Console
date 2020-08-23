using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Internal Functon Signature Parsing Implementation
/// </summary>
namespace Console.ScriptSystem.Deblocker.Functions.Internal
{
    /// <summary>
    /// The Internal Representation of the Parameters of Sequences.
    /// </summary>
    internal struct FunctionSignature
    {
        /// <summary>
        /// The Original Signature as it was defined in the Sequence
        /// </summary>
        public string OriginalSignature;
        /// <summary>
        /// The List of Parameter Names
        /// </summary>
        public List<string> ParameterNames;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="parameterNames">The Original Parameter Signature</param>
        public FunctionSignature(string parameterNames)
        {
            OriginalSignature = parameterNames;
            ParameterNames = parameterNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        /// <summary>
        /// Returns the Correctly Formatted Sequence Signature String Representation
        /// </summary>
        /// <returns>Formatted Parameter Signature</returns>
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

        /// <summary>
        /// Parses the parameter signature from a line.
        /// </summary>
        /// <param name="line">The Line with the Parameter Signature</param>
        /// <param name="sigStart">Out Variable that contains the start index of the Signature</param>
        /// <param name="sigLength">Out Variable that contains the length of the signature</param>
        /// <returns>Parsed Signature</returns>
        internal static FunctionSignature Parse(Line line, out int sigStart, out int sigLength)
        {

            int open = line.OriginalLine.IndexOf(DeblockerSettings.OpenFunctionBracket);
            int close = line.OriginalLine.IndexOf(DeblockerSettings.CloseFunctionBracket, open + 1);
            if (open != -1 && close != -1 && open < close)
            {
                string sigP = line.OriginalLine.Substring(open + 1, close - open - 1);
                sigStart = open;
                sigLength = sigP.Length + 2;
                string s = line.OriginalLine.Substring(sigStart, sigLength);
                FunctionSignature sig =
                    new FunctionSignature(sigP);
                if (open != 0 && line.OriginalLine[open - 1] == ' ' &&
                    close != line.OriginalLine.Length - 1 && line.OriginalLine[close + 1] == ' ')
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
    }
}