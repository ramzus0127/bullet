using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField]
	Vector3 _Speed;

	public void FixedUpdate() 
	{
		transform.localEulerAngles = _Speed * FrameManager._frame;
	}
}
