using System.Collections.Generic;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.PropertySystem;

namespace Console.Core.Utils
{
    public class TestClass
    {
        [Property("core.test.object")]
        public object TestObject;
        [Property("core.test.list")]
        public List<object> TestList;
        [Property("core.test.array")]
        public object[] TestArray;

        public static void InitializeTests()
        {
            TestClass tc = new TestClass();
            CommandAttributeUtils.AddCommands(tc);
            PropertyAttributeUtils.AddProperties<TestClass>();
        }

        [Command("second", "")]
        private static void TestCommandSecond([SelectionProperty] object value)
        {
            AConsoleManager.Instance.Log(value);
        }

        [Command("first", "")]
        private static void TestCommandFirst([SelectionProperty] object[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                AConsoleManager.Instance.Log(value[i]);
            }
        }
    }
}