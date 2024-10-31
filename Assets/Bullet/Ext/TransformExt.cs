using UnityEngine;

public static class TransformExt
{
	public static void SetLayer(this Transform aSelf, string aLayerName) 
	{
		var tras = aSelf.GetComponentsInChildren<Transform>();
		for (int i = 0; i < tras.Length; i++)
		{
			tras[i].gameObject.layer = LayerMask.NameToLayer(aLayerName);
		}
	}
}
