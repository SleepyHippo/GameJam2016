// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//
using System;
using UnityEngine;
using System.Collections.Generic;

public class CommonUtil
{
    public static void ResetTransform(Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static int GetX(int index, int width)
    {
        return index % width;
    }

    public static int GetY(int index, int width)
    {
        return index / width;
    }

    public static int GetIndex(int x, int y, int width)
    {
        return y * width + x;
    }

    public static int GetIndex(Item item, int width)
    {
        return item.y * width + item.x;
    }


	public static List<Tower.Buff> GetBuffList(int buff)
	{
		List<Tower.Buff> buffList = new List<Tower.Buff>(4);

		if((buff & (int) Tower.Buff.Ice) > 0)
		{
			buffList.Add(Tower.Buff.Ice);
		}

		if((buff & (int) Tower.Buff.Through) > 0)
		{
			buffList.Add(Tower.Buff.Through);
		}

		if((buff & (int) Tower.Buff.Explode) > 0)
		{
			buffList.Add(Tower.Buff.Explode);
		}

		return buffList;
	}

	public static bool HasBuff(int buff, Tower.Buff buffId)
	{
		return (buff & (int) buffId) > 0;
	}
	
	public static void SetTransform(Transform tsf, Transform fromTsf)
    {
        tsf.position = fromTsf.position;
        tsf.rotation = fromTsf.rotation;
        tsf.localScale = fromTsf.localScale;
    }
}

