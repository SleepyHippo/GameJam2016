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
	}

	public static void ShowMainUI()
	{
		instance.uiLayer.gameObject.SetActive(true);
	}

	public static void HideMainUI()
	{
		instance.uiLayer.gameObject.SetActive(false);
	}

	public static void ShowSceneUI()
	{
		instance.sceneLayer.gameObject.SetActive(true);
	}

	public static void HideSceneUI()
	{
		instance.sceneLayer.gameObject.SetActive(false);
	}

    public UIPanel effectLayer;

	public UIPanel uiLayer;

	public UIPanel sceneLayer;

    public UIRoot uiRoot;

	public Camera uiCamera;

	public MainUISkin mainUISkin;

	public StartUIPanelSkin startUISkin;

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