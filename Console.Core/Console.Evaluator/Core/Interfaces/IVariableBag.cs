
namespace Console.Evaluator.Core.Interfaces
{
    public interface IVariableBag
    {
        IEvalTypedValue GetVariable(string varname);
    }
}