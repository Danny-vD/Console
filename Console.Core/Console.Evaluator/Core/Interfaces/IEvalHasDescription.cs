namespace Console.Evaluator.Core.Interfaces
{
    /// <summary>
    /// Defines an interface that has a name and a description
    /// </summary>
    public interface IEvalHasDescription
    {
        /// <summary>
        /// Name of the Object
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The Description of the Object
        /// </summary>
        string Description { get; }
    }
}