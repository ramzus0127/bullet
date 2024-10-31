using UnityEngine;

public abstract class BattleViewBase : MonoBehaviour
{
	protected Rent _Rent;
	public abstract bool _remove { get; }
	public abstract void UpdateView();

	protected void Init() 
	{
		_Rent = GetComponent<Rent>();
		UpdateView();
	}

	public virtual bool Return()
	{
		var ret = _remove;
		if (ret)
		{
			_Rent.Return();
			_Rent = null;
		}
		return ret;
	}

	public void ForceReturn()
	{
		_Rent.Return();
		_Rent = null;
	}
}
