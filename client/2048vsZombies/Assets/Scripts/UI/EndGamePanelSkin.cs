using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
		UIManager.instance.effectLayer.gameObject.SetActive(false);
    }

    void OnRestartButtonClick( GameObject go )
	{
//        gameObject.SetActive(false);
//        Messenger.Broadcast(MessageConst.GAME_RESTART);
        Messenger.RemoveListener(MessageConst.GAME_OVER_START, OnGameOverStart);
		TurnManager.Reset();
        SleepyHippo.Util.GameObjectPool.Instance.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
