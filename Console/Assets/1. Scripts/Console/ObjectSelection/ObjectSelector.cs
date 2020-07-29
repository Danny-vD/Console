using System.Collections.Generic;
using System.Linq;
using Console.Console;
using UnityEngine;
using VDFramework;

namespace Console.ObjectSelection
{
	public class ObjectSelector : BetterMonoBehaviour
	{
		[Tooltip("The camera to raycast from (defaults to Camera.Main)")]
		public Camera RaycastFrom;

		[SerializeField]
		private LayerMask SelectableLayers = default;

		[SerializeField]
		private SelectedObjectsVisualiser visualiser = null;
		
		public List<object> SelectedObjects => selectedObjects.Select(item => item as object).ToList();

		[SerializeField]
		private List<GameObject> selectedObjects;

		private void Awake()
		{
			selectedObjects = new List<GameObject>();

			if (RaycastFrom == null)
			{
				if (Camera.main == null)
				{
					ConsoleManager.LogError("No suitable Camera to raycast from");
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
					// default key to add to selection
					if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
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
			selectedObjects.RemoveAll(item => item == null);
			visualiser.Redraw(selectedObjects);
		}

		public void RemoveFromSelection(GameObject selectedObject)
		{
			selectedObjects.Remove(selectedObject);
			visualiser.Redraw(selectedObjects);
		}
		
		private void SelectObject(GameObject selectedObject)
		{
			selectedObjects.Clear();
			AddToSelection(selectedObject);
		}

		private void AddToSelection(GameObject selectedObject)
		{
			selectedObjects.Add(selectedObject);
			visualiser.Redraw(selectedObjects);
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
	}
}