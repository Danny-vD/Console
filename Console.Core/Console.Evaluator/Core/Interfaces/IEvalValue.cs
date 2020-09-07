/// <summary>
/// Namespace of All interfaces in the Evaluator
/// </summary>

namespace Console.Evaluator.Core.Interfaces
{
    /// <summary>
    /// The IEvalValue defines a Value and Value Changed Event Handler Properties
    /// </summary>
    public interface IEvalValue
    {

        /// <summary>
        /// The Value of the Object
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Value Changed Event
        /// </summary>
        event ValueChangedEventHandler ValueChanged;

    }
}