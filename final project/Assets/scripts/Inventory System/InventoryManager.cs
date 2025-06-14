using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveData
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

        regristall();
    }

    public void regristall()
    {
        //註冊格數
        backpack = new Inventory(backpackSlotCount);
        toolbar = new Inventory(toolbarSlotCount);
        storagebox = new Inventory(storageboxSlotCount);

        //註冊進inventoryByName的字典中
        inventoryByName.Clear();
        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("Toolbar", toolbar);
        inventoryByName.Add("Storagebox", storagebox);
    }

    //新增物品
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
    private void LoadSaveItem(Inventory inventory, List<SaveItem> items, int type)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var saved = items[i];
            int quantity = items[i].quantity;
            ItemData data = ItemManager.instance.GetItemDataByName(saved.Name);
            if (data != null)
            {
                Debug.Log("add");
                switch (type)
                {
                    case 0:
                        Add("Backpack", data, quantity);
                        Debug.Log("addtobackpack");
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
        Debug.Log("儲存背包資料中");
        saveData.backpackItems = ConvertItemsToSaveItems(backpack);
        saveData.toolbarItems = ConvertItemsToSaveItems(toolbar);
        saveData.storeboxItems = ConvertItemsToSaveItems(storagebox);
    }

    [System.Obsolete]
    public void LoadData(SaveData saveData)
    {
       
        regristall();
        foreach (Inventory_UI ui in Resources.FindObjectsOfTypeAll<Inventory_UI>())
        {
            Debug.Log($"reconnect:{ui.name}");
            ui.Reconnect();
        }
        LoadSaveItem(backpack, saveData.backpackItems, 0);
        LoadSaveItem(toolbar, saveData.toolbarItems, 1);
        LoadSaveItem(storagebox, saveData.storeboxItems, 2);
    }

    //清空指定Inventory
    public void Clear(string inventoryName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].ClearInventory();
            Debug.Log($"{inventoryName} 已清空");
        }
        else
        {
            Debug.LogWarning($"找不到名為 {inventoryName} 的背包，無法清空！");
        }
    }

    //尋找是否有特定物品
    public bool Contains(string inventoryName, ItemData data)
    {
        if (inventoryByName.TryGetValue(inventoryName, out Inventory inventory))
        {
            return inventory.Contains(data);
        }
        return false;
    }
    //總共有該物品幾個
    public int GetItemCount(string inventoryName, ItemData data)
    {
        if (inventoryByName.TryGetValue(inventoryName, out Inventory inventory))
        {
            return inventory.GetItemCount(data);
        }
        return 0;
    }
    //找出第一個包含該物品的slot
    public Inventory.Slot FindFirstSlotWith(string inventoryName, ItemData data)
    {
        if (inventoryByName.TryGetValue(inventoryName, out Inventory inventory))
        {
            return inventory.FindFirstSlotWith(data);
        }
        return null;
    }

    //減少特定物品
    public bool TryRemove(string inventoryName, ItemData data, int amount = 1)
    {
        if (inventoryByName.TryGetValue(inventoryName, out Inventory inventory))
        {
            return inventory.TryRemoveItem(data, amount);
        }
        Debug.LogWarning($"❌ 無法移除，找不到名為 {inventoryName} 的 Inventory");
        return false;
    }
    
    /// 清除 Backpack 和 Toolbar 中的所有物品（不影響 Storagebox）
    public void ClearBackpackAndToolbar()
    {
        if (backpack != null)
        {
            backpack.ClearInventory();
            Debug.Log("Backpack 已清空");
        }

        if (toolbar != null)
        {
            toolbar.ClearInventory();
            Debug.Log("Toolbar 已清空");
        }

        // 若 UI 需同步刷新
        foreach (Inventory_UI ui in Resources.FindObjectsOfTypeAll<Inventory_UI>())
        {
            ui.Reconnect(); // 讓 UI 顯示最新內容
        }
    }



}
