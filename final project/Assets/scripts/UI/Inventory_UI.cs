using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Inventory_UI : MonoBehaviour
{
    public enum InventoryType { Inventory, Toolbar }
    public InventoryType uiType;
    public GameObject inventoryPanel;
    public player_trigger player;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;
    private bool dragSingle;
    

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
        //設置SLOT ID
        SetupSlots();

        if (uiType == InventoryType.Inventory)
        {
            inventoryPanel.SetActive(false); // 背包一開始隱藏
        }
        else if (uiType == InventoryType.Toolbar)
        {
            inventoryPanel.SetActive(true);  // 工具欄一開始顯示
        }

        //清空slot
        Refresh();
    }
    void Update()
    {
        //按tab鍵打開視窗(之後換成按鈕)
        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            ToggleInventory();
        }

        //長按左邊shift拖曳一樣東西
        if(Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }
    }

    public void ToggleInventory()
    {
        // 僅當這個 UI 是 Inventory 類型時才切換顯示
        if (uiType != InventoryType.Inventory) return;

        if(inventoryPanel != null)
        {
            if(!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                Refresh();
            }
            else
            {
                inventoryPanel.SetActive(false);
            }
        }
        
    }

    void Refresh(){

        if(slots.Count == player.inventory.slots.Count)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                var sourceSlot = player.inventory.slots[i];
                Debug.Log($"刷新 Slot[{i}] → {sourceSlot.itemName} x{sourceSlot.count}");

                Collectable prefab = GameManager.instance.itemManager.GetCollectablePrefab(player.inventory.slots[i].type);

                /*if (prefab == null)
                {
                    Debug.LogWarning($"❌ 無法取得 prefab，slot[{i}] 的 type 是 {sourceSlot.type}");
                }*/

                if (prefab != null && player.inventory.slots[i].count > 0)
                {
                    //Debug.Log($"⚠ 嘗試設置 slot[{i}]，圖示為：{prefab.item?.data?.icon?.name}");
                    slots[i].SetItem(player.inventory.slots[i], prefab.item); // 把 item 傳進 UI slot
                }
                else
                {
                    //Debug.Log($"❌ 跳過 slot[{i}]，prefab.item 為 null？count={player.inventory.slots[i].count}");
                    slots[i].SetEmpty();
                }
            }
        }
        else if(slots.Count == player.toolbar.slots.Count)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                //Debug.Log($"刷新 slot[{i}] 類型：{player.inventory.slots[i].type} 數量：{player.inventory.slots[i].count}");

                Collectable prefab = GameManager.instance.itemManager.GetCollectablePrefab(player.toolbar.slots[i].type);

                if (prefab != null && player.toolbar.slots[i].count > 0)
                {
                    //Debug.Log($"⚠ 嘗試設置 slot[{i}]，圖示為：{prefab.item?.data?.icon?.name}");
                    slots[i].SetItem(player.toolbar.slots[i], prefab.item); // 把 item 傳進 UI slot
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
        var slot = player.inventory.slots[draggedSlot.slotID];

        Collectable prefab = GameManager.instance.itemManager.GetCollectablePrefab(slot.type);

        if(prefab == null)
        {
            Debug.LogWarning($"找不到 itemToDrop，無法移除。類型為：{slot.type}");
            return;
        }   

        if(prefab != null)
        {
            if(dragSingle)
            {
                player.DropItem(slot.type);
                player.inventory.Remove(draggedSlot.slotID);
            }
            else
            {
                player.DropItem(slot.type, player.inventory.slots[draggedSlot.slotID].count);
                player.inventory.Remove(draggedSlot.slotID, player.inventory.slots[draggedSlot.slotID].count);
            }
            
            Refresh();
        }
        
        draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon); //複製一個新icon
        draggedIcon.transform.SetParent(canvas.transform);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(draggedIcon.gameObject);
        //Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject); //圖標可以跟著滑鼠連續移動
        //Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject); //鼠標放開，圖片消失
        draggedIcon = null;

        //Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(Slot_UI slot)
    {
        player.inventory.MoveSlot(draggedSlot.slotID, slot.slotID);
        Refresh();
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
        }
    }
}
