using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具欄以及裝備道具欄的每一格項目
/// </summary>

public class InventoryItem : MonoBehaviour
{
    [HideInInspector]       // 將 public 公開的欄位隱藏
    /// <summary>
    ///  是否有道具
    /// </summary>
    public bool hasProp;
    /// <summary>
    /// 道具顯示
    /// </summary>
    public Image imgProp;
    /// <summary>
    /// 道具數量
    /// </summary>
    public Text textProp;

    // Awake 隱藏物件上不會執行   
    private void Awake()
    {
        imgProp = transform.Find("道具顯示").GetComponent<Image>();
        textProp = transform.Find("道具數量").GetComponent<Text>();
    }

}
