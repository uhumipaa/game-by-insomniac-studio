using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryUIEntry
{
    public string inventoryName;
    public GameObject prefab;
}

public class UI_Manager : MonoBehaviour
{
    public List<InventoryUIEntry> inventoryUIPrefabs;
    public Transform uiCanvas;
    //public GameObject inventoryPanel;

    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();
    //public List<Inventory_UI> inventoryUIs = new List<Inventory_UI>();

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    private static bool initialized = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // UI Manager 跨場景
        //Initialize();
    }

    private void Start()
    {
        StartCoroutine(InitializeUIOnce());
    }

    private IEnumerator InitializeUIOnce()
    {
        if (initialized) yield break;
        initialized = true;// 等待 InventoryManager 初始化

        yield return null;

        Transform canvasParent = CanvasManager.Instance.UIRoot;

        foreach (var entry in inventoryUIPrefabs)
        {
            if (!inventoryUIByName.ContainsKey(entry.inventoryName))
            {
                GameObject uiGO = Instantiate(entry.prefab, canvasParent);
                DontDestroyOnLoad(uiGO);

                var ui = uiGO.GetComponent<Inventory_UI>();
                ui.inventoryName = entry.inventoryName;
                ui.Reconnect();

                inventoryUIByName.Add(entry.inventoryName, ui);
                //inventoryUIs.Add(ui);

                //一開始關閉背包視窗
                if (entry.inventoryName == "Backpack")
                {
                    uiGO.SetActive(false);
                }
            }
        }

        RefreshAll();
    }
    private void Update()
    {
        //長按左邊shift拖曳一樣東西
        if (Input.GetKey(KeyCode.LeftShift))
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
            /*//如果視窗是關著且遊戲暫停
            if (!inventoryPanel.activeSelf && PauseController.IsGamePaused)
            {
                return;
            }*/

            ToggleInventoryUI();
            //PauseController.SetPause(inventoryPanel.activeSelf); //視窗開啟時暫停
        }
    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
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

        GameObject backpackGO = targetUI.gameObject;

        bool isActive = backpackGO.activeSelf;
        backpackGO.SetActive(!isActive); //開啟或關閉backpack視窗

        //同步 raycastTarget 開關
        if (targetUI.panelImage != null)
        {
            targetUI.panelImage.raycastTarget = !isActive;
            Debug.Log($"raycast target : {!isActive}");
        }
            

        if (!isActive) //如果視窗顯示 -> 刷新
        {
            targetUI.Refresh();
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach (KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            if (keyValuePair.Value.isReady)
            {
                keyValuePair.Value.Refresh();
            }
            else
            {
                Debug.LogWarning($"跳過刷新 {keyValuePair.Key}：尚未初始化完成");
            }
        }
    }

    /*void Initialize()
    {
        foreach (Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }

            // 設定 UI 的初始顯示狀態
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(false); // 背包一開始隱藏
            }
        }
    }*/
}
