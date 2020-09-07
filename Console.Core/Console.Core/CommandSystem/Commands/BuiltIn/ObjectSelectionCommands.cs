using Console.Core.CommandSystem.Attributes;
using Console.Core.LogSystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Default command to clear and list all selected objects
    /// </summary>
    public class ObjectSelectionCommands
    {

        private static readonly ALogger SelectedObjectLogger = TypedLogger.CreateTypedWithPrefix("list-selection");

        private static readonly ALogger SelectedObjectClearLogger =
            TypedLogger.CreateTypedWithPrefix("clear-selection");

        /// <summary>
        /// Adds all Selection Commands
        /// </summary>
        public static void AddSelectionCommands()
        {
            CommandAttributeUtils.AddCommands<ObjectSelectionCommands>();
        }

        #region List Selection

        /// <summary>
        /// Prints the Selected objects.
        /// </summary>
        [Command(
            "list-selection",
            HelpMessage = "Lists all Selected Objects",
            Namespace = ConsoleCoreConfig.SELECTION_NAMESPACE,
            Aliases = new[] { "sl" }
        )]
        private static void ListSelected()
        {
            string s = "\nSelected Objects:\n\t";
            for (int i = 0; i < AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count; i++)
            {
                s += AConsoleManager.Instance.ObjectSelector.SelectedObjects[i].ToString();
                if (i != AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count - 1)
                {
                    s += "\n\t";
                }
            }

            SelectedObjectLogger.Log(s);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the Selected Objects List.
        /// </summary>
        [Command(
            "clear-selection",
            HelpMessage = "Clears all Selected Objects",
            Namespace = ConsoleCoreConfig.SELECTION_NAMESPACE,
            Aliases = new[] { "sclear" }
        )]
        public static void ClearSelection()
        {
            SelectedObjectClearLogger.Log("Cleared Selected Objects");
            AConsoleManager.Instance.ObjectSelector.ClearSelection();
        }

        #endregion

    }
}