using UnityEngine;
using System.Collections;

public class HPProgressBarSkin : MonoBehaviour {

	public UIFollowTarget followTarget;

	public UIProgressBar progressBar;

	public void SetFollowTarget(Transform target)
	{
		followTarget.target = target;
		followTarget.gameCamera = UIManager.instance.mainCamera;
		followTarget.uiCamera = UIManager.instance.uiCamera;
	}
}
