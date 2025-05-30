using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour,ISaveData
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

    ItemManager itemManager;

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

        itemManager = FindAnyObjectByType<ItemManager>().GetComponent<ItemManager>();
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

    private List<SaveItem> ConvertItemsToSaveItems(Inventory inventory)
    {
        var result = new List<SaveItem>();
        foreach (var slot in inventory.slots)
        {
            if (!slot.IsEmpty)
            {
                result.Add(new SaveItem(slot.itemData.itemName, slot.count));
            }
        }
        return result;
    }
    private void LoadSaveItem(Inventory inventory, List<SaveItem> items,int type)
    {
        inventory.slots.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            var saved = items[i];
            int quantity = items[i].quantity;
            ItemData data = itemManager.GetItemDataByName(saved.Name);
            if (data != null)
            {
                var slot = new Inventory.Slot();
                switch (type)
                {
                    case 0:
                        Add("Backpack", data, quantity);
                        break;
                    case 1:
                        Add("Toolbar", data, quantity);
                        break;
                    case 2:
                        Add("Storagebox", data, quantity);
                        break;
                }
                
            }
            else
            {
                Debug.LogWarning($"❌ 無法找到 itemID：{saved.Name}");
            }
        }
    }

    public void SaveData(ref SaveData saveData)
    {
        
        saveData.backpackItems = ConvertItemsToSaveItems(backpack);
        saveData.toolbarItems = ConvertItemsToSaveItems(toolbar);
        saveData.equippedItems = ConvertItemsToSaveItems(storagebox); 
    }

    public void LoadData(SaveData saveData)
    {
        LoadSaveItem(backpack, saveData.backpackItems,0);
        LoadSaveItem(toolbar, saveData.toolbarItems,1);
        LoadSaveItem(storagebox, saveData.equippedItems,2);
    }

}
