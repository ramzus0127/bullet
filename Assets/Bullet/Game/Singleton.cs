using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Singleton<T>
{ 
	static T _i;

	public static T i
	{
		get
		{
			if (Application.isPlaying) return _i;
			if (_i == null) _i = FindObjectOfType<T>();
			return _i;
		}
		set
		{
			_i = value;
		}
	}

	protected virtual void Awake() 
	{
		_i = (T)this;
	}
}

