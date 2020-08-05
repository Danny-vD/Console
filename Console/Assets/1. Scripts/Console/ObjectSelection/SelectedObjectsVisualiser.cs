using System.Collections.Generic;
using Console.Core.ObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using VDUnityFramework.Standard.BaseClasses;
using VDUnityFramework.Standard.UnityExtensions;

namespace Console.ObjectSelection
{
    public class SelectedObjectsVisualiser : BetterMonoBehaviour
    {
        [SerializeField]
        private GameObject prefab = null;

        [SerializeField]
        private GameObject Window = null;

        public void Redraw(AObjectSelector selector)
        {
            CachedTransform.DestroyChildren();

            int count = 0;

            foreach (object obj in selector.SelectedObjects)
            {
                GameObject @object = obj as GameObject;
                if (@object == null)
                    continue;

                GameObject item = Instantiate(prefab, CachedTransform);
                item.GetComponentInChildren<Text>().text = $"{count}: {@object.name} [{@object.GetInstanceID()}]";

                Button button = item.GetComponentInChildren<Button>();
                button.onClick.AddListener(RemoveObject);

                ++count;

                void RemoveObject()
                {
                    selector.RemoveFromSelection(@object);
                    //objects.Remove(@object);
                    Destroy(item);
                    Redraw(selector);
                }
            }

            Window.SetActive(selector.SelectedObjects.Count > 0);
        }
    }
}