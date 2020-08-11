using System;
using Console.Evaluator.Core.Enums;

namespace Console.Evaluator.Core.Interfaces
{
    public interface IEvalTypedValue : IEvalValue
    {
        Type SystemType { get; }
        EvalType EvalType { get; }
    }
}