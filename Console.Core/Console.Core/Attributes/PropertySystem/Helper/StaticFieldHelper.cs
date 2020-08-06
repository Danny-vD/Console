using System.Reflection;

namespace Console.Core.Attributes.PropertySystem.Helper
{
    public class StaticFieldHelper : FieldHelper
    {
        public StaticFieldHelper(FieldInfo info) : base(null, info) { }
    }
}