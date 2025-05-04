using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    //建立dictionary (鍵(collectableType)對應物品)
    private Dictionary<ItemType, Item> typeDict = new Dictionary<ItemType, Item>();
    private void Awake()
    {
        foreach(Item item in items)
        {
            Debug.Log($"註冊 type：{item.data.type}, 來自 prefab：{item.name}");
            AddItem(item); //將item加入字典中
        }
    }

    private void AddItem(Item item)
    {
        //如果還沒有這個item 就加進去
        if(!typeDict.ContainsKey(item.data.type)) 
        {
            typeDict.Add(item.data.type, item);
            Debug.Log("註冊 item type：" + item.data.type);
        }
    }

    public Item GetItemByType(ItemType type)
    {
        if(typeDict.ContainsKey(type)) //傳入一個類型，若有這個類型，回傳對應的物品
        {
            return typeDict[type];
        }
        return null;//沒有找到
        
    }
}