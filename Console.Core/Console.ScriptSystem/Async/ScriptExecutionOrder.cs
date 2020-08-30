namespace Console.ScriptSystem.Async
{
    /// <summary>
    /// Sets the Order in which the Runners get Executed
    /// SubFirst => Sub Runners have Higher Priority than the Parent
    /// </summary>
    public enum ScriptExecutionOrder
    {
        /// <summary>
        /// Will Execute the Parent Script Content before Executing the Subscripts
        /// </summary>
        ParentFirst,
        /// <summary>
        /// Will Execute the Sub Scripts before continuing the Parent Script
        /// </summary>
        SubFirst
    }
}