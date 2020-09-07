namespace Console.Core.ExtensionSystem
{
    /// <summary>
    /// Specifies the Different Load Order Settings.
    /// </summary>
    public enum LoadOrder
    {

        /// <summary>
        /// Default. Gets Loaded After LoadOrder.First and before LoadOrder.After.
        /// </summary>
        Default,

        /// <summary>
        /// Extensions get loaded as early as possible
        /// </summary>
        First,

        /// <summary>
        /// Extensions get loaded as late as possible.
        /// </summary>
        After

    }
}