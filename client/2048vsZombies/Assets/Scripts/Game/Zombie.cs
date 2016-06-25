// -----------------------------------------------------------------------------
//
//  Author :    Duke Zhou
//  Data :      2016/6/25
//
// -----------------------------------------------------------------------------
//

using UnityEngine;
using DG.Tweening;
using SleepyHippo.Util;
using System.Collections.Generic;

public class Zombie : DynamicItem
{
    public enum Skill
    {
        LockZone = 1
    }
    public Transform modelTransform;
    /// <summary>
    /// 移动间隔：几回合走一步
    /// </summary>
    public int moveInterval = 3;
    /// <summary>
    /// 每次移动走几格
    /// </summary>
    public int moveDistance = 1;

	/// <summary>
	/// 最大生命值
	/// </summary>
	public int maxHP;

    /// <summary>
    /// 生命值
    /// </summary>
    public int hp;

    /// <summary>
    /// 怪物技能，入场的时候触发
    /// </summary>
    public int skill;

    private int nowMoveInterval = -1;

    public bool readyToMove
    {
        get
        {
            return nowMoveInterval <= 0;
        }
    }

	public Transform progressAnchor;
	public GameObject plane;
	public MeshRenderer meshRenderer;

	private const string HPBarPath = "Prefabs/UI/HPBarComponent";
	private const string ICE_MATERIAL_PATH = "Materials/Ice";

	private HPProgressBarSkin hpProgressSkin;
	private Material _iceMaterial;
	private Material _originMaterial;

	private static GameObject _hpBarSource;

    void Awake()
    {
        nowMoveInterval = moveInterval;
    }

    public override void MoveDown(int distance, bool destroy = false)
    {
        PlayMoveAnimation();
        y -= moveDistance;
        Tweener tweener = gameObject.transform.DOMoveZ(gameObject.transform.position.z - moveDistance, 0.5f);
        if(destroy)
            tweener.OnComplete(OnComplete);
        nowMoveInterval = moveInterval;
    }

    public override void OnTick()
    {
        if(canMove)
        {
            nowMoveInterval--;
        }
    }

    public void MoveTo(Vector3 position)
    {
        x = (int)position.x;
        y = (int)position.z;
        PlayMoveAnimation();
        gameObject.transform.DOMove(position, 0.5f);
        nowMoveInterval = moveInterval;
    }

    public void TakeDamage(int damage, int buff)
    {
		int oldHP = hp;
		hp -= damage;
		RefreshHPBar(oldHP, hp < 0 ? 0: hp);
		if(hp <= 0)
		{
			Messenger<Zombie>.Broadcast(MessageConst.ZOMBIE_DIE, this);
		}
		else
		{
			PlayEffect(buff);
		}
    }

	private void PlayEffect(int buff)
	{
		GameObject source;
		List<Tower.Buff> buffList = CommonUtil.GetBuffList(buff);
		List<Tower.Buff>.Enumerator iter = buffList.GetEnumerator();
		
		while(iter.MoveNext())
		{
			switch(iter.Current)
			{
				case Tower.Buff.Ice:
					SetIceMaterialAndHideWu();
					break;
				case Tower.Buff.Through:
					break;
				case Tower.Buff.Explode:
					break;
			}
		}
		iter.Dispose();
	}

	private void SetIceMaterialAndHideWu()
	{
		if(null == meshRenderer)
		{
			return;
		}

		if(null == _originMaterial)
		{
			_originMaterial = meshRenderer.material;
		}

		_iceMaterial = _iceMaterial ?? Resources.Load<Material>(ICE_MATERIAL_PATH);
		if(null != _iceMaterial)
		{
			meshRenderer.material = _iceMaterial;
		}

		this.plane.SetActive(false);
	}

	private void RecoverMaterial()
	{
		if(null != meshRenderer && null != _originMaterial)
		{
			meshRenderer.material = _originMaterial;
		}

		this.plane.SetActive(true);
	}

    protected override void PlayMoveAnimation()
    {
        modelTransform.DOLocalJump(Vector3.zero, 1, 1, 0.5f);
    }

	private void RefreshHPBar(int oldData, int newDada)
	{
		if(null == hpProgressSkin)
		{
			_hpBarSource = _hpBarSource ?? Resources.Load<GameObject>(HPBarPath);
			hpProgressSkin = GameObjectPool.Instance.Spawn(_hpBarSource).GetComponent<HPProgressBarSkin>();
			CommonUtil.ResetTransform(hpProgressSkin.transform);
			hpProgressSkin.SetFollowTarget(progressAnchor);
		}

		if(newDada <= 0)
		{
			hpProgressSkin.gameObject.SetActive(false);
		}
		else
		{
			if(!hpProgressSkin.gameObject.activeSelf)
			{
				hpProgressSkin.gameObject.SetActive(true);
			}
			
			hpProgressSkin.progressBar.value = newDada / (float) maxHP;
		}
	}
}