using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static Dictionary<ItemType, ItemData> itemDict;

    static ItemDatabase()
    {
        LoadItemData();
    }

    private static void LoadItemData()
    {
        itemDict = new Dictionary<ItemType, ItemData>();
        var items = Resources.LoadAll<ItemData>("Items");
        foreach (var item in items)
        {
            if (!itemDict.ContainsKey(item.type))
                itemDict[item.type] = item;
            else
                Debug.LogWarning($"Duplicate ItemType: {item.type}");
        }
    }

    public static ItemData GetItemData(ItemType type)
    {
        if (itemDict == null) LoadItemData();

        if (itemDict.TryGetValue(type, out var itemData))
            return itemData;

        Debug.LogWarning($"ItemType not found: {type}");
        return null;
    }
}
