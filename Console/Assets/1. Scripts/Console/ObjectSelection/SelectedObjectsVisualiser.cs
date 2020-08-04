using System.Collections.Generic;
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
		
		public void Redraw(List<GameObject> objects)
		{
			CachedTransform.DestroyChildren();

			int count = 0;

			foreach (GameObject @object in objects)
			{
				GameObject item = Instantiate(prefab, CachedTransform);
				item.GetComponentInChildren<Text>().text = $"{count}: {@object.name} [{@object.GetInstanceID()}]";

				Button button = item.GetComponentInChildren<Button>();
				button.onClick.AddListener(RemoveObject);

				++count;

				void RemoveObject()
				{
					objects.Remove(@object);
					Destroy(item);
					Redraw(objects);
				}
			}
			
			Window.SetActive(objects.Count > 0);
		}
	}
}