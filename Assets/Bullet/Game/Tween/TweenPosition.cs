using UnityEngine;

public class TweenPosition : TweenVector3
{
	protected override Vector3 _targetVal { get => transform.localPosition; set => transform.localPosition = value; }
}
