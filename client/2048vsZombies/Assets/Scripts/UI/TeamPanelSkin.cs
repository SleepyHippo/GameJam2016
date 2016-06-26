using UnityEngine;
using System.Collections;

public class TeamPanelSkin : MonoBehaviour {

	public GameObject backBtn;

	public void Init()
	{
		Messenger.AddListener(MessageConst.SHOW_TEAM_DETAIL, OnTeamDetail);
		UIEventListener.Get(backBtn).onClick = OnBackButtonClick;
		gameObject.SetActive(false);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener(MessageConst.SHOW_TEAM_DETAIL, OnTeamDetail);
	}

	void OnTeamDetail()
	{
		gameObject.SetActive(true);
	}

	void OnBackButtonClick(GameObject go)
	{
		gameObject.SetActive(false);
		Messenger.Broadcast(MessageConst.SHOW_STARTUP_UI);
	}
}
