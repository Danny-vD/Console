
namespace Console.Evaluator.Core.Interfaces
{
    /// <summary>
    /// IVariableBag implements a Collection of Variables.
    /// </summary>
    public interface IVariableBag
    {
        /// <summary>
        /// Returns the Variable by name
        /// </summary>
        /// <param name="varname">The Variable Name</param>
        /// <returns>IEvalTypedValue instance with the specified name.</returns>
        IEvalTypedValue GetVariable(string varname);
    }
}