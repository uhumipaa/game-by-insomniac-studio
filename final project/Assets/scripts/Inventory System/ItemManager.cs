using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
/*public class ItemEntry
{
    public ItemType type;
    public Collectable prefab;
}*/
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    [Header("物品資料")]
    public List<ItemData> itemList;

    [Header("通用 Collectable Prefab")]
    public Collectable collectablePrefab;

    //建立dictionary
    private Dictionary<ItemType, ItemData> dict = new();

    private void Awake()
    {
       
        // 註冊 ItemData
        foreach (var data in itemList)
        {
            if (!dict.ContainsKey(data.type))
            {
                dict.Add(data.type, data);
            }
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //保留物件跨場景
        }
        else
        {
            Destroy(gameObject); //若有重複就刪掉
            return;
        }
    }
   
   //取得ITEM資料
    public ItemData GetItemData(ItemType type)
    {
        if (dict.ContainsKey(type))
        {
            return dict[type];
        }
        Debug.LogWarning($"❌ 找不到 ItemData：{type}");
        return null;
    }

    //生成掉落物prefab
    public Collectable SpawnCollectable(ItemType type, Vector3 position)
    {
        ItemData data = GetItemData(type);

        if (collectablePrefab != null && data != null)
        {
            Collectable instance = Instantiate(collectablePrefab, position, Quaternion.identity);
            instance.SetItemData(data);
            return instance;
        }

        Debug.LogWarning($"❌ 無法生成 Collectable：{type}");
        return null;
    }

    //利用物品名字查找物品
    public ItemData GetItemDataByName(string name)
    {
        return itemList.Find(item => item.itemName == name);
    }
}