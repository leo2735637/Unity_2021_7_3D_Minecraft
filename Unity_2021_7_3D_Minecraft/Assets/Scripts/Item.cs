﻿using UnityEngine;

/// <summary>
/// 已經取地的道具資訊
/// </summary>
public class Item : MonoBehaviour
{
    [Header("道具物件")]
    public GameObject goItem;
    [Header("當前數量")]
    public int count;
    [Header("道具類型")]
    public PropType propType;
}
