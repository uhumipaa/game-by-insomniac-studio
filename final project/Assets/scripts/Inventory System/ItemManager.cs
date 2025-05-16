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
    [Header("物品資料")]
    public List<ItemData> itemList;

    [Header("通用 Collectable Prefab")]
    public Collectable collectablePrefab;

    //建立dictionary
    private Dictionary<ItemType, ItemData> dict = new();

    private void Awake()
    {
        /*//註冊collectable prefab
        foreach(var data in itemList)
        {
             // 確保 prefab 上的 item 屬性不為 null
            if (entry.prefab.item == null)
            {
                entry.prefab.item = entry.prefab.GetComponent<Item>();
            }

            if (entry.prefab.item == null)
            {
                Debug.LogError($"❌ {entry.prefab.name} 上找不到 Item 組件，無法註冊 {entry.type}");
                continue;
            }
            AddItem(entry);
        }

        foreach(var data in itemList)
        {
            if (!dict.ContainsKey(data.type))
            {
                dict.Add(data.type, data.prefab.item.data);
                Debug.Log($"✅ 註冊 ItemData：{data.prefab.item.data.itemName}");
            }
        }*/

        // 註冊 ItemData
        foreach (var data in itemList)
        {
            if (!dict.ContainsKey(data.type))
            {
                dict.Add(data.type, data);
                Debug.Log($"✅ 註冊 ItemData：{data.itemName}");
            }
        }
    }

    /*private void AddItem(ItemEntry entry)
    {
        //如果還沒有這個item 就加進去
        if(!dict.ContainsKey(entry.type)) 
        {
            dict.Add(entry.type, entry.prefab.item.data);
            Debug.Log("註冊 item type：" + entry.type);
        }
    }*/

    public ItemData GetItemData(ItemType type)
    {
        if (dict.ContainsKey(type))
        {
            return dict[type];
        }
        Debug.LogWarning($"❌ 找不到 ItemData：{type}");
        return null;
    }

    /*public Collectable GetCollectablePrefab(ItemType type)
    {
        if(dict.ContainsKey(type)) //傳入一個類型，若有這個類型，回傳對應的物品
        {
            return dict[type];
        }

        //Debug.LogWarning($"❌ GetCollectablePrefab 找不到對應的 ItemType：{type}");
        return null;//沒有找到
        
    }*/

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
}