using UnityEngine;

public class FrameTweenController : MonoBehaviour
{
	IFrameTween _tween => GetComponent<IFrameTween>();

	[ContextMenu("SetStart")]
	public void SetStart()
	{
		_tween.SetStart();
	}

	[ContextMenu("SetEnd")]
	public void SetEnd()
	{
		_tween.SetEnd();
	}

	[ContextMenu("SetAtStart")]
	public void SetAtStart()
	{
		_tween.SetAtStart();
	}

	[ContextMenu("SetAtEnd")]
	public void SetAtEnd()
	{
		_tween.SetAtEnd();
	}

	public void UpdateFrame(int aFrame) 
	{
		_tween.UpdateFrame(aFrame);
	}
}
