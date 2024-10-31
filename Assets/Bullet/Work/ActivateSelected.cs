#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;

public static class ActivateSelected
{
	[InitializeOnLoadMethod]
	static void Init() 
	{
		Selection.selectionChanged += () =>
		{
			if (!EditorPrefs.GetBool("ActivateSelected", false)) return;

			if (Selection.activeGameObject)
			{
				if (Selection.activeGameObject.transform.parent)
				{
					var parent = Selection.activeGameObject.transform.parent;
					for (int i = 0; i < parent.childCount; i++)
					{
						parent.GetChild(i).gameObject.SetActive(false);
					}
				}
				else
				{
					var rootGos = SceneManager.GetActiveScene().GetRootGameObjects();
					for (int i = 0; i < rootGos.Length; i++) 
					{
						rootGos[i].SetActive(false);
					}
				}
				Selection.activeGameObject.SetActive(true);
			}
		};
	}

	[MenuItem("Work/ActivateSelected")]
	static void ActivateSelectedToggle()
	{
		EditorPrefs.SetBool("ActivateSelected", !EditorPrefs.GetBool("ActivateSelected", false));
	}

	[MenuItem("Work/ActivateSelected", true)]
	static bool ActivateSelectedToggleValidate()
	{
		Menu.SetChecked("Work/ActivateSelected", EditorPrefs.GetBool("ActivateSelected", false));
		return true;
	}
}
#endif