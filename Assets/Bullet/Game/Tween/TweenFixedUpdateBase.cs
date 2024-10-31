using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

interface ITween : ITweenBase
{
	void Play(Action aOnDone = null);
	UniTask PlayAsync(Action aOnDone = null);
	void Rewind(Action aOnDone = null);
	UniTask RewindAsync(Action aOnDone = null);
	void Stop();
}

[RequireComponent(typeof(TweenFixedUpdateController))]
public abstract class TweenFixedUpdateBase<T> : TweenBase<T>, ITween
{
	float _CurTime;
	float _EndTime;
	float _Dir;

	CancellationTokenSource _Cancel;

	protected override float _evaluateTime => _CurTime;

	Action _OnPlayDone;
	Action _OnRewindDone;

	TweenFixedUpdateController _Controller;
	public TweenFixedUpdateController _controller => _Controller;

	void OnValidate()
	{
		if (!_Curve) return;
		_EndTime = _Curve._time;
	}

	protected virtual void Awake()
	{
		_EndTime = _Curve._time;
		_Controller = GetComponent<TweenFixedUpdateController>();
	}

	void OnDestroy()
	{
		if (_Cancel != null)
			_Cancel.Cancel();
	}

	public void Cancel()
	{
		Stop();
		_Cancel.Cancel();
	}

	public override void SetAtStart()
	{
		_CurTime = 0f;
		_Dir = 0f;
		Apply();
	}

	public override void SetAtEnd()
	{
		_CurTime = _EndTime;
		_Dir = 0f;
		Apply();
	}

	public virtual void Play(Action aOnDone = null) 
	{
		_OnPlayDone = aOnDone;
		_Dir = 1f;
		enabled = true;
	}

	public async UniTask PlayAsync(Action aOnDone = null)
	{
		_OnPlayDone = aOnDone;
		_Dir = 1f;
		enabled = true;

		_Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(() => !enabled, cancellationToken: _Cancel.Token);
	}

	public virtual void Rewind(Action aOnDone = null)
	{
		_OnRewindDone = aOnDone;
		_Dir = -1f;
		enabled = true;
	}

	public async UniTask RewindAsync(Action aOnDone = null)
	{
		_OnRewindDone = aOnDone;
		_Dir = -1f;
		enabled = true;

		_Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(() => !enabled, cancellationToken: _Cancel.Token);
	}

	public virtual void Stop()
	{
		_Dir = 0f;
		enabled = false;
	}

	void FixedUpdate()
	{
		_CurTime += Time.fixedDeltaTime * _Dir;

		if (_CurTime >= _EndTime)
		{
			_OnPlayDone?.Invoke();
			enabled = false;
			_CurTime = _EndTime;
		}

		if (_CurTime <= 0f)
		{
			_OnRewindDone?.Invoke();
			enabled = false;
			_CurTime = 0f;
		}

		Apply();
	}
}
