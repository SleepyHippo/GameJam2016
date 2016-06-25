// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//
using System;
using UnityEngine;

public class Land : MonoBehaviour
{
    public bool isGood = true;
    public GameObject goodLand;
    public GameObject badLand;

    void Awake()
    {
        SetType(true);
    }

    public void SetType(bool good)
    {
        isGood = good;
        goodLand.SetActive(good);
        badLand.SetActive(!good);
    }
}