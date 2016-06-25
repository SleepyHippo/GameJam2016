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

public class BoardZombie : MonoBehaviour
{
    public const int WIDTH = 4;
    public const int HEIGHT = 10;

    public GameObject zombieTemplate;

    /// <summary>
    /// -1：污染的土地
    /// 0：正常土地
    /// 1: 僵尸
    /// 原点在左下角：
    /// …………………………
    /// 12 13 14 15
    ///  8  9 10 11
    ///  4  5  6  7
    ///  0  1  2  3
    /// </summary>
    public int[] typeMap = new int[WIDTH * HEIGHT];

    /// <summary>
    /// 显示组件
    /// </summary>
    public Dictionary<int, Item> itemMap = new Dictionary<int, Item>();

    void Awake()
    {
        
    }
}

