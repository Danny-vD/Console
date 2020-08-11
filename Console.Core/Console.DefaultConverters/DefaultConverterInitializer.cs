using System;
using System.Reflection;
using Console.Core;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.DefaultConverters
{
    public class DefaultConverterInitializer : AExtensionInitializer
    {
        [Command("enum-list", "Lists all Values of the Specified Enum")]
        private static void ListEnumValues(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            if (t != null && t.IsEnum)
            {
                string s = $"Values of Enum: {t.Name}\n";
                Array vals = Enum.GetValues(t);
                foreach (object val in vals)
                {
                    s += "\t" + Enum.GetName(t, val) + "=" + (int)val + "\n";
                }
                AConsoleManager.Instance.Log(s);
                return;
            }
            AConsoleManager.Instance.LogWarning("Qualified Name is not recognized or the Type is not an Enum");
        }

        [Property("version.defaultconverters")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<DefaultConverterInitializer>();
            PropertyAttributeUtils.AddProperties<DefaultConverterInitializer>();
            PropertyAttributeUtils.AddProperties<DateTimeConverter>();
            PropertyAttributeUtils.AddProperties<EnumConverter>();
            CustomConvertManager.AddConverter(new DateTimeConverter());
            CustomConvertManager.AddConverter(new FileInfoConverter());
            CustomConvertManager.AddConverter(new DirInfoConverter());
            CustomConvertManager.AddConverter(new EnumDigitConverter()); //Before Enum Converter(Enum Converter detects the same conditions)
            CustomConvertManager.AddConverter(new EnumConverter());
        }
    }
}
