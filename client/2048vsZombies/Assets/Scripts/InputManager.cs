using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    bool active;
	// Use this for initialization
	void Start () {
        active = false;
        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
        Messenger.AddListener(MessageConst.GAME_OVER_START, OnGameOver);
	}
	
    void OnDestroy()
    {
        Messenger.RemoveListener(MessageConst.GAME_START, OnGameStart);
        Messenger.RemoveListener(MessageConst.GAME_OVER_START, OnGameOver);
    }

	// Update is called once per frame
	void Update () {
        if(active)
        {
            if(Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Messenger.Broadcast(MessageConst.INPUT_LEFT);
            }
            if(Input.GetKeyUp(KeyCode.RightArrow))
            {
                Messenger.Broadcast(MessageConst.INPUT_RIGHT);
            }
            if(Input.GetKeyUp(KeyCode.UpArrow))
            {
                Messenger.Broadcast(MessageConst.INPUT_UP);
            }
            if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                Messenger.Broadcast(MessageConst.INPUT_DOWN);
            }
        }
	}

    void OnGameStart()
    {
        active = true;
    }

    void OnGameOver()
    {
        active = false;
    }
}
