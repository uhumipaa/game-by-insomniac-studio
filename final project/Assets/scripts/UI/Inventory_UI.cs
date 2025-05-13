using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Inventory_UI : MonoBehaviour
{
    public enum InventoryType { Inventory, Toolbar }
    public InventoryType uiType;
    public string inventoryName;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;

    private bool dragSingle;
    private Inventory inventory;
    

    private void Awake()
    {
        canvas = Object.FindFirstObjectByType<Canvas>();

        // 只抓 Slots 子物件裡的 Slot_UI
        Transform slotRoot = transform.Find("Background/Slots");
        if (slotRoot != null)
        {
            slots = new List<Slot_UI>(slotRoot.GetComponentsInChildren<Slot_UI>());
            //Debug.Log($"已載入背包格數：{slots.Count}");
        }
        else
        {
            //Debug.LogError("找不到 Slots 節點！請確認階層是否為 inventory/Background/Slots");
        }
    }
    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);

        //設置SLOT ID
        SetupSlots();

        //清空slot
        Refresh();
    }
   
    public void Refresh()
    {
        if(slots.Count == inventory.slots.Count)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                var sourceSlot = inventory.slots[i];
                //Debug.Log($"刷新 Slot[{i}] → {sourceSlot.itemName} x{sourceSlot.count}");

                Collectable prefab = GameManager.instance.itemManager.GetCollectablePrefab(inventory.slots[i].type);

                /*if (prefab == null)
                {
                    Debug.LogWarning($"❌ 無法取得 prefab，slot[{i}] 的 type 是 {sourceSlot.type}");
                }*/

                if (prefab != null && inventory.slots[i].count > 0)
                {
                    //Debug.Log($"⚠ 嘗試設置 slot[{i}]，圖示為：{prefab.item?.data?.icon?.name}");
                    slots[i].SetItem(inventory.slots[i], prefab.item); // 把 item 傳進 UI slot
                }
                else
                {
                    //Debug.Log($"❌ 跳過 slot[{i}]，prefab.item 為 null？count={player.inventory.slots[i].count}");
                    slots[i].SetEmpty();
                }
            }
        }
    }

    //拖曳
    public void Remove()
    {
        var slot = inventory.slots[UI_Manager.draggedSlot.slotID];

        Collectable prefab = GameManager.instance.itemManager.GetCollectablePrefab(slot.type);

        if(prefab == null)
        {
            Debug.LogWarning($"找不到 itemToDrop，無法移除。類型為：{slot.type}");
            return;
        }   

        if(prefab != null)
        {
            if(UI_Manager.dragSingle)
            {
                GameManager.instance.player.DropItem(slot.type);
                inventory.Remove(UI_Manager.draggedSlot.slotID);
            }
            else
            {
                GameManager.instance.player.DropItem(slot.type, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                inventory.Remove(UI_Manager.draggedSlot.slotID, inventory.slots[UI_Manager.draggedSlot.slotID].count);
            }
            
            Refresh();
        }
        
        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;
        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon); //複製一個新icon
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform);
        UI_Manager.draggedIcon.raycastTarget = false;
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        //Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UI_Manager.draggedIcon.gameObject); //圖標可以跟著滑鼠連續移動
        //Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(UI_Manager.draggedIcon.gameObject); //鼠標放開，圖片消失
        UI_Manager.draggedIcon = null;

        //Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(Slot_UI slot)
    {
        if(UI_Manager.dragSingle)
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);    
        }
        else
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                                                      UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count); 
        }

        GameManager.instance.uiManager.RefreshAll();
        
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void SetupSlots()
    {
        int counter = 0;

        foreach(Slot_UI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }
}
