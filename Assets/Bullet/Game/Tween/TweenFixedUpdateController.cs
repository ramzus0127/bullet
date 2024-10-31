using Cysharp.Threading.Tasks;
using UnityEngine;

public class TweenFixedUpdateController : MonoBehaviour
{
	ITween _tween => GetComponent<ITween>();

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

	[ContextMenu("Play")]
	public void Play()
	{
		_tween.Play();
	}

	public UniTask PlayAsync()
	{
		return _tween.PlayAsync();
	}

	[ContextMenu("Rewind")]
	public void Rewind()
	{
		_tween.Rewind();
	}

	public UniTask RewindAsync()
	{
		return _tween.RewindAsync();
	}

	[ContextMenu("Stop")]
	public void Stop()
	{
		_tween.Stop();
	}
}
