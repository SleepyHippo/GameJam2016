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
    public override void MoveLeft(int distance)
    {
        gameObject.transform.DOMoveX(gameObject.transform.position.x - distance, 0.2f);
    }
    public override void MoveRight(int distance)
    {
        gameObject.transform.DOMoveX(gameObject.transform.position.x + distance, 0.2f);
    }
    public override void MoveUp(int distance)
    {
        gameObject.transform.DOMoveY(gameObject.transform.position.y + distance, 0.2f);
    }
    public override void MoveDown(int distance)
    {
        gameObject.transform.DOMoveY(gameObject.transform.position.y - distance, 0.2f);
    }
    public override void OnTick()
    {
    }
    #endregion
}