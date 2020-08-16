using System.Collections.Generic;

namespace Console.Core
{
    /// <summary>
    /// Selector that allows for selected objects to be passed to commands as well.
    /// </summary>
    public abstract class AObjectSelector
    {
        /// <summary>
        /// The Selected Objects
        /// </summary>
        public abstract List<object> SelectedObjects { get; }

        /// <summary>
        /// Clears the Selected Objects
        /// </summary>
        public virtual void ClearSelection() => SelectedObjects.Clear();

        /// <summary>
        /// Removes the Specified Object from the Selection
        /// </summary>
        /// <param name="obj">Object to Remove</param>
        public virtual void RemoveFromSelection(object obj) => SelectedObjects.Remove(obj);

        /// <summary>
        /// Adds the Specified object to the selection
        /// </summary>
        /// <param name="obj">Object to Add</param>
        public virtual void AddToSelection(object obj) => SelectedObjects.Add(obj);

        /// <summary>
        /// Checks all selected objects for their Validity and removes Invalid Objects from the List
        /// </summary>
        public virtual void CheckValid() => SelectedObjects.RemoveAll(x => x == null);

        /// <summary>
        /// Selects this object only.
        /// </summary>
        /// <param name="obj">Object to Select</param>
        public virtual void SelectObject(object obj)
        {
            SelectedObjects.Clear();
            AddToSelection(obj);
        }
    }

    
}