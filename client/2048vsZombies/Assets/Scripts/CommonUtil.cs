// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//
using System;
using UnityEngine;

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
}

