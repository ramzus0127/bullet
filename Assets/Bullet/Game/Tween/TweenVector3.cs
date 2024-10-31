using UnityEngine;

public abstract class TweenVector3 : TweenFixedUpdateBase<Vector3>
{
	protected override Vector3 _lerp => Vector3.Lerp(_Start, _End, _evaluated);
}
