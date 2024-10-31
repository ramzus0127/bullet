using UnityEngine;
using static BulletMaster;
using static CardMaster;
using static UnitMaster;

public class MasterManager : Singleton<MasterManager>
{
	[SerializeField]
	CardMaster[] _CardMasters;

	[SerializeField]
	UnitMaster[] _UnitMasters;

	[SerializeField]
	BulletMaster[] _BullMasters;

	public static CardMaster GetCardMaster(CardTyp aTyp) => i._CardMasters[(int)aTyp];

	public static UnitMaster GetUnitMaster(UnitTyp aTyp) => i._UnitMasters[(int)aTyp];

	public static BulletMaster GetBullMaster(BulletTyp aTyp) => i._BullMasters[(int)aTyp];

	void OnValidate()
	{
		name = GetType().Name;
		Init();
	}

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		Init();
	}

	void Init() 
	{
		for (int i = 0; i < _CardMasters.Length; i++) _CardMasters[i].SetTyp(i);
		for (int i = 0; i < _UnitMasters.Length; i++) _UnitMasters[i].SetTyp(i);
		for (int i = 0; i < _BullMasters.Length; i++) _BullMasters[i].SetTyp(i);
	}
}