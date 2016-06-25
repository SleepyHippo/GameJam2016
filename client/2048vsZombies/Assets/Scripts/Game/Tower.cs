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

    public void Shoot()
    {
        //use Pool to spawn bullet

        //set bullet damage, fire!
    }

    public void Upgrade()
    {
        gameObject.transform.DOPunchScale(Vector3.one, 0.2f).SetDelay(0.2f);
        power *= 2;
        //change material
    }


}