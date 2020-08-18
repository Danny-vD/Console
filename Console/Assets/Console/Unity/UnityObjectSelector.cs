using System.Collections.Generic;
using Console.Core;

namespace Console.Unity
{

    /// <summary>
    /// AObjectSelector Implementation
    /// </summary>
    public class UnityObjectSelector : AObjectSelector
    {

        /// <summary>
        /// The Selected Objects
        /// </summary>
        public override List<object> SelectedObjects { get; } = new List<object>();
    }
}