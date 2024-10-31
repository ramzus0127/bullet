using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongTap : MonoBehaviour, IPointerDownHandler
{
	[SerializeField]
	float _LongTapTime;

	[SerializeField]
	float _DragThreshold;

	bool _IsHold;

	float _CurTime;

	Action _OnInvoke;

	bool _IsInvoked;

	Vector3 _LastMousePos;
	Vector3 _DeltaMousePos;
	float _Drag;

	Canvas _Canvas;

	void Awake()
	{
		_Canvas = GetComponentInParent<Canvas>();
	}

	public void Init(float aLongTapTime, float aDragThreshold, Action aOnInvoke) 
	{
		_LongTapTime = aLongTapTime;
		_DragThreshold = aDragThreshold;
		_OnInvoke = aOnInvoke;
		_LastMousePos = Input.mousePosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_IsHold = true;
	}

	void Update()
	{
		if (!Input.GetMouseButton(0))
			_IsHold = false;

		if (_IsHold)
		{
			_DeltaMousePos = Input.mousePosition - _LastMousePos;
			_CurTime += Time.deltaTime;
			_Drag += _DeltaMousePos.magnitude / _Canvas.scaleFactor;
		}

		if (_Drag >= _DragThreshold)
			_IsHold = false;

		if (!_IsHold)
		{
			_Drag = 0f;
			_CurTime = 0f;
			_IsInvoked = false;
		}
		else
		{
			if (!_IsInvoked)
			{
				if (_CurTime >= _LongTapTime)
				{
					_IsInvoked = true;
					_OnInvoke();
				}
			}
		}

		_LastMousePos = Input.mousePosition;
	}
}
