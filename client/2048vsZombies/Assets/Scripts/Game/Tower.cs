// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//

using UnityEngine;
using DG.Tweening;

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

	public MeshRenderer renderer;
	public Transform firePoint;

	private static GameObject sourceBullet;

    public void Shoot()
    {
		sourceBullet = sourceBullet ?? Resources.Load<GameObject>("Bullet/TestBullet");
		GameObject clone = SleepyHippo.Util.GameObjectPool.InstanceNoClear.Spawn(sourceBullet, 1);
		Bullet bullet = clone.GetComponent<Bullet>();
		bullet.Fire(firePoint.position, buff, 10, 10);
    }

    public void Upgrade()
    {
        gameObject.transform.DOPunchScale(Vector3.one, 0.2f).SetDelay(0.2f);
        power *= 2;
        //change material
		renderer.material = Resources.Load<Material>("Materials/" + power);
    }
}