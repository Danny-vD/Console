using System.Collections.Generic;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.Utils;

namespace Console.CLI
{
    public class CLIObjectSelectionCommands
    {
        private static Dictionary<string, object> SelectableObjects =>
            (AConsoleManager.Instance.ObjectSelector is CLIObjSelector os)
                ? os.SelectableObjects
                : new Dictionary<string, object>();
        public static void AddSelectionCommands()
        {
            CommandAttributeUtils.AddCommands<CLIObjectSelectionCommands>();
        }



        [Command("add-selectable", "Adds an object to the selection")]
        private static void Select(string key)
        {
            if (SelectableObjects.ContainsKey(key))
            {
                AConsoleManager.Instance.Log("Selecting Object: " + SelectableObjects[key]);
                AConsoleManager.Instance.ObjectSelector.AddToSelection(SelectableObjects[key]);
                AConsoleManager.Instance.Log("Selected Objects: " + AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count);
            }
        }



        [Command("select-selectable", "Select an object")]
        private static void SelectOne(string key)
        {
            AConsoleManager.Instance.ObjectSelector.ClearSelection();
            Select(key);
        }

        [Command("remove-selectable", "Removes an object from the selection")]
        private static void RemoveSelection(string key)
        {
            if (SelectableObjects.ContainsKey(key))
                AConsoleManager.Instance.ObjectSelector.RemoveFromSelection(SelectableObjects[key]);
        }


        [Command("list-selectable", "Lists all Selectable Objects", "al")]
        private static void ListSelectable()
        {
            string s = "Selectable Objects:";
            foreach (KeyValuePair<string, object> selectableObject in SelectableObjects)
            {
                s += "\n\t" + selectableObject.Key + " = " + selectableObject.Value;
            }
            AConsoleManager.Instance.Log(s);
        }
    }
}