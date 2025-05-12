using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();
    public GameObject inventoryPanel;
    public List<Inventory_UI> inventoryUIs;
    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        //長按左邊shift拖曳一樣東西
        if(Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }

        // 按下 Tab 切換背包視窗
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryUI();
        }
    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        Debug.LogWarning("There is no inventory ui for" + inventoryName);
        return null;
    }

    public void ToggleInventoryUI()
    {
        //按tab顯示inventory window
        //取得對應的 Inventory_UI 來判斷

        Inventory_UI targetUI = GetInventoryUI("Backpack"); 

        if (targetUI == null) return;

        // 僅當這個 UI 是 Inventory 類型時才切換顯示
        if (targetUI.uiType != Inventory_UI.InventoryType.Inventory) return;

        if(inventoryPanel != null)
        {
            if(!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else
            {
                inventoryPanel.SetActive(false);
            }
        }  
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach(KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    void Initialize()
    {
        foreach(Inventory_UI ui in inventoryUIs)
        {
            if(!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }

             // 設定 UI 的初始顯示狀態
            if (inventoryPanel != null)
            {
                if (ui.uiType == Inventory_UI.InventoryType.Inventory)
                {
                    inventoryPanel.SetActive(false); // 背包一開始隱藏
                }
                else if (ui.uiType == Inventory_UI.InventoryType.Toolbar)
                {   
                    inventoryPanel.SetActive(true); // 工具欄一開始顯示
                }
            }
        }
    }
}
