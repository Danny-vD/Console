namespace Console.Core.ReflectionSystem.Interfaces
{
    /// <summary>
    /// Inherited by Classes that can return an inner value
    /// </summary>
    public interface IGettable
    {
        /// <summary>
        /// Returns the Inner Value
        /// </summary>
        /// <returns>The Inner Value</returns>
        object Get();
    }
}