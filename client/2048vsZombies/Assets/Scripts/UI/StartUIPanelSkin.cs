using UnityEngine;
using System.Collections;

public class StartUIPanelSkin : MonoBehaviour {

	public GameObject startButton;

    public void Init()
	{
        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
        Messenger.AddListener(MessageConst.GAME_RESTART, OnGameRestart);
		UIEventListener.Get(startButton).onClick = OnStartButtonClick;
	}

    void OnGameStart()
    {
        gameObject.SetActive(false);
    }

    void OnGameRestart()
    {
        gameObject.SetActive(true);
    }

	void OnStartButtonClick( GameObject go )
	{
//        Messenger.Broadcast(MessageConst.MOVIE_START);
        GameManager.TestSetCameraEndPosition();
        Messenger.Broadcast(MessageConst.GAME_START);
	}
}
