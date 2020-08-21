using System.Collections.Generic;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.CLI
{
    /// <summary>
    /// Commands to Select/Unselect Objects
    /// </summary>
    public class CLIObjectSelectionCommands
    {
        /// <summary>
        /// Helper Property. Contains all selectable objects
        /// </summary>
        private static Dictionary<string, object> SelectableObjects =>
            (AConsoleManager.Instance.ObjectSelector is CLIObjSelector os)
                ? os.SelectableObjects
                : new Dictionary<string, object>();

        /// <summary>
        /// Adds all Commands in this class.
        /// </summary>
        public static void AddSelectionCommands()
        {
            CommandAttributeUtils.AddCommands<CLIObjectSelectionCommands>();
        }


        /// <summary>
        /// Adds an object to the selected objects
        /// </summary>
        /// <param name="key">Key of the Object to Select</param>
        [Command("add-selectable", "Adds an object to the selection")]
        private static void Select(string key)
        {
            if (SelectableObjects.ContainsKey(key))
            {
                Program.Logger.Log("Selecting Object: " + SelectableObjects[key]);
                AConsoleManager.Instance.ObjectSelector.AddToSelection(SelectableObjects[key]);
                Program.Logger.Log("Selected Objects: " + AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count);
            }
        }



        /// <summary>
        /// Makes the object to select the only selected object
        /// </summary>
        /// <param name="key">Key of the Object to Select</param>
        [Command("select-selectable", "Select an object")]
        private static void SelectOne(string key)
        {
            AConsoleManager.Instance.ObjectSelector.ClearSelection();
            Select(key);
        }


        /// <summary>
        /// Removes an object from the selection
        /// </summary>
        /// <param name="key">Key of the Object to Remove</param>
        [Command("remove-selectable", "Removes an object from the selection")]
        private static void RemoveSelection(string key)
        {
            if (SelectableObjects.ContainsKey(key))
                AConsoleManager.Instance.ObjectSelector.RemoveFromSelection(SelectableObjects[key]);
        }


        /// <summary>
        /// Lists all Selectable Objects in the Console
        /// </summary>
        [Command("list-selectable", "Lists all Selectable Objects", "al")]
        private static void ListSelectable()
        {
            string s = "Selectable Objects:";
            foreach (KeyValuePair<string, object> selectableObject in SelectableObjects)
            {
                s += "\n\t" + selectableObject.Key + " = " + selectableObject.Value;
            }
            Program.Logger.Log(s);
        }
    }
}