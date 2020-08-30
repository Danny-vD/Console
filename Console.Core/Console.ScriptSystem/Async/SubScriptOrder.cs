namespace Console.ScriptSystem.Async
{
    /// <summary>
    /// Specifies the Order in which the SubRunner get executed in turns
    /// </summary>
    public enum SubScriptOrder
    {
        /// <summary>
        /// The Last Script that got Inserted at this level will be finished first
        /// </summary>
        Stack,

        /// <summary>
        /// The First Script that got Inserted at this level will be finished first
        /// </summary>
        Queue,

        /// <summary>
        /// The Subscripts will get one execution cycle in turns
        /// </summary>
        Rotating
    }
}