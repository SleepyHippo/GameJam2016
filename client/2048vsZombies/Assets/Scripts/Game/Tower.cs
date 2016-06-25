// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//

using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Tower : DynamicItem
{
    public enum Buff
    {
        Ice = 1,
        Through = 2,
        Explode = 4
    }

    public const int ICE_TIME = 3;
    public const int THROUGH_TIME = 2;
    public const int EXPLODE_TIME = 2;

    private int nowIceLeft;
    private int nowThroughLeft;
    private int nowExplodeLeft;

    private int _power;
    public int power
    {
        get { return _power; }
        set
        {
            if(_power != value)
            {
                _power = value;
                meshRenderer.material = Resources.Load<Material>("Materials/" + _power);
            }
        }
    }
    public int buff;

	public MeshRenderer meshRenderer;
	public Transform firePoint;

	private static GameObject sourceBullet;

    void Awake()
    {
        Messenger.AddListener(MessageConst.SKILL_ICE, OnSkillIce);
        Messenger.AddListener(MessageConst.SKILL_THROUGH, OnSkillThrough);
        Messenger.AddListener(MessageConst.SKILL_EXPLODE, OnSkillExplode);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(MessageConst.SKILL_ICE, OnSkillIce);
        Messenger.RemoveListener(MessageConst.SKILL_THROUGH, OnSkillThrough);
        Messenger.RemoveListener(MessageConst.SKILL_EXPLODE, OnSkillExplode);
    }

    public override void OnTick()
    {
        if(--nowIceLeft <= 0)
        {
            if((buff & (int)Buff.Ice) > 0)
            {
                buff -= (int)Buff.Ice;
            }
        }
        if(--nowThroughLeft <= 0)
        {
            if((buff & (int)Buff.Through) > 0)
            {
                buff -= (int)Buff.Through;
            }
        }
        if(--nowExplodeLeft <= 0)
        {
            if((buff & (int)Buff.Explode) > 0)
            {
                buff -= (int)Buff.Explode;
            }
        }
    }

	[ContextMenu("Shoot")]
    public void Shoot()
    {
		StartCoroutine(WaitAndShoot(0.22f));
    }

    public void Upgrade()
    {
        gameObject.transform.DOPunchScale(Vector3.one, 0.2f).SetDelay(0.2f);
        power *= 2;
    }

	private IEnumerator WaitAndShoot(float delay)
	{
		yield return new WaitForSeconds(delay);
		sourceBullet = sourceBullet ?? Resources.Load<GameObject>("Bullet/Bullet");
		GameObject clone = SleepyHippo.Util.GameObjectPool.Instance.Spawn(sourceBullet, 1);
		Bullet bullet = clone.GetComponent<Bullet>();
		bullet.Fire(firePoint.position, buff, power, 10);
	}

    void OnSkillIce()
    {
        buff = buff | (int)Buff.Ice;
        nowIceLeft = ICE_TIME;
    }

    void OnSkillThrough()
    {
        buff = buff | (int)Buff.Through;
        nowThroughLeft = THROUGH_TIME;
    }

    void OnSkillExplode()
    {
        buff = buff | (int)Buff.Explode;
        nowExplodeLeft = EXPLODE_TIME;
    }
}