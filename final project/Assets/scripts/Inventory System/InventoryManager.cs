using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    private void Awake()
    {
        backpack = new Inventory(backpackSlotCount);
        toolbar = new Inventory(toolbarSlotCount);

        //把backpack跟toolbar註冊進inventoryByName的字典中
        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("Toolbar", toolbar);
    }

    public void Add(string inventoryName, ItemData data, int amount = 1)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(data, amount);
            Debug.Log($"{inventoryName} 新增 {amount}個{data.itemName}");
        }    
    }
    
    //查找功能
    public Inventory GetInventoryByName(string inventoryName)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }
}
