﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour 
{
	//TODO 改成bullet mob
	public delegate void OnHitMob(Bullet bullet);

	public float speed
	{
		get;
		private set;
	}
	public int damage
	{
		get;
		private set;
	}

	public int buffer
	{
		get;
		private set;
	}


	private const string MobTag = "Mob";
	private bool _hasHit;
	private bool _isInit;
	private OnHitMob hitCallback;
	private GameObject effect;

	private static Dictionary<Tower.Buff, GameObject> effectMap = new Dictionary<Tower.Buff, GameObject>();

	private Vector3 _directionSpeed;
	Vector3 directionSpeed
	{
		get
		{
			if(null == _directionSpeed)
			{
				_directionSpeed = new Vector3(0, 0, speed);
			}
			if(_directionSpeed.z != speed)
			{
				_directionSpeed.x = 0;
				_directionSpeed.y = 0;
				_directionSpeed.z = speed;
			}

			return _directionSpeed;
		}
	}

	public void Fire(Vector3 startPos, int buffer, int damage, float speed = 10.0f, OnHitMob callback = null)
	{
		if(!_isInit)
		{
			this.gameObject.transform.position = startPos;
			this.gameObject.transform.forward = Vector3.forward;
			hitCallback = callback;
			this.buffer = buffer;
			this.damage = damage;
			this.speed = speed;
			AttachBuff(buffer);

			_isInit = true;
			_hasHit = false;
		}
	}

	void Update()
	{
		if(_isInit && !_hasHit)
		{
			transform.position += directionSpeed * Time.deltaTime;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!_hasHit && other.gameObject.CompareTag(MobTag))
		{
			_hasHit = true;
//			PlayEffect();
			if(null != hitCallback)
			{
				hitCallback(this);
			}
			Recycle();
		}
	}

//	private void PlayEffect()
//	{
//		switch(buffer)
//		{
//			case Tower.Buff.Ice:
//				break;
//			case Tower.Buff.Through:
//				break;
//			case Tower.Buff.Explode:
//				break;
//		}
//	}

	private void AttachBuff(int buffer)
	{

			GameObject source;
		List<int> buffList = CommonUtil.GetBuffList(buffer);
		List<int>.Enumerator iter = buffList.GetEnumerator();

		while(iter.MoveNext())
		{
			Tower.Buff buff = (Tower.Buff) iter.Current;
			if(!effectMap.TryGetValue(buff, out source))
			{
				source = Resources.Load<GameObject>("Effect/" + buff.ToString());
				if(null != source)
				{
					effectMap.Add(buff, source);
				}
			}

			if(null != source)
			{
				effect = SleepyHippo.Util.GameObjectPool.Instance.Spawn(source, 1);
				effect.transform.parent = this.transform;
				CommonUtil.ResetTransform(effect.transform);
			}
			source = null;
		}
	}

	private void Recycle()
	{
		_isInit = false;
		_hasHit = false;

		if(null != effect)
		{
			SleepyHippo.Util.GameObjectPool.InstanceNoClear.Recycle(this.effect);
		}

		transform.position = Vector3.zero;
		SleepyHippo.Util.GameObjectPool.InstanceNoClear.Recycle(this.gameObject);
	}
}
