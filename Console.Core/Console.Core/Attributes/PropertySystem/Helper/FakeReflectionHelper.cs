namespace Console.Core.Attributes.PropertySystem.Helper
{
    public class FakeReflectionHelper : ReflectionHelper
    {
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