using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainUISkin : MonoBehaviour 
{
	public SkillIconSkin skillBingdong;
	public SkillIconSkin skillBaozha;
	public SkillIconSkin skillChuantou;

	void Awake()
	{
		if(null != skillBingdong)
		{
			//TODO 数据套入
			InitSkillIcon(skillBingdong, "bingdong", 0, 10);
			UIEventListener.Get(skillBingdong.gameObject).onClick = OnBingdongBtnClick;
		}

		if(null != skillBaozha)
		{
			InitSkillIcon(skillBaozha, "baozha", 0, 10);
			UIEventListener.Get(skillBaozha.gameObject).onClick = OnBaozhaBtnClick;
		}

		if(null != skillChuantou)
		{
			InitSkillIcon(skillChuantou, "chuantou", 0, 10);
			UIEventListener.Get(skillChuantou.gameObject).onClick = OnChuantouBtnClick;
		}

        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
        Messenger.AddListener(MessageConst.GAME_OVER_START, OnGameOverStart);
        Messenger<int>.AddListener(MessageConst.TURN_START, OnTurnStart);
	}

    void OnGameStart()
    {
        gameObject.SetActive(true);
    }

    void OnGameOverStart()
    {
        gameObject.SetActive(false);
    }

	void OnBingdongBtnClick(GameObject go)
    {
        if (!skillBingdong.isCD)
        {
            skillBingdong.OnSkillClick();
        }
    }

    void OnBaozhaBtnClick(GameObject go)
    {
        if (!skillBaozha.isCD)
        {
            skillBaozha.OnSkillClick();
        }
    }

    void OnChuantouBtnClick(GameObject go)
    {
        if (!skillChuantou.isCD)
        {
            skillChuantou.OnSkillClick();
        }
    }

	void OnTurnStart(int turn)
	{
		if(null != skillBingdong)
		{
			skillBingdong.OnTurnStart();
		}
		
		if(null != skillBaozha)
		{
			skillBaozha.OnTurnStart();
		}
		
		if(null != skillChuantou)
		{
			skillChuantou.OnTurnStart();
		}
	}

	private void InitSkillIcon(SkillIconSkin skin, string spriteName, int remainCount, int cd)
	{
		skin.OnInit(spriteName, remainCount, cd);
	}
}
