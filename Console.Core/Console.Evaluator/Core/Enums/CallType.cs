using System;

/// <summary>
/// Namespace of all Enums in the Evaluator.
/// </summary>
namespace Console.Evaluator.Core.Enums
{
    /// <summary>
    /// Defines the Different CallTypes that the Evaluator can Utilize
    /// </summary>
    [Flags]
    internal enum CallType
    {

        /// <summary>
        /// The Call will be invoked with a Field as the Target
        /// </summary>
        Field = 1,

        /// <summary>
        /// The Call will be invoked with a Method as the Target
        /// </summary>
        Method = 2,

        /// <summary>
        /// The Call will be invoked with a Property as the Target
        /// </summary>
        Property = 4,

        /// <summary>
        /// The Call will be invoked with this setting when the Target Could not be Determined.
        /// </summary>
        All = 7

    }
}