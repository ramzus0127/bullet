using UnityEngine;

interface ITweenBase
{
	void SetStart();
	void SetEnd();
	void SetAtStart();
	void SetAtEnd();
}

public abstract class TweenBase<T> : MonoBehaviour, ITweenBase
{
    [SerializeField]
    protected CurveAsset _Curve;

    [SerializeField]
    protected T _Start;

	[SerializeField]
	protected T _End;

	protected abstract T _targetVal { get; set; }

	protected abstract T _lerp { get; }

	protected abstract float _evaluateTime { get; }

	protected float _evaluated => _Curve.Evaluate(_evaluateTime);

	public virtual void SetStart() => _Start = _targetVal;

	public virtual void SetEnd() => _End = _targetVal;

	public abstract void SetAtStart();

	public abstract void SetAtEnd();

	protected void Apply()
	{
		_targetVal = _lerp;
	}
}
