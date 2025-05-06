using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ItemEntry
{
    public ItemType type;
    public Collectable prefab;
}
public class ItemManager : MonoBehaviour
{
    public List<ItemEntry> itemList;

    //建立dictionary (鍵(collectableType)對應物品)
    private Dictionary<ItemType, Collectable> dict = new();
    private void Awake()
    {
        foreach(var entry in itemList)
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

            Debug.Log($"✅ 註冊 type：{entry.type}, prefab：{entry.prefab.name}");
            AddItem(entry);
        }
    }

    private void AddItem(ItemEntry entry)
    {
        //如果還沒有這個item 就加進去
        if(!dict.ContainsKey(entry.type)) 
        {
            dict.Add(entry.type, entry.prefab);
            Debug.Log("註冊 item type：" + entry.type);
        }
    }

    public Collectable GetCollectablePrefab(ItemType type)
    {
        if(dict.ContainsKey(type)) //傳入一個類型，若有這個類型，回傳對應的物品
        {
            return dict[type];
        }
        return null;//沒有找到
        
    }
}