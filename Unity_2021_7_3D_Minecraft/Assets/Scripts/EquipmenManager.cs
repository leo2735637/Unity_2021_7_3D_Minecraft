﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;   

/// <summary>
/// 裝備管理器：可以使用滾輪或按鍵切換裝備
/// 以及使用裝備系統
/// </summary>
public class EquipmenManager : MonoBehaviour
{
    #region 欄位
    [Header("裝備道具 1 - 5")]
    public Transform[] traEquipmentItem;      

    /// <summary>
    /// 當前裝備的編號：0 - 4
    /// </summary>
    private int indexEquipment;
    /// <summary>
    /// 選取邊框-顯示選取裝備道具用
    /// </summary>
    private Transform traSelectionOutline;
    /// <summary>
    /// 選取邊框-選取邊框的UI變形元件
    /// </summary>
    private RectTransform rectSelectionOutline;
    /// <summary>
    /// 道具管理器：需要取得裡面的陣列資料 Item 陣列 
    /// </summary>
    private Inventory inventory;
    /// <summary>
    /// 顯示道具位置：手部骨架內的空物件
    /// </summary>
    private Transform traPropPosition;
    #endregion

    [Header("蓋地形的範圍"), Range(0, 30)]
    public float rangeBuildTerrain = 3;
    [Header("地形物件高度")]
    public float heightTerrainObject = 0.8f;

    /// <summary>
    /// 是否使用地形物件
    /// </summary>
    private bool usingTerrainObject;
    /// <summary>
    /// 玩家攝影機物件
    /// </summary>
    private Transform playerCamera;

    #region 事件    
    private void Start()
    {
        traSelectionOutline = GameObject.Find("選取邊框").transform;
        rectSelectionOutline = traSelectionOutline.GetComponent<RectTransform>();
        inventory = GameObject.Find("道具管理器").GetComponent<Inventory>();
        traPropPosition = GameObject.Find("顯示道具位置").transform;
        playerCamera = GameObject.Find("攝影機").transform;
    }

    private void Update()
    {
        SwitchEquipment();

        ClickAndUseEquipment();
    }

    private void OnDrawGizmos()
    {
        if (playerCamera && usingTerrainObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(playerCamera.position, playerCamera.forward * rangeBuildTerrain);

            Gizmos.color = new Color(1, 0.3f, 0);

            Vector3 posOrignal = playerCamera.position + playerCamera.forward * rangeBuildTerrain;

           // print("原始座標：" + posOrignal);

            // 計算座標、X 與 Z 四捨五入、Y 以高度為倍數(預設為 0.8)   
            Vector3 posCalculate = Vector3.zero;
            posCalculate.x = Mathf.Round(posOrignal.x);
            posCalculate.z = Mathf.Round(posOrignal.z);
            posCalculate.y = (int)(posOrignal.y / heightTerrainObject) * heightTerrainObject + heightTerrainObject / 2;

            //print("計算後的座標：" + posCalculate);

            Gizmos.DrawWireCube(posOrignal, new Vector3(1, heightTerrainObject, 1));

            Gizmos.color = new Color(0.5f, 0.1f, 0.8f, 0.1f);
            Gizmos.DrawWireCube(posCalculate, new Vector3(1.5f, heightTerrainObject - 0.5f, 0.5f));
            Gizmos.DrawWireCube(posCalculate, new Vector3(0.5f, heightTerrainObject + 0.5f, 0.5f));
            Gizmos.DrawWireCube(posCalculate, new Vector3(0.5f, heightTerrainObject - 0.5f, 1.5f));
            #endregion
        }
    }


    /// <summary>
    /// 點擊並使用裝備
    /// </summary>
    private void ClickAndUseEquipment()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           if(usingTerrainObject) CheckTerrainObject();
        }
    }


    /// <summary>
    /// 檢查是否能蓋地形物件並建立地形物件
    /// </summary>
    private void CheckTerrainObject()
    {
        #region 計算角色面前可以蓋地形物件的座標
        Vector3 posOrignal = playerCamera.position + playerCamera.forward * rangeBuildTerrain;
        Vector3 posCalculate = Vector3.zero;
        posCalculate.x = Mathf.Round(posOrignal.x);
        posCalculate.z = Mathf.Round(posOrignal.z);
        posCalculate.y = (int)(posOrignal.y / heightTerrainObject) * heightTerrainObject + heightTerrainObject / 2;
        #endregion

        // 碰撞檢查：左右、上下、前後 、OverlapBox(中心點、尺寸的一半 - 半徑)
        Collider[] hitRL = Physics.OverlapBox(posCalculate, new Vector3(1.5f, heightTerrainObject - 0.5f, 0.5f) / 2);
        Collider[] hitUD = Physics.OverlapBox(posCalculate, new Vector3(0.5f, heightTerrainObject + 0.5f, 0.5f) / 2);
        Collider[] hitFB = Physics.OverlapBox(posCalculate, new Vector3(0.5f, heightTerrainObject - 0.5f, 1.5f) / 2);

        if (hitRL.Length > 0 || hitUD.Length > 0 || hitFB.Length > 0)
        {
            // 檢查要蓋地形的座標有沒有地形存在、沒有才能蓋地形 
            Vector3 posBuild = posCalculate + Vector3.up * heightTerrainObject / 2;
            int countSameRL = hitRL.Where(X => X.transform.position == posBuild).ToList().Count;
            int countSameUD = hitUD.Where(X => X.transform.position == posBuild).ToList().Count;
            int countSameFB = hitFB.Where(X => X.transform.position == posBuild).ToList().Count;

            if (countSameRL == 0 && countSameUD == 0 && countSameFB == 0)
            {
                Instantiate(traEquipmentItem[indexEquipment].GetComponent<Item>().goItem, posBuild, Quaternion.identity);
            }
            else
            {
                print("要蓋地形的座標上已經有其他地形存在，不能蓋!!!");
            }
        }
    }

    /// <summary>
    /// 切換裝備
    /// 透過滾輪切換
    /// </summary>
    private void SwitchEquipment()
    {
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");        // 滾輪的職 

        if (wheel < 0)                                              // 如果 往後捲   
        {
            indexEquipment++;                                       // 編號遞增 
            if (indexEquipment == 5) indexEquipment = 0;            // 如果編號超出範圍就回到零
            SetSelectiomEquipment();
        }
        else if (wheel < 0)                                         // 如果 往後捲   
        {
            indexEquipment--;                                       // 編號遞增 
            if (indexEquipment == -1) indexEquipment = 4;           // 如果編號超出範圍就回到零 
            SetSelectiomEquipment();
        }
    }

    /// <summary>
    /// 設定目前選擇的裝備：將選取邊框設定到目前的裝備
    /// </summary>
    private void SetSelectiomEquipment()
    {
        traSelectionOutline.SetParent(traEquipmentItem[indexEquipment]);
        rectSelectionOutline.anchoredPosition = Vector2.zero;

        ShowEquipment();
    }

    /// <summary>
    /// 使用過的道具物件資訊
    /// </summary>
    public List<GameObject> listUsingItem = new List<GameObject>();

    /// <summary>
    /// 顯示裝備：
    /// 還沒用過的裝備生成出來放進資料庫內、已經用過的從資料庫內拿
    /// 並且顯示在手上調整：座標、角度與尺寸
    /// </summary>
    public void ShowEquipment()
    {
        // 隱藏所有使用過的道具
         for (int i = 0; i < listUsingItem.Count; i++) listUsingItem[i].SetActive(false);

        Item itemData = inventory.itemDataEquipment[indexEquipment];        // 目前的道具資料 

            usingTerrainObject = false;         // 不是使用地形物件

        if (itemData.goItem)
        {
            usingTerrainObject = true;          // 是使用地形物件

            // 判斷 清單內的道具 是否包含 當前選取的道具，列：草地(clone) 包含 草地 就表示用過
            int count = listUsingItem.Where(x => x.name.Contains(itemData.goItem.name)).ToList().Count;
            
            // 如果 清單內 沒有使用過此道具 就處理生成
            if (count == 0)
            {
                 // 還沒使用過先生成 
                 GameObject equip = Instantiate(itemData.goItem, traPropPosition);
                 equip.transform.localScale = Vector3.one * 0.3f;
                 equip.transform.localPosition = Vector3.zero;

                 listUsingItem.Add(equip);                                       // 將裝備添加到清單資料內 
            }
            else
            {
                // 否則 就處理 顯示
                // 取得清單內的裝備     
                GameObject goEquip = listUsingItem.Where(x => x.name.Contains(itemData.goItem.name)).First();
                goEquip.SetActive(true);
            }
        }  
    }
}
