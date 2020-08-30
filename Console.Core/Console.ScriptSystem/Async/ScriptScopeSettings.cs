namespace Console.ScriptSystem.Async
{
    /// <summary>
    /// Specifies the Position where new Runners get Added.
    /// </summary>
    public enum ScriptScopeSettings
    {
        /// <summary>
        /// Will be executed as Subscript of the Parent
        /// </summary>
        SubLevel,
        /// <summary>
        /// Will be executed on the Same Level as the Parent Script
        /// </summary>
        ParentLevel
    }
}