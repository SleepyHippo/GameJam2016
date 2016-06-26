using UnityEngine;
using System.Collections;

public class AutoRecycle : MonoBehaviour {


	public float delay;

	void OnEnable()
	{
		StartCoroutine(WaitRecycle(delay));
	}

	IEnumerator WaitRecycle(float delay)
	{
		yield return new WaitForSeconds(delay);
		SleepyHippo.Util.GameObjectPool.Instance.Recycle(this.gameObject);
	}
}
