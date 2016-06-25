using UnityEngine;
using System.Collections;

public class StartUIPanelSkin : MonoBehaviour {

	public GameObject startButton;

    public void Init()
	{
        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
		UIEventListener.Get(startButton).onClick = OnStartButtonClick;
	}

    void OnGameStart()
    {
        gameObject.SetActive(false);
    }

	void OnStartButtonClick( GameObject go )
	{
//        Messenger.Broadcast(MessageConst.MOVIE_START);
        GameManager.TestSetCameraEndPosition();
        Messenger.Broadcast(MessageConst.GAME_START);
	}
}
