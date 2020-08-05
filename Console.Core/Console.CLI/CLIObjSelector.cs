using System.Collections.Generic;
using Console.Core.ObjectSelection;

namespace Console.CLI
{
    public class CLIObjSelector : AObjectSelector
    {
        public override List<object> SelectedObjects { get; } = new List<object>();
    }
}