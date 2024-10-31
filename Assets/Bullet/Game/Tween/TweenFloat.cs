using UnityEngine;

public abstract class TweenFloat : TweenFixedUpdateBase<float>
{
	protected override float _lerp => Mathf.Lerp(_Start, _End, _evaluated);
}
