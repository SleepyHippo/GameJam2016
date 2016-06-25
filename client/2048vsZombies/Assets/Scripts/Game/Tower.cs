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

    public int power;//2048的数字
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
        //change material
		meshRenderer.material = Resources.Load<Material>("Materials/" + power);
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