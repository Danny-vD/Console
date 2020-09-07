using System.Collections.Generic;

using Console.Core;

namespace Console.CLI
{
    /// <summary>
    /// Object Selector Implementation for the CLI
    /// </summary>
    public class CLIObjSelector : AObjectSelector
    {

        /// <summary>
        /// All Selectable Objects.
        /// Keyed by String.
        /// </summary>
        public Dictionary<string, object> SelectableObjects = new Dictionary<string, object>();

        /// <summary>
        /// The Selected Objects
        /// </summary>
        public override List<object> SelectedObjects { get; } = new List<object>();

    }
}