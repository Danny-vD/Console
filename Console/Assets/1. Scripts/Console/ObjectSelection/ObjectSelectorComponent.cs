using System.Collections.Generic;
using System.Linq;
using Console.Console;
using UnityEngine;
using VDUnityFramework.Standard.BaseClasses;

namespace Console.ObjectSelection
{
    public class ObjectSelectorComponent : BetterMonoBehaviour
    {
        public UnityObjectSelector Selector { get; private set; }

        [Tooltip("The camera to raycast from (defaults to Camera.Main)")]
        public Camera RaycastFrom;

        [SerializeField, Tooltip("Optional: A script that will visualise all selected objects")]
        private SelectedObjectsVisualiser visualiser = null;

        [SerializeField, Tooltip("Keep in mind that you can't select Screen space overlay canvases, no matter what")]
        private LayerMask SelectableLayers = default;

        [SerializeField,
         Tooltip("You need to press at least 1 of these keys to Add to the selection, instead of override it")]
        private List<KeyCode> AddToSelectionKeys = new List<KeyCode>() { KeyCode.LeftControl, KeyCode.RightControl };
        

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

        public void RemoveFromSelection(GameObject selectedObject)
        {
            Selector.SelectedObjects.Remove(selectedObject);
            RedrawVisualiser();
        }

        private void SelectObject(GameObject selectedObject)
        {
            Selector.SelectedObjects.Clear();
            AddToSelection(selectedObject);
        }

        private void AddToSelection(GameObject selectedObject)
        {
            Selector.SelectedObjects.Add(selectedObject);
            RedrawVisualiser();
        }

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

        private void RedrawVisualiser()
        {
            if (visualiser)
            {
                visualiser.Redraw(Selector);
            }
        }
    }
}