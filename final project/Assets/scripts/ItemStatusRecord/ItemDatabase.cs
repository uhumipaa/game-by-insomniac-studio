using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static Dictionary<string, ItemData> itemDict;

    static ItemDatabase()
    {
        LoadItemData();
    }

    private static void LoadItemData()
    {
        itemDict = new Dictionary<string, ItemData>();
        var items = Resources.LoadAll<ItemData>("Items"); // 請將所有 ItemData 放進 Resources/Items 資料夾
        foreach (var item in items)
        {
            if (!itemDict.ContainsKey(item.ID))
                itemDict[item.ID] = item;
            else
                Debug.LogWarning($"Duplicate Item ID found: {item.ID}");
        }
    }

    public static ItemData GetItemData(string id)
    {
        if (itemDict == null) LoadItemData();

        if (itemDict.TryGetValue(id, out var itemData))
            return itemData;

        Debug.LogWarning($"Item ID not found: {id}");
        return null;
    }
}
