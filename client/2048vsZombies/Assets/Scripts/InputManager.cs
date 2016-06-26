using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    bool active;

    bool startSwipe = false;
    Vector3 startSwipPosition;
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

//    void EasyTouch_On_Swipe (Gesture gesture)
//    {
//        if(gesture.swipe == EasyTouch.SwipeDirection.Left)
//        {
//            Messenger.Broadcast(MessageConst.INPUT_LEFT);
//        }
//        if(gesture.swipe == EasyTouch.SwipeDirection.Right)
//        {
//            Messenger.Broadcast(MessageConst.INPUT_RIGHT);
//        }
//        if(gesture.swipe == EasyTouch.SwipeDirection.Up)
//        {
//            Messenger.Broadcast(MessageConst.INPUT_UP);
//        }
//        if(gesture.swipe == EasyTouch.SwipeDirection.Down)
//        {
//            Messenger.Broadcast(MessageConst.INPUT_DOWN);
//        }
//    }

	// Update is called once per frame
	void Update () {
        if(active)
        {
            if(Input.GetMouseButtonDown(0))
            {
                startSwipe = true;
                startSwipPosition = Input.mousePosition;
            }
            if(Input.GetMouseButtonUp(0))
            {
                startSwipe = false;
                Vector3 delta = Input.mousePosition - startSwipPosition;
                if(delta.x > 0 && delta.x > Mathf.Abs(delta.y))
                    Messenger.Broadcast(MessageConst.INPUT_RIGHT);
                else if(delta.x < 0 && Mathf.Abs(delta.x) > delta.y)
                    Messenger.Broadcast(MessageConst.INPUT_LEFT);
                else if(delta.y > 0 && delta.y > Mathf.Abs(delta.x))
                    Messenger.Broadcast(MessageConst.INPUT_UP);
                else if(delta.y < 0 && Mathf.Abs(delta.y) > delta.x)
                    Messenger.Broadcast(MessageConst.INPUT_DOWN);
            }
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
