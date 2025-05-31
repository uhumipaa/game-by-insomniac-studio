using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory_UI : MonoBehaviour
{
    [SerializeField] CanvasGroup[] UIs;
    [SerializeField] public Image panelImage; //backpack 視窗
    public enum InventoryType { Inventory, Toolbar }
    public enum SlotGroup { Backpack, Equipment }

    public InventoryType uiType;
    public string inventoryName;
    public List<Slot_UI> slots = new List<Slot_UI>();
    public List<Slot_UI> bagslots = new List<Slot_UI>();
    public List<Slot_UI> equipslots = new List<Slot_UI>();
    [SerializeField] private Canvas canvas;
    [SerializeField] int equipmentoffset;
    [SerializeField] bool isequipment = false;

    public Inventory inventory;
    public bool isReady = false;
    [SerializeField] private int slotOffset = 0;
    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        Debug.Log($"🟢 Inventory_UI 綁定 Canvas：{canvas?.name}");

        // 抓 Slots 子物件
        Transform bagslotRoot = transform.Find("Background/Slots");

        slots = new List<Slot_UI>(bagslotRoot.GetComponentsInChildren<Slot_UI>());

    }

    private IEnumerator Start()
    {
        yield return null;
        Reconnect();
    }

    public void Refresh()
    {
        //if (slots.Count == inventory.slots.Count)
        //{
        /*
        if (isequipment)
        {
            for (int i = equipmentoffset; i < slots.Count; i++)
            {
                var sourceSlot = inventory.slots[i];
                if (sourceSlot.count > 0)
                    slots[i].SetItem(sourceSlot);
                else
                    slots[i].SetEmpty();
            }
        }
        */
        for (int i = 0; i < slots.Count&&i<inventory.slots.Count; i++)
        {
            var sourceSlot = inventory.slots[i];
            if (sourceSlot.count > 0)
                slots[i].SetItem(sourceSlot);
            else
                slots[i].SetEmpty();
        }
        //}
    }

    public void Remove()
    {
        /*if (UI_Manager.draggedSlot == null)
        {
            Debug.LogWarning("❗ 無法刪除，draggedSlot 為 null");
            return;
        }

        if (inventory == null)
        {
            Debug.LogWarning("❗ inventory 為 null，可能尚未初始化");
            return;
        }*/

        int id = UI_Manager.draggedSlot.slotID;
        if (id < 0 || id >= inventory.slots.Count)
        {
            Debug.LogWarning($"❗ slotID 超出範圍：{id}");
            return;
        }

        var slot = inventory.slots[UI_Manager.draggedSlot.slotID];
        GameManager gm = FindFirstObjectByType<GameManager>();

        /////
        if (gm == null)
        {
            Debug.LogError("❌ 找不到 GameManager！");
            return;
        }

        if (gm.player == null)
        {
            Debug.LogWarning("⚠ GameManager.player 為 null，嘗試自動尋找並綁定");
            var player = FindFirstObjectByType<player_trigger>();
            if (player != null)
            {
                gm.player = player;
                Debug.Log("✅ 已自動補綁 GameManager.player");
            }
            else
            {
                Debug.LogError("❌ 找不到 player_trigger，無法放置物品");
                return;
            }
        }
        ////

        if (UI_Manager.dragSingle)
        {
            gm.player.DropItem(slot.itemData.type);
            inventory.Remove(UI_Manager.draggedSlot.slotID);
        }
        else
        {
            gm.player.DropItem(slot.itemData.type, slot.count);
            inventory.Remove(UI_Manager.draggedSlot.slotID, slot.count);
        }

        Refresh();
        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        if (slot.inventory == null)
        {
            Debug.LogError($"❌ Slot ID {slot.slotID} 的 inventory 為 null，無法開始拖曳！");
            return;
        }

        UI_Manager.draggedSlot = slot;
        UI_Manager.draggedIcon = Instantiate(slot.itemIcon);
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform);
        UI_Manager.draggedIcon.transform.SetAsLastSibling(); // 確保顯示在最上層
        UI_Manager.draggedIcon.raycastTarget = false;
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        var canvasGroup = UI_Manager.draggedIcon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = UI_Manager.draggedIcon.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotDrag()
    {
        if (UI_Manager.draggedIcon != null)
            MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        if (UI_Manager.draggedIcon != null)
            Destroy(UI_Manager.draggedIcon.gameObject);

        UI_Manager.draggedIcon = null;

    }

    public void SlotDrop(Slot_UI slot)
    {
        var fromSlot = UI_Manager.draggedSlot;
        var toSlot = slot;

        if (UI_Manager.dragSingle)
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
        else
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);

        //立即刷新拖曳來源與目標 slot
        //fromSlot.Refresh();
        //toSlot.Refresh();

        /*FindFirstObjectByType<GameManager>().uiManager.RefreshAll();*/

        //UI_Manager.draggedSlot = null;
        //UI_Manager.draggedIcon = null;

        //先刪除拖曳圖示，避免殘影
        if (UI_Manager.draggedIcon != null)
            Destroy(UI_Manager.draggedIcon.gameObject);

        UI_Manager.draggedIcon = null;
        UI_Manager.draggedSlot = null;

        //先刷新目標 slot
        toSlot.Refresh();

        //嘗試從場景中找到 fromSlot 的 UI 並刷新
        bool sourceUIRefreshed = false;

        foreach (var ui in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (ui is Inventory_UI inventoryUI && inventoryUI.inventory == fromSlot.inventory)
            {
                Debug.Log($"✅ 找到來源 UI：{inventoryUI.inventoryName}");
                inventoryUI.Refresh();
                sourceUIRefreshed = true;
                break;
            }
            else if (ui is storageBox_UI storageUI && storageUI.inventory == fromSlot.inventory)
            {
                Debug.Log("✅ 找到來源是 storagebox");
                storageUI.Refresh();
                sourceUIRefreshed = true;
                break;
            }
        }

        //fallback：如果找不到來源 UI，就刷新 fromSlot 自己
        if (!sourceUIRefreshed)
        {
            Debug.Log("⚠️ 沒找到來源 UI，直接刷新 fromSlot");
            fromSlot.Refresh();
        }

        Debug.Log($"🧩 fromSlot.inventory == toolbarInventory? {fromSlot.inventory == InventoryManager.Instance.toolbar}");

    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                null,
                out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void SetupSlots()
    {
        int counter = 0;
        //if (isequipment) counter += equipmentoffset;
        foreach (Slot_UI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }

    public void Reconnect()
    {
        inventory = InventoryManager.Instance.GetInventoryByName(inventoryName);

        if (inventory == null)
        {
            Debug.LogError($"❌ 無法綁定 Inventory：{inventoryName}");
            return;
        }

        SetupSlots();
        Refresh();
        isReady = true;

        // 初始化 Remove 面板的 DropReceiver 綁定
        var receiver = GetComponentInChildren<DropReceiver>(); ;
        if (receiver != null)
        {
            receiver.Initialize(this);
            Debug.Log("✅ DropReceiver 綁定成功！");
        }
        else
        {
            //Debug.LogWarning("⚠ 沒有找到 DropReceiver，請確認 RemoveItem_panel 上是否有掛腳本");
        }
    }
    public void switchtobackpack()
    {
        UIs[0].alpha = 1;
        UIs[0].blocksRaycasts = true;
        UIs[0].interactable = true;

        UIs[1].alpha = 0;
        UIs[1].blocksRaycasts = false;
        UIs[1].interactable = false;
    }
    public void switchtostatus()
    {
        UIs[0].alpha = 0;
        //UIs[0].blocksRaycasts = false;
        //UIs[0].interactable = false;

        UIs[1].alpha = 1;
        //UIs[1].blocksRaycasts = true;
        //UIs[1].interactable = true;
        UIs[1].GetComponent<IventoryStatus>().setstatus();
    }

    private string GetInventoryName(Inventory inv)
    {
        foreach (var ui in FindObjectsByType<Inventory_UI>(FindObjectsSortMode.None))
        {
            if (ui.inventory == inv)
                return ui.inventoryName;
        }
        return "";
    }
    //按下X
    public void CloseUI()
    {
        //如果backpack關閉 raycastTarget關閉
        if (panelImage != null) panelImage.raycastTarget = false;

        //backpack面板關閉
        gameObject.SetActive(false);
    }


}
