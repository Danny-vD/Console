using System.Reflection;

namespace Console.Core.Utils.Reflection.Fields
{
    public class StaticFieldHelper : FieldHelper
    {
        public StaticFieldHelper(FieldInfo info) : base(null, info) { }
    }
}