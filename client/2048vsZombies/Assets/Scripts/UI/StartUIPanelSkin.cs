using UnityEngine;
using System.Collections;

public class StartUIPanelSkin : MonoBehaviour {

	public GameObject startButton;
	public GameObject detailButton;

    public void Init()
	{
		Messenger.AddListener(MessageConst.SHOW_STARTUP_UI, OnShowStartupUI);
        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
		UIEventListener.Get(startButton).onClick = OnStartButtonClick;
		UIEventListener.Get(detailButton).onClick = OnDetailButtonClick;
        gameObject.SetActive(true);
	}

    void OnGameStart()
    {
        gameObject.SetActive(false);
    }

	void OnShowStartupUI()
	{
		gameObject.SetActive(true);
	}

    void OnDestroy()
    {
        Messenger.RemoveListener(MessageConst.GAME_START, OnGameStart);
		Messenger.RemoveListener(MessageConst.SHOW_STARTUP_UI, OnShowStartupUI);
    }

	void OnStartButtonClick( GameObject go )
	{
        Messenger.Broadcast(MessageConst.MOVIE_START);
        gameObject.SetActive(false);
//        GameManager.TestSetCameraEndPosition();
//        Messenger.Broadcast(MessageConst.GAME_START);
	}

	void OnDetailButtonClick( GameObject go )
	{
        gameObject.SetActive(false);
		Messenger.Broadcast(MessageConst.SHOW_TEAM_DETAIL);
	}
}
