using UnityEngine;

[CreateAssetMenu(fileName ="合成表",menuName ="leo/合成表")]
public class MergeTable : ScriptableObject
{
    [Header("所有合成資料")]
    public MergeData[] allMergeData;
}

/// <summary>
/// 合成資料：儲存每一筆合成資料
/// </summary>>
public class MergeData 
{
    /// <summary>
    /// 素材 1 ~ 4 所需配置的物件
    /// </summary>
    [Header("素材 1 ~ 4 所需配置的物件")]
    public GameObject[] goMergo = new GameObject[4];
    [Header("合成後的物件")]
    public GameObject goMergeResult;
}
