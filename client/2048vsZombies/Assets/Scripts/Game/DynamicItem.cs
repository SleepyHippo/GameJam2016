// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//
using UnityEngine;
using DG.Tweening;

public class DynamicItem : Item
{
    #region implemented abstract members of Item
    public override void MoveLeft(int distance, bool destroy = false)
    {
        PlayMoveAnimation();
        x -= distance;
        Tweener tweener = gameObject.transform.DOMoveX(gameObject.transform.position.x - distance, 0.2f);
        if(destroy)
            tweener.OnComplete(KillSelf);
    }
    public override void MoveRight(int distance, bool destroy = false)
    {
        PlayMoveAnimation();
        x += distance;
        Tweener tweener = gameObject.transform.DOMoveX(gameObject.transform.position.x + distance, 0.2f);
        if(destroy)
            tweener.OnComplete(KillSelf);
    }
    public override void MoveUp(int distance, bool destroy = false)
    {
        PlayMoveAnimation();
        y += distance;
        Tweener tweener = gameObject.transform.DOMoveZ(gameObject.transform.position.z + distance, 0.2f);
        if(destroy)
            tweener.OnComplete(KillSelf);
    }
    public override void MoveDown(int distance, bool destroy = false)
    {
        PlayMoveAnimation();
        y -= distance;
        Tweener tweener = gameObject.transform.DOMoveZ(gameObject.transform.position.z - distance, 0.2f);
        if(destroy)
            tweener.OnComplete(KillSelf);
    }
    public override void OnTick()
    {
    }
    #endregion

    protected virtual void PlayMoveAnimation()
    {
    }

    protected void KillSelf()
    {
        SleepyHippo.Util.GameObjectPool.Instance.Recycle(gameObject);
    }
}