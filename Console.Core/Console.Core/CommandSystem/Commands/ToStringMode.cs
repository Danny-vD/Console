namespace Console.Core.CommandSystem.Commands
{
    /// <summary>
    /// The ToStringMode is specifying the Length/Information of the ToString method.
    /// </summary>
    public enum ToStringMode
    {
        /// <summary>
        /// Default ToStringMode.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Short Mode. Reduces OutputLength/Information
        /// </summary>
        Short = 1,
        /// <summary>
        /// Long Mode. More OutputLength/Information
        /// </summary>
        Long = 2,
        /// <summary>
        /// No Output. Returns Empty String
        /// </summary>
        None = 3,
    }
}