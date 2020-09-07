namespace Console.Core.CommandSystem.Commands
{
    /// <summary>
    /// Defines Minimum and Maximum Parameter Count for a Command
    /// </summary>
    public struct ParameterRange
    {

        /// <summary>
        /// Minimum Parameter Count
        /// </summary>
        public int Min;

        /// <summary>
        /// Maximum Parameter Count
        /// </summary>
        public int Max;

        /// <summary>
        /// Creates a Parameter Range with only one possible parameter count
        /// </summary>
        /// <param name="max">Maximum/Minimum Parameter Count</param>
        public ParameterRange(int max)
        {
            Min = Max = max;
        }

        /// <summary>
        /// Creates a Parameter Range of variable parameter count
        /// </summary>
        /// <param name="min">Minimum Parameter Count</param>
        /// <param name="max">Maximum Parameter Count</param>
        public ParameterRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Returns true when the specified parameter count is inside this range.
        /// </summary>
        /// <param name="value">Value to check if it is Contained</param>
        /// <returns>True if the Value is within the range.</returns>
        public bool Contains(int value)
        {
            return Min <= value && Max >= value;
        }


        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Min} - {Max}";
        }

    }
}