using UnityEngine;

public class FrameTweenScale : FrameTweenVector3
{
	protected override Vector3 _targetVal { get => transform.localScale; set => transform.localScale = value; }
}
