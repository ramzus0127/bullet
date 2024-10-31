using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler
{
	Canvas _Canvas;
	bool _CanDrag;
	Vector3 _LastMousePos;
	Vector3 _DeltaMousePos;
	Action _OnStartDrag;
	Action _OnEndDrag;

	void Awake()
	{
		_Canvas = GetComponentInParent<Canvas>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_CanDrag = true;
		_OnStartDrag?.Invoke();
	}

	public void Init(bool aIsDragImmediate, Action aOnStartDrag, Action aOnEndDrag) 
	{
		_OnStartDrag = aOnStartDrag;	
		_OnEndDrag = aOnEndDrag;
		if (aIsDragImmediate) _CanDrag = true;
	}

	void OnEnable()
	{
		_LastMousePos = Input.mousePosition;
	}

	void Update()
	{
		if (_CanDrag)
		{
			if (Input.GetMouseButtonUp(0))
			{
				_OnEndDrag?.Invoke();
				_CanDrag = false;
			}
		}

		_DeltaMousePos = Input.mousePosition - _LastMousePos;

		if (_CanDrag)
			transform.localPosition += _DeltaMousePos / _Canvas.scaleFactor;

		_LastMousePos = Input.mousePosition;
	}
}
