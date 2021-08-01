﻿using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("採集範圍"), Range(0, 30)]
    public float rangeCollection = 2.5f;    

    private Rigidbody rig;
    private Animator ani;
    private Transform traCamera;

    #endregion

    #region 事件
    /// <summary>
    /// 道具管理器
    /// </summary>
    private Inventory inventory;
    /// <summary>
    /// 裝備管理器
    /// </summary>
    private EquipmenManager equipmentManage;
            
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        //transform.Find("子物件名稱") 透過名稱搜尋子物件
        ani = transform.Find("男生").GetComponent<Animator>();
        traCamera = GameObject.Find("攝影機").transform;

        inventory = GameObject.Find("道具管理器").GetComponent<Inventory>();
        equipmentManage = GameObject.Find("裝備管理器").GetComponent<EquipmenManager>();
    }

    private void OnDrawGizmos()
    {
        // 開發裝備系統時先隱藏 
        Gizmos.color = Color.clear;
        // 圖示.繪製射線(中心點、射線方向)
        Gizmos.DrawRay(traCamera.position, traCamera.forward * rangeCollection);
    }

    private void Update()
    {
        Collection();
    }

    private void OnCollisionEnter(Collision collision)
    {
        EatProp(collision.gameObject);
    }

    #endregion

    #region 方法

    /// <summary>
    /// 採集：繪製射線並且判定是否攻擊以及採集物件
    /// 條件：判定裝備管理器、當前的裝備類型、不是地形物件才能採集、包含：無、武器
    /// </summary>
    private void Collection()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && !EquipmenManager.instance.usingTerrainObject)
        {
            //射線碰撞資訊
            RaycastHit hit;
            //物理.射線碰撞(中心點、射線方向、射線碰撞資訊、射線長度) - out 將資訊儲存在輸入的欄位內 
            if(Physics.Raycast(traCamera.position, traCamera.forward, out hit, rangeCollection, 1 << 8))
            {
                //對採集物造成傷害
                hit.collider.GetComponent<ObjectCollection>().Hit(1);
            }
            
        }
                
    }

    /// <summary>
    /// 吃道具、判定吃到的道具並刪除道具類加道具
    /// </summary>
    /// <param name="prop">吃到的道具</param> 
    private void EatProp(GameObject prop)
    {
        if (prop.tag=="可以吃的道具")
        {
            inventory.AddProp(prop.GetComponent<Prop>());
            equipmentManage.ShowEquipment();
        }
    }   


    #endregion

}
