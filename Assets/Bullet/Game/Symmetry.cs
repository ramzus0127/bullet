using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Symmetry : MonoBehaviour
{
    [SerializeField]
    GameObject _Copy;

	[SerializeField]
	bool _Update;

	void Awake()
	{
		if (Application.isPlaying)
		{
			enabled = false;
			return;
		}
	}

	void Update()
    {
		if (_Update) 
		{
			_Update = false;

			if (_Copy)
				DestroyImmediate(_Copy);

			_Copy = Instantiate(gameObject, transform.parent);

			var scale = _Copy.transform.localScale;
			scale.x = -scale.x;
			_Copy.transform.localScale = scale;
		}
    }
}
