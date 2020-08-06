using System.Collections.Generic;
using Console.Core.ObjectSelection;

namespace Console.ObjectSelection
{
    public class UnityObjectSelector : AObjectSelector
    {
        public override List<object> SelectedObjects { get; } = new List<object>();
    }
}