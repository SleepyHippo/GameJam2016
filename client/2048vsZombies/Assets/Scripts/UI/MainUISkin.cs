using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainUISkin : MonoBehaviour 
{
	public UILabel turnLabel;

	public SkillIconSkin skillBingdong;
	public SkillIconSkin skillBaozha;
	public SkillIconSkin skillChuantou;

	private int bingdongCD = 20;
	private int baozhaCD = 15;
	private int chuantouCD = 10;


    public void Init()
	{
		if(null != skillBingdong)
		{
			//TODO 数据套入
			InitSkillIcon(skillBingdong, "bingdong", bingdongCD, bingdongCD);
			UIEventListener.Get(skillBingdong.gameObject).onClick = OnBingdongBtnClick;
		}

		if(null != skillBaozha)
		{
			InitSkillIcon(skillBaozha, "baozha", baozhaCD, baozhaCD);
			UIEventListener.Get(skillBaozha.gameObject).onClick = OnBaozhaBtnClick;
		}

		if(null != skillChuantou)
		{
			InitSkillIcon(skillChuantou, "chuantou", chuantouCD, chuantouCD);
			UIEventListener.Get(skillChuantou.gameObject).onClick = OnChuantouBtnClick;
		}

        Messenger.AddListener(MessageConst.GAME_START, OnGameStart);
        Messenger.AddListener(MessageConst.GAME_OVER_START, OnGameOverStart);
        Messenger<int>.AddListener(MessageConst.TURN_START, OnTurnStart);
//        Messenger<int>.AddListener(MessageConst.TURN_END, OnTurnEnd);
	}

    void OnGameStart()
    {
        gameObject.SetActive(true);
    }

    void OnGameOverStart()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(MessageConst.GAME_START, OnGameStart);
        Messenger.RemoveListener(MessageConst.GAME_OVER_START, OnGameOverStart);
        Messenger<int>.RemoveListener(MessageConst.TURN_START, OnTurnStart);
//		Messenger<int>.RemoveListener(MessageConst.TURN_END, OnTurnEnd);
    }

	void OnBingdongBtnClick(GameObject go)
    {
        if (!skillBingdong.isCD)
        {
            skillBingdong.OnSkillClick();
            Messenger.Broadcast(MessageConst.SKILL_ICE);
        }
    }

    void OnBaozhaBtnClick(GameObject go)
    {
        if (!skillBaozha.isCD)
        {
            skillBaozha.OnSkillClick();
            Messenger.Broadcast(MessageConst.SKILL_EXPLODE);
        }
    }

    void OnChuantouBtnClick(GameObject go)
    {
        if (!skillChuantou.isCD)
        {
            skillChuantou.OnSkillClick();
            Messenger.Broadcast(MessageConst.SKILL_THROUGH);
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

		turnLabel.text = (turn - 1).ToString();
	}

	private void InitSkillIcon(SkillIconSkin skin, string spriteName, int remainCount, int cd)
	{
		skin.OnInit(spriteName, remainCount, cd);
	}
}
