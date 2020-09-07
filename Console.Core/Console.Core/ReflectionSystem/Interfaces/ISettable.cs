namespace Console.Core.ReflectionSystem.Interfaces
{
    /// <summary>
    /// Inherited by Classes that can set an inner value
    /// </summary>
    public interface ISettable
    {

        /// <summary>
        /// Sets the Inner Value
        /// </summary>
        /// <param name="value">New Value</param>
        void Set(object value);

    }
}