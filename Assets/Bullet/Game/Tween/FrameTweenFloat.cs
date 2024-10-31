using UnityEngine;

public abstract class FrameTweenFloat : FrameTweenBase<float>
{
	protected override float _lerp => Mathf.Lerp(_Start, _End, _evaluated);
}
