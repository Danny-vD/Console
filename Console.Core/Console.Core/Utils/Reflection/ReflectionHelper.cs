namespace Console.Core.Utils.Reflection
{
    public abstract class ReflectionHelper
    {
        public abstract bool CanWrite { get; }

        public abstract object GetValue();
        public abstract void SetValue(object value);
    }
}