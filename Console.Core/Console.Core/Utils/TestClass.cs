using System.Collections.Generic;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Console;

namespace Console.Core.Utils
{
    public class TestClass
    {
        [ConsoleProperty("test.object")]
        public object TestObject;
        [ConsoleProperty("test.list")]
        public List<object> TestList;
        [ConsoleProperty("test.array")]
        public object[] TestArray;

        public static void InitializeTests()
        {
            TestClass tc = new TestClass();
            CommandAttributeUtils.AddCommands(tc);
            ConsolePropertyAttributeUtils.AddProperties<TestClass>();
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