namespace Console.Evaluator.Core.Interfaces
{
    /// <summary>
    /// IEvalFunctions is used as Function Provider 
    /// </summary>
    public interface IEvalFunctions
    {

        /// <summary>
        /// The Inherited Functions of this object
        /// </summary>
        /// <returns>The Base Class/Inherited IEvalFunctions Instance</returns>
        IEvalFunctions InheritedFunctions();

    }
}