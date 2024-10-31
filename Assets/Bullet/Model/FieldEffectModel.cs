using System;

[Serializable]
public class FieldEffectModel
{
	public enum Type
	{
		None = -1,
		Missle,
		MagneticField,
	}

	public Type _Type;

	public string _name => _Type.ToString();

	FieldEffectModel()
	{
		_Type = Type.None;
	}
}

