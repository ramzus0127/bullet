public class UnitViewRoot : BattleViewRootBase<UnitView>
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
		var rent = ResourceManager.RentUnit(aUnit._master._typ);
		rent.gameObject.SetActive(true);
		rent.transform.SetParent(transform, false);

		var view = rent.GetComponent<UnitView>();
		view.Init(aUnit);
		_Views.Add(view);
	}
}
