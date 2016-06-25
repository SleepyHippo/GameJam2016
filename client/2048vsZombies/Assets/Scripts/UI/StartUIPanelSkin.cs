using UnityEngine;
using System.Collections;

public class StartUIPanelSkin : MonoBehaviour {

	public GameObject startButton;

	void Awake()
	{
		UIEventListener.Get(startButton).onClick = OnStartButtonClick;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnStartButtonClick( GameObject go )
	{
		Messenger.Broadcast(MessageConst.MOVIE_START);
	}
}
