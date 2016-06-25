using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    public Transform[] cameraPositions;

	void Start () {
        Camera camera = UIManager.instance.mainCamera;
        CommonUtil.SetTransform(camera.transform, cameraPositions[0]);
        Messenger.AddListener(MessageConst.MOVIE_START, PlayCameraAnimation);
	}
	
    void PlayCameraAnimation()
    {
        Camera camera = UIManager.instance.mainCamera;
        CommonUtil.SetTransform(camera.transform, cameraPositions[0]);
        float phase1Time = 2f;
        float phase2Time = 1f;
        float phase3Time = 3f;
        camera.transform.DOMove(cameraPositions[1].position, phase1Time).SetEase(Ease.OutQuad);
        camera.transform.DORotate(cameraPositions[1].eulerAngles, phase1Time).SetEase(Ease.Linear);
        camera.transform.DOMove(cameraPositions[2].position, phase2Time).SetDelay(phase1Time).SetEase(Ease.Linear);
        camera.transform.DORotate(cameraPositions[2].eulerAngles, phase2Time).SetDelay(phase1Time).SetEase(Ease.Linear);
        camera.transform.DOMove(cameraPositions[3].position, phase3Time).SetDelay(phase1Time+phase2Time).SetEase(Ease.Linear);
        camera.transform.DORotate(cameraPositions[3].eulerAngles, phase3Time).SetDelay(phase1Time+phase2Time).SetEase(Ease.Linear).OnComplete(OnAnimationComplete);
    }

    void OnAnimationComplete()
    {
        Messenger.Broadcast(MessageConst.GAME_START);
    }
}
