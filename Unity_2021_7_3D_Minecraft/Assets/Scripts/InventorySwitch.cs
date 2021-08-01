using UnityEngine;

/// <summary>
/// 切換道具管理器
/// 切換道具：裝備與道具欄
/// 點擊後將道具資訊存放到【選取中的道具】
/// 並判定與另一個欄位切換
/// </summary>

public class InventorySwitch : MonoBehaviour
{    
    #region 欄位
    [Header("選取中的道具")]
    public Transform traChooseProp;
    #endregion

    #region 事件
    private void Start()
    {
        
    }
    private void Update()
    {
        CheckMousePosionItem();
    }

    #endregion

    #region 方法
    /// <summary>
    /// 檢查滑鼠座標上的道具
    /// </summary>
    private void CheckMousePosionItem()
    {
        Vector3 posMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        print("滑鼠的座標：" + posMouse);
    }

    #endregion
}

