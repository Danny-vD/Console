namespace Console.Core.Utils.Reflection
{
    public class FakeReflectionHelper : ReflectionHelper
    {
        public override bool CanWrite => true;
        private object Value;
        public FakeReflectionHelper(object value)
        {
            Value = value;
        }

        public override object GetValue() => Value;

        public override void SetValue(object value)
        {
            Value = value;
        }
    }
}