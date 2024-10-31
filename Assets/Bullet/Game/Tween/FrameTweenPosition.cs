using UnityEngine;

public class FrameTweenPosition : FrameTweenVector3
{
	protected override Vector3 _targetVal { get => transform.localPosition; set => transform.localPosition = value; }
}
