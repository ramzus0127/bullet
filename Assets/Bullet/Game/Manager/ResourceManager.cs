using UnityEngine;
using static CardMaster;
using static UnitMaster;

public class ResourceManager : Singleton<ResourceManager>
{
	[SerializeField]
	Pool _UIPool;

	[SerializeField]
	Pool _Card3DModelPool;

	[SerializeField]
	Pool _UnitPool;

	[SerializeField]
	Pool _BullPool;

	[SerializeField]
	Pool _UnitPlacerPool;

	public enum UIPrefabTyp 
	{
		CardView,
		CardDetailView,
		HPBarView,
	}

	public enum BulletPrefabTyp
	{
		Laser,
		Beam,
	}

	void OnValidate()
	{
		name = GetType().Name;
	}

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	public static T RentUI<T>(UIPrefabTyp aTyp)
	{
		return i._UIPool.Rent((int)aTyp).GetComponent<T>();
	}

	public static Rent RentCard3DModel(CardTyp aTyp)
	{
		return i._Card3DModelPool.Rent((int)aTyp);
	}

	public static Rent RentUnit(UnitTyp aTyp)
	{
		return i._UnitPool.Rent((int)aTyp);
	}

	public static Rent RentBull(BulletPrefabTyp aTyp, bool aIsP1)
	{
		var typ = (int)aTyp;
		typ *= 2;
		typ += aIsP1 ? 0 : 1;
		return i._BullPool.Rent(typ);
	}

	public static Rent RentUnitPlacer(CardTyp aTyp)
	{
		return i._UnitPlacerPool.Rent((int)aTyp);
	}
}
