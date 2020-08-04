using System.Reflection;

namespace Console.Attributes.PropertySystem.Helper
{
    internal class StaticFieldHelper : FieldHelper
    {
        internal StaticFieldHelper(FieldInfo info) : base(null, info) { }
    }
}