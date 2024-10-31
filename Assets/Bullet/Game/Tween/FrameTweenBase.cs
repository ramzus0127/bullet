using UnityEngine;

interface IFrameTween : ITweenBase
{
	void UpdateFrame(int aFrame);
}

[RequireComponent(typeof(FrameTweenController))]
public abstract class FrameTweenBase<T> : TweenBase<T>, IFrameTween
{
	[SerializeField]
	int _StartFrame;

	protected int _CurFrame;
	protected int _EndFrame;

	const float FRAME_TIME = 1f / 60f;

	protected override float _evaluateTime => (_CurFrame - _StartFrame) / 60f;

	FrameTweenController _Controller;
	public FrameTweenController _controller => _Controller;

	void OnValidate()
	{
		if (!_Curve) return;
		_EndFrame = Mathf.CeilToInt(_Curve._time / FRAME_TIME) + _StartFrame;
	}

	protected virtual void Awake()
	{
		_EndFrame = Mathf.CeilToInt(_Curve._time / FRAME_TIME) + _StartFrame;
		_Controller = GetComponent<FrameTweenController>();
	}

	public void UpdateFrame(int aFrame)
	{
		if (aFrame < _StartFrame)
		{ 
			SetAtStart();
			return;
		}

		_CurFrame = aFrame;
		if (_CurFrame <= 0)
		{
			enabled = false;
			_CurFrame = 0;
		}

		if (_CurFrame >= _EndFrame)
		{
			enabled = false;
			_CurFrame = _EndFrame;
		}
		Apply();
	}

	public override void SetAtStart()
	{
		_CurFrame = 0;
		Apply();
	}

	public override void SetAtEnd()
	{
		_CurFrame = _EndFrame;
		Apply();
	}
}
