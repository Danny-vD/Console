using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.Utils;

namespace Console.Core.Commands.BuiltIn
{
    public class ObjectSelectionCommands
    {
        public static void AddSelectionCommands()
        {
            CommandAttributeUtils.AddCommands<ObjectSelectionCommands>();
        }

        #region List Selection

        [Command("list-selection", "Lists all Selected Objects", "sl")]
        private static void ListSelected()
        {
            string s = "Selected Objects:\n\t";
            for (int i = 0; i < AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count; i++)
            {
                s += AConsoleManager.Instance.ObjectSelector.SelectedObjects[i].ToString();
                if (i != AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count - 1)
                {
                    s += "\n\t";
                }
            }
            AConsoleManager.Instance.Log(s);
        }

        #endregion

        #region Clear

        [Command("clear-selection", "Clears all Selected Objects", "sclear")]
        public static void ClearSelection()
        {
            AConsoleManager.Instance.Log("Cleared Selected Objects");
            AConsoleManager.Instance.ObjectSelector.ClearSelection();
        }

        #endregion
    }
}