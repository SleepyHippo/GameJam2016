using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	void Start () {
	
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			Shoot(0);
		}
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Shoot(1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Shoot(2);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			Shoot(3);
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			Shoot(4);
		}
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			Shoot(5);
		}
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			Shoot(6);
		}
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			Shoot(7);
		}
	}

	void Shoot(int buff)
	{
		GameObject source = Resources.Load<GameObject>("Bullet/Bullet");
		GameObject clone = SleepyHippo.Util.GameObjectPool.Instance.Spawn(source, 1);
		Bullet bullet = clone.GetComponent<Bullet>();
		bullet.Fire(new Vector3(0, 0.3f, 0), buff, 10, 1, OnHitMob);
	}

	void OnHitMob(Bullet bullet)
	{
		Debug.Log("Hit");
	}
}
	