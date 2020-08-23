namespace Console.Evaluator.Core.Enums
{
    /// <summary>
    /// The Priority Defines the precedence of certain operators
    /// </summary>
    internal enum Priority
    {
        /// <summary>
        /// No Priority
        /// </summary>
        None = 0,
        /// <summary>
        /// Priority for OR Operator
        /// </summary>
        Or = 1,
        /// <summary>
        /// Priority for AND Operator
        /// </summary>
        And = 2,
        /// <summary>
        /// Priority for NOT Operator
        /// </summary>
        Not = 3,
        /// <summary>
        /// Priority for EQUALITY Operator
        /// </summary>
        Equality = 4,
        /// <summary>
        /// Priority for CONCATENATION Operator
        /// </summary>
        Concat = 5,
        /// <summary>
        /// Priority for ADD/SUBSTRACT Operators
        /// </summary>
        Plusminus = 6,
        /// <summary>
        /// Priority for MUL/DIV Operators
        /// </summary>
        Muldiv = 7,
        /// <summary>
        /// Priority for PERCENTAGE Operator
        /// </summary>
        Percent = 8,
        /// <summary>
        /// Priority for UNARY MINUS Operator
        /// </summary>
        Unaryminus = 9
    }
}