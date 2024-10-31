using UnityEngine;

public class TweenScale : TweenVector3
{
	protected override Vector3 _targetVal { get => transform.localScale; set => transform.localScale = value; }
}
