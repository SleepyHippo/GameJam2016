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
}