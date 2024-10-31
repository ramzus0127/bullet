using UnityEngine;

public class FrameTweenAlpha : FrameTweenFloat
{
	CanvasGroup _CanvasGroup;

	protected override float _targetVal { get => _CanvasGroup.alpha; set => _CanvasGroup.alpha = value; }

	protected override void Awake()
	{
		base.Awake();
		_CanvasGroup = GetComponent<CanvasGroup>();
	}
}
