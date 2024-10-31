using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
	public enum Layer 
	{
		MostBackUI,
		MostBack3DUI,
		MostBackUI2,
		BackUI,
		Back3DUI,
		BackUI2,
		MiddleUI,
		Middle3DUI,
		MiddleUI2,
		FrontUI,
		Front3DUI,
		FrontUI2,
		MostFrontUI,
		MostFront3DUI,
		MostFrontUI2,
	}

	[Serializable]
	public class CanvasCamera 
	{
		public Layer _Layer;
		public Canvas _Canvas;
		public int _OrderInLayer; 
	}

	[UnityEngine.Serialization.FormerlySerializedAs("_CanvasCameraList")]
	[SerializeField]
	CanvasCamera[] _CanvasCameras;

	[SerializeField]
	Camera _BaseCamera;

	Camera[] _Cameras;

	void OnValidate()
	{
		name = GetType().Name;
	}

#if UNITY_EDITOR
	[ContextMenu("Setup")]
	void Setup()
	{
		_Cameras = GetComponentsInChildren<Camera>();

		for (int i = 0; i < _Cameras.Length; i++)
		{
			DestroyImmediate(_Cameras[i].gameObject);
		}

		var enums = Enum.GetValues(typeof(Layer));

		foreach (var item in enums)
		{
			var name = item.ToString();
			var go = new GameObject(name, typeof(Camera));
			go.transform.SetParent(transform, false);
			go.GetComponent<Camera>();
		}

		_Cameras = GetComponentsInChildren<Camera>();
		Camera baseCam;
		if (_BaseCamera)
		{
			baseCam = _BaseCamera;
		}
		else
		{ 
			baseCam = _Cameras[0];
			baseCam.cullingMask = 1 << LayerMask.NameToLayer(((Layer)0).ToString());
		}

		baseCam.gameObject.tag = "MainCamera";
		baseCam.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;

		for (int i = 0; i < _Cameras.Length; i++)
		{
			var cam = _Cameras[i];
			if (cam != baseCam)
			{
				cam.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
				baseCam.GetUniversalAdditionalCameraData().cameraStack.Add(cam);
				cam.cullingMask = 1 << LayerMask.NameToLayer(((Layer)i).ToString());
			}
		}

		_Cameras = GetComponentsInChildren<Camera>();

		for (int iii = 0; iii < _CanvasCameras.Length; iii++)
		{
			var canvasCamera = _CanvasCameras[iii];
			canvasCamera._Canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvasCamera._Canvas.worldCamera = _Cameras[(int)canvasCamera._Layer];
			canvasCamera._Canvas.name = $"Canvas{canvasCamera._Layer}";
			canvasCamera._Canvas.sortingOrder = canvasCamera._OrderInLayer;
			canvasCamera._Canvas.gameObject.layer = LayerMask.NameToLayer(canvasCamera._Layer.ToString());
		}

		var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(gameObject.scene.path);
		Debug.Log(obj);
		EditorUtility.SetDirty(obj);
	}
#endif
}
