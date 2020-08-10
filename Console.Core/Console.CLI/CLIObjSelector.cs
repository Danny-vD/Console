using System.Collections.Generic;
using Console.Core.ObjectSelection;

namespace Console.CLI
{
    public class CLIObjSelector : AObjectSelector
    {
        public Dictionary<string, object> SelectableObjects = new Dictionary<string, object>();
        public override List<object> SelectedObjects { get; } = new List<object>();
    }
}