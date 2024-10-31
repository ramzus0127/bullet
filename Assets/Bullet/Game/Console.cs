using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public class Console : MonoBehaviour
{
	[SerializeField]
	bool _Quick;

	[SerializeField]
	int _LogInter;

	int _logInter => _Quick ? 1 : _LogInter;

	[SerializeField]
	int _CaretBlinkInter;

	TextMeshProUGUI _TextLog;

	string _Log = "";
	string _LogOutput = "";
	int _Frame;

	string _Prompt = "";
	string _Caret = "";

	CancellationTokenSource _Cancel;

	void Awake()
	{
		_TextLog = GetComponent<TextMeshProUGUI>();
	}

	public void Init(string aPrompt, string aCaret) 
	{
		_Prompt = aPrompt;
		_Caret = aCaret;
	}

	public async UniTask PlayLogAsync(string aLog)
	{
		_Log = aLog;
		_Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(() => _LogOutput.Length == _Log.Length, cancellationToken: _Cancel.Token);
	}

	public void PlayLog(string aLog)
	{
		_Log = aLog;
	}

	public void Cancel() 
	{
		_Cancel.Cancel();
	}

	void OnDestroy()
	{
		if (_Cancel != null)
			_Cancel.Cancel();
	}

	public void ResetLog() 
	{
		_Log = "";
		_LogOutput = "";
	}

	void FixedUpdate()
	{
		if (_Frame / _CaretBlinkInter % 2 == 0)
			_Caret = "";
		else
			_Caret = "_";

		if (_Frame % _logInter == 0)
		{
			if (_LogOutput.Length < _Log.Length)
			{
				if (_Log.Length - _LogOutput.Length == 1)
					_LogOutput = _Log;
				else
					_LogOutput = _Log.Remove(_LogOutput.Length + 1);
			}
		}

		var logOutputs = _LogOutput.Split('\n');
		var logs = _Log.Split("\n");
		for (int i = 0; i < logOutputs.Length; i++)
		{
			if (logs.Length != 1 && logs[i].Length == 0) continue;
			if (logs[i].StartsWith(" ")) continue;

			logOutputs[i] = _Prompt + logOutputs[i];
		}

		var log =  string.Join('\n', logOutputs);
		_TextLog.text = log + _Caret;

		_Frame++;
	}
}

