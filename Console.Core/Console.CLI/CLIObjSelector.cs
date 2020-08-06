using System.Collections.Generic;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Console;
using Console.Core.ObjectSelection;

namespace Console.CLI
{
    public class CLIObjSelector : AObjectSelector
    {

        [Command("add", "Adds an object to the selection")]
        private void Select(string key)
        {
            if (SelectableObjects.ContainsKey(key))
            {
                AConsoleManager.Instance.Log("Selecting Object: " + SelectableObjects[key]);
                SelectedObjects.Add(SelectableObjects[key]);
                AConsoleManager.Instance.Log("Selected Objects: " + SelectedObjects.Count);
            }
        }



        [Command("select", "Select an object")]
        private void SelectOne(string key)
        {
            ClearSelection();
            Select(key);
        }

        [Command("remove", "Removes an object from the selection")]
        private void RemoveSelection(string key)
        {
            if (SelectableObjects.ContainsKey(key))
                SelectedObjects.Remove(SelectableObjects[key]);
        }

        [Command("clear-selection", "Clears all Selected Objects", "sclear")]
        private void ClearSelection()
        {
            AConsoleManager.Instance.Log("Cleared Selected Objects");
            SelectedObjects.Clear();
        }


        [Command("list-selection", "Lists all Selected Objects", "sl")]
        private void ListSelected()
        {
            string s = "Selected Objects:\n\t";
            for (int i = 0; i < SelectedObjects.Count; i++)
            {
                s += SelectedObjects[i].ToString();
                if (i != SelectableObjects.Count - 1)
                {
                    s += "\n\t";
                }
            }
            AConsoleManager.Instance.Log(s);
        }

        [Command("list-selectable", "Lists all Selectable Objects", "al")]
        private void ListSelectable()
        {
            string s = "Selectable Objects:";
            foreach (KeyValuePair<string, object> selectableObject in SelectableObjects)
            {
                s += "\n\t" + selectableObject.Key + " = " + selectableObject.Value;
            }
            AConsoleManager.Instance.Log(s);
        }


        public CLIObjSelector()
        {
            CommandAttributeUtils.AddCommands(this);
        }


        public Dictionary<string, object> SelectableObjects = new Dictionary<string, object>();
        public override List<object> SelectedObjects { get; } = new List<object>();
    }
}