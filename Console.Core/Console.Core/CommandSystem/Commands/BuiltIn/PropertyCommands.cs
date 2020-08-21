﻿using System.Collections.Generic;
using System.Linq;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Default Commands that allow to interface with the property system
    /// </summary>
    public class PropertyCommands
    {
        private static PrefixLogger ListPropertiesLogger = new PrefixLogger("list-properties");
        private static PrefixLogger GetPropertiesLogger = new PrefixLogger("get-properties");

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
        [Command("list-properties", "Lists all Properties", "lp")]
        private static void ListPropertiesCommand()
        {
            ListPropertiesCommand(string.Empty);
        }

        /// <summary>
        /// Lists all Properties that start with the specified string
        /// </summary>
        /// <param name="start">Search Term.</param>
        [Command("list-properties", "Lists all Properties that start with the specified sequence", "lp")]
        private static void ListPropertiesCommand(string start)
        {
            string ret = "Properties:\n\t";
            List<string> p = PropertyManager.AllPropertyPaths;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].StartsWith(start))
                {
                    ret += p[i];
                    if (i != p.Count - 1) ret += "\n\t";
                }
            }
            ListPropertiesLogger.Log(ret);
        }

        #endregion

        #region Get Properties

        /// <summary>
        /// Lists All properties and their values.
        /// </summary>
        [Command("get-property",
            "Gets the value of the specified property and prints its ToString implementation to the console.", "gp")]
        private static void GetProperty() => GetProperty("");

        /// <summary>
        /// Lists all Properties(including values) that start with the specified string.
        /// </summary>
        /// <param name="propertyPath">Search Term/Property Path</param>
        [Command("get-property",
            "Gets the value of the specified property and prints its ToString implementation to the console.", "gp")]
        private static void GetProperty(string propertyPath)
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
                        object v;
                        if (!PropertyManager.TryGetValue(target, out v)) v = "ERROR";
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
        [Command("set-property-selection", "Sets the specified property to the specified value", "sps")]
        private static void SetPropertySelection(string propertyPath, [SelectionProperty(true)] object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath)) return;
            
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        /// <summary>
        /// Adds the Selected Item as the value of the specified property
        /// </summary>
        /// <param name="propertyPath">Property Name</param>
        /// <param name="propertyValue">New Value of the Property</param>
        [Command("add-property-selection", "Sets or Adds the specified property to the specified value", "aps")]
        private static void AddPropertySelection(string propertyPath, [SelectionProperty(true)] object propertyValue)
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
        [Command("set-property", "Sets the specified property to the specified value", "sp")]
        private static void SetProperty(string propertyPath, object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
                return;
            
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        /// <summary>
        /// Adds the Specified Item as the value of the specified property
        /// </summary>
        /// <param name="propertyPath">Property Name</param>
        /// <param name="propertyValue">New Value of the Property</param>
        [Command("add-property", "Sets or Adds the specified property to the specified value", "ap")]
        private static void AddProperty(string propertyPath, object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                PropertyManager.AddProperty(propertyPath, propertyValue);
                return;
            }
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        #endregion
    }
}