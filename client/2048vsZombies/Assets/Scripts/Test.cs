using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	void Start () {
	
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameObject source = Resources.Load<GameObject>("Bullet/TestBullet");
			GameObject clone = SleepyHippo.Util.GameObjectPool.InstanceNoClear.Spawn(source, 1);
			Bullet bullet = clone.GetComponent<Bullet>();
			bullet.Fire( Vector3.zero, 1, 10, 1, OnHitMob);
		}
	}

	void OnHitMob(Bullet bullet)
	{
		Debug.Log("Hit");
	}
}
	