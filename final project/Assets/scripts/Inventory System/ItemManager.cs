using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;

    //建立dictionary (鍵(collectableType)對應物品)
    private Dictionary<CollectableType, Collectable> collectableItemDict = new Dictionary<CollectableType, Collectable>();
    private void Awake()
    {
        foreach(Collectable item in collectableItems)
        {
            AddItem(item); //將item加入字典中
        }
    }

    private void AddItem(Collectable item)
    {
        //如果還沒有這個item 就加進去
        if(!collectableItemDict.ContainsKey(item.type)) 
        {
            collectableItemDict.Add(item.type, item);
        }
    }

    public Collectable GetItemByType(CollectableType type)
    {
        if(collectableItemDict.ContainsKey(type)) //傳入一個類型，若有這個類型，回傳對應的物品
        {
            return collectableItemDict[type];
        }

        return null;//沒有找到
    }
}
