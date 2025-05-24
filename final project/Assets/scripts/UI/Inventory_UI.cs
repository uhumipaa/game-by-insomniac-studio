using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory_UI : MonoBehaviour
{
    [SerializeField] public Image panelImage; //backpack 視窗
    public enum InventoryType { Inventory, Toolbar }
    public InventoryType uiType;
    public string inventoryName;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;

    private Inventory inventory;
    public bool isReady = false;

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        Debug.Log($"🟢 Inventory_UI 綁定 Canvas：{canvas?.name}");

        // 抓 Slots 子物件
        Transform slotRoot = transform.Find("Background/Slots");
        if (slotRoot != null)
            slots = new List<Slot_UI>(slotRoot.GetComponentsInChildren<Slot_UI>());
    }

    private IEnumerator Start()
    {
        yield return null;
        Reconnect();
    }

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var sourceSlot = inventory.slots[i];
                if (sourceSlot.count > 0)
                    slots[i].SetItem(sourceSlot);
                else
                    slots[i].SetEmpty();
            }
        }
    }

    public void Remove()
    {
        if (UI_Manager.draggedSlot == null)
        {
            Debug.LogWarning("❗ 無法刪除，draggedSlot 為 null");
            return;
        }

        if (inventory == null)
        {
            Debug.LogWarning("❗ inventory 為 null，可能尚未初始化");
            return;
        }

        int id = UI_Manager.draggedSlot.slotID;
        if (id < 0 || id >= inventory.slots.Count)
        {
            Debug.LogWarning($"❗ slotID 超出範圍：{id}");
            return;
        }
        var slot = inventory.slots[UI_Manager.draggedSlot.slotID];

        if (UI_Manager.dragSingle)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            gm.player.DropItem(slot.itemData.type);
            inventory.Remove(UI_Manager.draggedSlot.slotID);
        }
        else
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
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
        if (UI_Manager.dragSingle)
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
        else
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);

        FindFirstObjectByType<GameManager>().uiManager.RefreshAll();
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
        var receiver = transform.parent.GetComponentInChildren<DropReceiver>();
        if (receiver != null)
        {
            receiver.Initialize(this);
            Debug.Log("✅ DropReceiver 綁定成功！");
        }
        else
        {
            Debug.LogWarning("⚠ 沒有找到 DropReceiver，請確認 RemoveItem_panel 上是否有掛腳本");
        }
    }

    //按下X
    public void CloseUI()
    {
        //如果backpack關閉 raycastTarget關閉
        if (panelImage != null) panelImage.raycastTarget = false;

        gameObject.SetActive(false);
    }
}
