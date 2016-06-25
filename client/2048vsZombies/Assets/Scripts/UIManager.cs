using UnityEngine;
using System.Collections;

public class UIManager:MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager instance
    {
		get
		{
			return _instance;
		}
    }

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
        startUISkin.Init();
        mainUISkin.Init();
        endGameUISkin.Init();
	}

    public UIPanel effectLayer;

	public UIPanel uiLayer;

	public UIPanel sceneLayer;

    public UIRoot uiRoot;

	public Camera uiCamera;

	public MainUISkin mainUISkin;

    public StartUIPanelSkin startUISkin;

    public EndGamePanelSkin endGameUISkin;

	private Camera _mainCamera;

	public Camera mainCamera
	{
		get
		{
			if (!_mainCamera)
			{
				_mainCamera = Camera.main;
			}
			return _mainCamera;
		}
	}
}