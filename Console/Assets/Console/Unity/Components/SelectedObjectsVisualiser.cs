using Console.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Console.Unity.Components
{

    /// <summary>
    /// Visualizer UI For the Object Selector
    /// </summary>
    public class SelectedObjectsVisualiser : MonoBehaviour
    {
        /// <summary>
        /// Selected Object Display Prefab
        /// </summary>
        [SerializeField]
        private GameObject prefab = null;

        /// <summary>
        /// UI Window of the Visualizer
        /// </summary>
        [SerializeField]
        private GameObject Window = null;

        /// <summary>
        /// Destroys all children
        /// </summary>
        private static void DestroyChildren(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Updates the ModelView
        /// </summary>
        /// <param name="selector">Model</param>
        public void Redraw(AObjectSelector selector)
        {
            DestroyChildren(transform);

            int count = 0;

            foreach (object obj in selector.SelectedObjects)
            {
                GameObject @object = obj as GameObject;
                if (@object == null)
                    continue;

                GameObject item = Instantiate(prefab, transform);
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