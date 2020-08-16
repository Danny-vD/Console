using System;
using System.Reflection;
using Console.Core;
using Console.Core.ActivationSystem;
using Console.Core.CommandSystem;
using Console.Core.ConverterSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

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
            AConverter[] aco = ActivateOnAttributeUtils.ActivateObjects<AConverter>(Assembly.GetExecutingAssembly());
            foreach (AConverter aConverter in aco)
            {
                CustomConvertManager.AddConverter(aConverter);
            }
        }
    }
}
