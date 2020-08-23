using System.Collections.Generic;
using Console.Core;

namespace Console.Form
{
    /// <summary>
    /// Object Selector Implementation for Windows Form Programs
    /// </summary>
    public class FormObjSelector : AObjectSelector
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