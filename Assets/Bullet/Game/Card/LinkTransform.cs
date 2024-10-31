using UnityEngine;

public class LinkTransform : MonoBehaviour
{
	Transform _LinkTarget;
	Transform _OriParent;

	public void Link(Transform aLinkTarget, Transform aParent, Transform aOriParent) 
	{
		_LinkTarget = aLinkTarget;
		_OriParent = aOriParent;
		transform.SetParent(aParent);
	}

	public void Cancel() 
	{
		transform.SetParent(_OriParent);
	}

	void LateUpdate()
	{
		if (!_LinkTarget) return;
		transform.position = _LinkTarget.position;
	}
}
