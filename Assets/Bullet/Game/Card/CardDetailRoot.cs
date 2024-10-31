using UnityEngine;
using static ResourceManager;

public class CardDetailRoot : MonoBehaviour
{
	[SerializeField]
	Transform _CardDetailRootBack;

	[SerializeField]
	Transform _CardDetailRootFront;

	CardDetailView _CardDetailView;

	public void Open(CardModel aCard) 
	{
		if (_CardDetailView)
			_CardDetailView.Close();

		_CardDetailView = RentUI<CardDetailView>(UIPrefabTyp.CardDetailView);
		_CardDetailView.transform.SetParent(_CardDetailRootFront, false);
		_CardDetailView.Open(aCard, _CardDetailRootBack, res => 
		{
			if (_CardDetailView._id == res)
				_CardDetailView = null;
		});

		var tween = _CardDetailView.GetComponent<TweenPosition>();
		tween.Play();
	}

	public void Close() 
	{
		if (_CardDetailView)
			_CardDetailView.Close();
	}
}
