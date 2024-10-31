#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SelectParent : MonoBehaviour
{
	void OnDrawGizmosSelected()
	{
		if (Selection.activeTransform == transform)
			Selection.activeTransform = transform.parent;
	}
}
#endif