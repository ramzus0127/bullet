using UnityEngine;

public class FrameManager : Singleton<FrameManager>
{
	int _Frame;

	public static int _frame 
	{
		get
		{
			if (i == null)
				new GameObject("FrameManager", typeof(FrameManager));
			return i._Frame;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	void FixedUpdate()
	{
		_Frame++;
	}
}
