using UnityEngine;

public class GridRoot : MonoBehaviour
{
	[SerializeField]
	Vector2 _Inter;

	[SerializeField]
	int _MaxX;

	void OnValidate()
	{
		name = GetType().Name;

		for (int i = 0; i < transform.childCount; i++)
		{
			var x = (i % _MaxX) * _Inter.x;
			var y = (i / _MaxX) * _Inter.y;

			transform.GetChild(i).localPosition = new Vector2(x, y);
		}
	}
}
