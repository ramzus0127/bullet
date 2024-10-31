using System.Collections.Generic;
using UnityEngine;

public abstract class HomeUIBase : MonoBehaviour
{
	[UnityEngine.Serialization.FormerlySerializedAs("tweenControllers")]
	[SerializeField]
	protected List<TweenFixedUpdateController> _TweenControllers;

	public virtual void Show() 
	{
		for (int i = 0; i < _TweenControllers.Count; i++) 
		{
			_TweenControllers[i].Play();
		}
		OnShow();
	}

	public virtual void Hide() 
	{
		for (int i = 0; i < _TweenControllers.Count; i++)
		{
			_TweenControllers[i].Rewind();
		}
		OnHide();
	}

	protected virtual void OnShow() { }
	protected virtual void OnHide() { }

}

