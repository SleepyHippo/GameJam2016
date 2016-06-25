using UnityEngine;
using System.Collections;

public class StartUIPanelSkin : MonoBehaviour {

	public GameObject startButton;

	void Awake()
	{
        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
        Messenger.AddListener(MessageConst.GAME_RESTART, OnGameRestart);
		UIEventListener.Get(startButton).onClick = OnStartButtonClick;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		Messenger.Broadcast(MessageConst.MOVIE_START);
	}
}
