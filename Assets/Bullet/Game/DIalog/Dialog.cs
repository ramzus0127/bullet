using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
	static Dialog i { get; set; }

	[SerializeField]
	Button _BtnOK;

	[SerializeField]
	Button _BtnCancel;

	[SerializeField]
	Button _BtnClose;

	[SerializeField]
	TextMeshProUGUI _TxtTitle;

	[SerializeField]
	TextMeshProUGUI _TxtMsg;

	ITween _Tween;

	Action _OnOK;
	Action _OnCancel;

	void OnValidate()
	{
		name = GetType().Name;  
	}

	void Awake()
	{
		i = this;
		_Tween = GetComponentInChildren<ITween>();
	}

	void Start()
	{
		_Tween.SetAtStart();
		gameObject.SetActive(false);
		_BtnOK.onClick.AddListener(OnOK);
		_BtnCancel.onClick.AddListener(OnCancel);
		_BtnClose.onClick.AddListener(OnClose);
	}

	void OnOK() 
	{
		_OnOK?.Invoke();
		Close();
	}

	void OnCancel()
	{
		_OnCancel?.Invoke();
		Close();
	}

	void OnClose() 
	{
		Close();
	}

	public static void OpenWithOK(string aTitle, string aMsg, Action aOnOk = null) 
	{
		i._OnOK = aOnOk;
		i._BtnOK.gameObject.SetActive(true);
		i._BtnCancel.gameObject.SetActive(false);
		OpenSub(aTitle, aMsg);
	}

	public static void OpenWithCancel(string aTitle, string aMsg, Action aOnCancel = null)
	{
		i._BtnOK.gameObject.SetActive(false);
		i._BtnCancel.gameObject.SetActive(true);
		i._OnCancel = aOnCancel;
		OpenSub(aTitle, aMsg);
	}

	public static void OpenWithOKAndCancel(string aTitle, string aMsg, Action aOnOk = null, Action aOnCancel = null)
	{
		i._BtnOK.gameObject.SetActive(true);
		i._BtnCancel.gameObject.SetActive(true);
		i._OnOK = aOnOk;
		i._OnCancel = aOnCancel;
		OpenSub(aTitle, aMsg);
	}

	public static void OpenSub(string aTitle, string aMsg)
	{
		i._TxtTitle.text = aTitle;
		i._TxtMsg.text = aMsg;
		i.gameObject.SetActive(true);
		i._Tween.Play();
	}

	void Close()
	{
		i._Tween.Rewind(() => 
		{
			gameObject.SetActive(false);
		});
	}
}
