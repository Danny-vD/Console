using System.Collections.Generic;

namespace Console.Core.ObjectSelection
{
    public abstract class AObjectSelector
    {
        public abstract List<object> SelectedObjects { get; }

        public virtual void RemoveFromSelection(object obj)
        {
            SelectedObjects.Remove(obj);
        }

        public virtual void AddToSelection(object obj)
        {
            SelectedObjects.Add(obj);
        }

        public virtual void CheckValid()
        {
            SelectedObjects.RemoveAll(x => x == null);
        }

        public virtual void SelectObject(object obj)
        {
            SelectedObjects.Clear();
            AddToSelection(obj);
        }
    }

    
}