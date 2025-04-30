using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    //建立dictionary (鍵(collectableType)對應物品)
    private Dictionary<string, Item> nameToItemDict = new Dictionary<string, Item>();
    private void Awake()
    {
        foreach(Item item in items)
        {
            AddItem(item); //將item加入字典中
        }
    }

    private void AddItem(Item item)
    {
        //如果還沒有這個item 就加進去
        if(!nameToItemDict.ContainsKey(item.data.itemName)) 
        {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key)
    {
        if(nameToItemDict.ContainsKey(key)) //傳入一個類型，若有這個類型，回傳對應的物品
        {
            return nameToItemDict[key];
        }

        return null;//沒有找到
    }
}
