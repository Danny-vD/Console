namespace Console.Evaluator.Core.Interfaces
{
    public interface IEvalValue
    {
        object Value { get; }

        event ValueChangedEventHandler ValueChanged;

    }
}