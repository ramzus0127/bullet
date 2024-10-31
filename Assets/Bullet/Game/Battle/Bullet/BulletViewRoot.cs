public class BulletViewRoot : BattleViewRootBase<BulletView>
{
	public void Init(BattleModel aBattle) 
	{
		var bulls = aBattle._p1._bulls;
		for (int i = 0; i < bulls.Count; i++)
		{
			var bull = bulls[i];
			Add(bull, true);
		}

		bulls = aBattle._p2._bulls;
		for (int i = 0; i < bulls.Count; i++)
		{
			var bull = bulls[i];
			Add(bull, false);
		}
	}

	public void Add(BulletModel aBull, bool aIsP1) 
	{
		var rent = ResourceManager.RentBull(aBull._master._prefabTyp, aIsP1);
		rent.gameObject.SetActive(true);
		rent.transform.SetParent(transform, false);

		var view = rent.GetComponent<BulletView>();
		view.Init(aBull);
		_Views.Add(view);
	}
}
