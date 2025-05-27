using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    [Header("Storagebox")]
    public Inventory storagebox;
    public int storageboxSlotCount;


    private void Awake()
    {
        //單例處理
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //保留物件跨場景
        }
        else
        {
            Destroy(gameObject); //若有重複就刪掉
            return;
        }

        //註冊格數
        backpack = new Inventory(backpackSlotCount);
        toolbar = new Inventory(toolbarSlotCount);
        storagebox = new Inventory(storageboxSlotCount);

        //註冊進inventoryByName的字典中
        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("Toolbar", toolbar);
        inventoryByName.Add("Storagebox", storagebox);
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
        if (inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }

    //如果要增加儲存容器的話
    public void CreateInventory(string name, int slotCount)
    {
        if (!inventoryByName.ContainsKey(name))
        {
            inventoryByName[name] = new Inventory(slotCount);
        }
    }
}
