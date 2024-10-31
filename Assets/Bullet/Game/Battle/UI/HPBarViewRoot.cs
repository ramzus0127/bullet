using static ResourceManager;

public class HPBarViewRoot : BattleViewRootBase<HPBarView>
{
	public void Init(BattleModel aBattle) 
	{
		var units = aBattle._p1._units;
		for (int i = 0; i < units.Count; i++)
		{
			var unit = units[i];
			Add(unit);
		}

		units = aBattle._p2._units;
		for (int i = 0; i < units.Count; i++)
		{
			var unit = units[i];
			Add(unit);
		}
	}

	public void Add(UnitModel aUnit) 
	{
		var view = RentUI<HPBarView>(UIPrefabTyp.HPBarView);
		view.transform.SetParent(transform, false);
		view.Init(aUnit);
		view.gameObject.SetActive(true);
		_Views.Add(view);
	}
}
