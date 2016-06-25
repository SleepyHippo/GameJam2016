﻿using UnityEngine;
using System.Collections;

public class EndGamePanelSkin : MonoBehaviour {

    public UIPanel panel;
	public GameObject restartButton;

	public void Init()
	{
        Messenger.AddListener(MessageConst.GAME_OVER_START, OnGameOverStart);
        UIEventListener.Get(restartButton).onClick = OnRestartButtonClick;
	}

    void OnGameOverStart()
    {
        gameObject.SetActive(true);
    }

    void OnRestartButtonClick( GameObject go )
	{
        gameObject.SetActive(false);
        Messenger.Broadcast(MessageConst.GAME_RESTART);
	}
}
