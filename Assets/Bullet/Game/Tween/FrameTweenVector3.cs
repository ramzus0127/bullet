using Vector3 = UnityEngine.Vector3;

public abstract class FrameTweenVector3 : FrameTweenBase<Vector3>
{
	protected override Vector3 _lerp => Vector3.Lerp(_Start, _End, _evaluated);
}
