using System;

public abstract class MasterBase<T>
	where T : Enum
{
	public abstract T _typ { get; set; }
	public void SetTyp(int aIdx) => _typ = GetTyp(aIdx);
	public abstract T GetTyp(int aI);
}