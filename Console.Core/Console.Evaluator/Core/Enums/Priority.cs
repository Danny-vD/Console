namespace Console.Evaluator.Core.Enums
{
    internal enum Priority
    {
        None = 0,
        Or = 1,
        And = 2,
        Not = 3,
        Equality = 4,
        Concat = 5,
        Plusminus = 6,
        Muldiv = 7,
        Percent = 8,
        Unaryminus = 9
    }
}