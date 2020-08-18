using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Console.Unity.Components
{
    /// <summary>
    /// Object Selector Component, Acts as Container for the Selector Instance.
    /// Implements Unity Specific Logic
    /// </summary>
    public class ObjectSelectorComponent : MonoBehaviour
    {
        /// <summary>
        /// The Selector Instance
        /// </summary>
        public UnityObjectSelector Selector { get; private set; }

        /// <summary>
        /// The Origin of the Raycast
        /// </summary>
        [Tooltip("The camera to raycast from (defaults to Camera.Main)")]
        public Camera RaycastFrom;

        /// <summary>
        /// The Visualizer instance
        /// </summary>
        [SerializeField, Tooltip("Optional: A script that will visualise all selected objects")]
        private SelectedObjectsVisualiser visualiser = null;

        /// <summary>
        /// The Layers that are selectable with the UnityObjectSelector
        /// </summary>
        [SerializeField, Tooltip("Keep in mind that you can't select Screen space overlay canvases, no matter what")]
        private LayerMask SelectableLayers = default;

        /// <summary>
        /// The Keys that are used to add to the selection
        /// </summary>
        [SerializeField,
         Tooltip("You need to press at least 1 of these keys to Add to the selection, instead of override it")]
        private List<KeyCode> AddToSelectionKeys = new List<KeyCode>() { KeyCode.LeftControl, KeyCode.RightControl };
        
        /// <summary>
        /// Awake initializes the Selector.
        /// </summary>
        private void Awake()
        {
            Selector = new UnityObjectSelector();
            if (RaycastFrom == null)
            {
                if (Camera.main == null)
                {
                    ConsoleManagerComponent.LogError("No suitable Camera to raycast from");
                    return;
                }

                RaycastFrom = Camera.main;
            }
        }

        /// <summary>
        /// Update logic for the object selector
        /// Raycasting/...
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // left click
            {
                if (RaycastFrom == null)
                {
                    return;
                }

                if (RayCast(out GameObject hitObject))
                {
                    if (AddToSelectionKeys.Any(Input.GetKey))
                    {
                        AddToSelection(hitObject);
                    }
                    else
                    {
                        SelectObject(hitObject);
                    }
                }
            }
        }

        /// <summary>
        /// Force the objectSelector to loop over the list and check if all objects still exist
        /// </summary>
        public void CheckValid()
        {
            Selector.SelectedObjects.RemoveAll(item => item == null);
            RedrawVisualiser();
        }

        /// <summary>
        /// Removes the Selected Object from the Selection
        /// </summary>
        /// <param name="selectedObject">The object to remove</param>
        public void RemoveFromSelection(GameObject selectedObject)
        {
            Selector.SelectedObjects.Remove(selectedObject);
            RedrawVisualiser();
        }

        /// <summary>
        /// Makes this Object the only selected object
        /// </summary>
        /// <param name="selectedObject">The object to select</param>
        private void SelectObject(GameObject selectedObject)
        {
            Selector.SelectedObjects.Clear();
            AddToSelection(selectedObject);
        }

        /// <summary>
        /// Adds this Object to the Selection
        /// </summary>
        /// <param name="selectedObject">Object to Add</param>
        private void AddToSelection(GameObject selectedObject)
        {
            Selector.SelectedObjects.Add(selectedObject);
            RedrawVisualiser();
        }

        /// <summary>
        /// Raycasts the Ray from Camera to Mouse Position.
        /// </summary>
        /// <param name="objectHit">The object that has been hit by the ray</param>
        /// <returns>True if any object has been hit</returns>
        private bool RayCast(out GameObject objectHit)
        {
            Ray ray = RaycastFrom.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, SelectableLayers))
            {
                objectHit = hit.transform.gameObject;
                return true;
            }

            objectHit = null;
            return false;
        }

        /// <summary>
        /// Updates the ModelView with the Model
        /// </summary>
        private void RedrawVisualiser()
        {
            if (visualiser)
            {
                visualiser.Redraw(Selector);
            }
        }
    }
}