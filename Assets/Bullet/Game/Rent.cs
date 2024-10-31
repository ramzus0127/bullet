using UnityEngine;

public class Rent : MonoBehaviour
{ 
	Pool _Pool;

	public int _type { get; private set; }

	public void OnRent(int aType, Pool aPool) 
	{
		_type = aType;
		_Pool = aPool;
	}

	public void Return() 
	{
		_Pool.Return(this);
	}
}
