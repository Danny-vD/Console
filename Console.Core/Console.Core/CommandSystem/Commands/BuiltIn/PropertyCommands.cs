using System.Collections.Generic;
using System.Linq;

using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Builder.BuiltIn.PropertyAutoFill;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Default Commands that allow to interface with the property system
    /// </summary>
    public class PropertyCommands
    {

        private static readonly ALogger ListPropertiesLogger = TypedLogger.CreateTypedWithPrefix("list-properties");
        private static readonly ALogger GetPropertiesLogger = TypedLogger.CreateTypedWithPrefix("get-properties");

        /// <summary>
        /// Adds the Property Commands
        /// </summary>
        public static void AddPropertyCommands()
        {
            CommandAttributeUtils.AddCommands<PropertyCommands>();
        }

        #region List Properties

        /// <summary>
        /// Lists all Property Names
        /// </summary>
        [Command(
            "list-properties",
            HelpMessage = "Lists all Properties",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "lp" }
        )]
        private static void ListPropertiesCommand()
        {
            ListPropertiesCommand(string.Empty);
        }

        /// <summary>
        /// Lists all Properties that start with the specified string
        /// </summary>
        /// <param name="start">Search Term.</param>
        [Command(
            "list-properties",
            HelpMessage = "Lists all Properties that start with the specified sequence",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "lp" }
        )]
        private static void ListPropertiesCommand(
            [PropertyAutoFill]
            string start)
        {
            string ret = "Properties:\n\t";
            List<string> p = PropertyManager.AllPropertyPaths;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].StartsWith(start))
                {
                    ret += p[i];
                    if (i != p.Count - 1)
                    {
                        ret += "\n\t";
                    }
                }
            }

            ListPropertiesLogger.Log(ret);
        }

        #endregion

        #region Get Properties

        /// <summary>
        /// Lists All properties and their values.
        /// </summary>
        [Command(
            "get-property",
            HelpMessage =
                "Gets the value of the specified property and prints its ToString implementation to the console.",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "gp" }
        )]
        private static void GetProperty()
        {
            GetProperty("");
        }

        /// <summary>
        /// Lists all Properties(including values) that start with the specified string.
        /// </summary>
        /// <param name="propertyPath">Search Term/Property Path</param>
        [Command(
            "get-property",
            HelpMessage =
                "Gets the value of the specified property and prints its ToString implementation to the console.",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "gp" }
        )]
        private static void GetProperty(
            [PropertyAutoFill]
            string propertyPath)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                List<string> targets = PropertyManager.AllPropertyPaths.Where(x => x.StartsWith(propertyPath)).ToList();
                if (targets.Count == 0)
                {
                    GetPropertiesLogger.LogWarning("Can not find property path: " + propertyPath);
                }
                else
                {
                    string s = "Properties that match: " + propertyPath;
                    foreach (string target in targets)
                    {
                        if (!PropertyManager.TryGetValue(target, out object v))
                        {
                            v = "ERROR";
                        }

                        s += $"\n\t{target} = {v ?? "NULL"}";
                    }

                    GetPropertiesLogger.Log(s);
                }

                return;
            }

            if (!PropertyManager.TryGetValue(propertyPath, out object value))
            {
                GetPropertiesLogger.LogWarning("Can not get the value at path: " + propertyPath);
            }

            GetPropertiesLogger.Log($"{propertyPath} = {value}");
        }

        #endregion

        #region Set/Add Properties

        /// <summary>
        /// Sets the Selected Item as the value of the specified property
        /// </summary>
        /// <param name="propertyPath">Property Name</param>
        /// <param name="propertyValue">New Value of the Property</param>
        [Command(
            "set-property-selection",
            HelpMessage = "Sets the specified property to the specified value",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "sps" }
        )]
        private static void SetPropertySelection(
            [PropertyAutoFill]
            string propertyPath, [SelectionProperty(true)]
            object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                return;
            }

            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        /// <summary>
        /// Adds the Selected Item as the value of the specified property
        /// </summary>
        /// <param name="propertyPath">Property Name</param>
        /// <param name="propertyValue">New Value of the Property</param>
        [Command(
            "add-property-selection",
            HelpMessage = "Sets or Adds the specified property to the specified value",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "aps" }
        )]
        private static void AddPropertySelection(
            [PropertyAutoFill]
            string propertyPath, [SelectionProperty(true)]
            object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                PropertyManager.AddProperty(propertyPath, propertyValue);
                return;
            }

            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        /// <summary>
        /// Sets the Specified Item as the value of the specified property
        /// </summary>
        /// <param name="propertyPath">Property Name</param>
        /// <param name="propertyValue">New Value of the Property</param>
        [Command(
            "set-property",
            HelpMessage = "Sets the specified property to the specified value",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "sp" }
        )]
        private static void SetProperty(
            [PropertyAutoFill]
            string propertyPath, object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                return;
            }

            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        /// <summary>
        /// Adds the Specified Item as the value of the specified property
        /// </summary>
        /// <param name="propertyPath">Property Name</param>
        /// <param name="propertyValue">New Value of the Property</param>
        [Command(
            "add-property",
            HelpMessage = "Sets or Adds the specified property to the specified value",
            Namespace = ConsoleCoreConfig.PROPERTY_NAMESPACE,
            Aliases = new[] { "ap" }
        )]
        private static void AddProperty(
            [PropertyAutoFill]
            string propertyPath, object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                PropertyManager.AddProperty(propertyPath, propertyValue);
                return;
            }

            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        private static void DeleteProperty(
            [PropertyAutoFill]
            string propertyPath)
        {
            if (PropertyManager.HasProperty(propertyPath))
            {
                PropertyManager.Remove(propertyPath);
            }
        }

        #endregion

    }
}