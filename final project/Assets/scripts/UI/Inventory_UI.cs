using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public player_trigger player;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;
    

    private void Awake()
    {
        canvas = Object.FindFirstObjectByType<Canvas>();

        // 只抓 Slots 子物件裡的 Slot_UI
        Transform slotRoot = transform.Find("Background/Slots");
        if (slotRoot != null)
        {
            slots = new List<Slot_UI>(slotRoot.GetComponentsInChildren<Slot_UI>());
            Debug.Log($"已載入背包格數：{slots.Count}");
        }
        else
        {
            Debug.LogError("找不到 Slots 節點！請確認階層是否為 inventory/Background/Slots");
        }
    }
    void Start()
    {
        // 一開始關閉背包 UI
        inventoryPanel.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) //按tab鍵打開(之後換成按鈕)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
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

    void Refresh(){

        if(slots.Count == player.inventory.slots.Count)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                //Debug.Log($"刷新 slot[{i}] 類型：{player.inventory.slots[i].type} 數量：{player.inventory.slots[i].count}");

                Collectable prefab = GameManager.instance.itemManager.GetCollectablePrefab(player.inventory.slots[i].type);

                if (prefab != null && player.inventory.slots[i].count > 0)
                {
                    Debug.Log($"⚠ 嘗試設置 slot[{i}]，圖示為：{prefab.item?.data?.icon?.name}");
                    slots[i].SetItem(player.inventory.slots[i], prefab.item); // 把 item 傳進 UI slot
                }
                else
                {
                    Debug.Log($"❌ 跳過 slot[{i}]，prefab.item 為 null？count={player.inventory.slots[i].count}");
                    slots[i].SetEmpty();
                }
            }
        }
    }

    //當X被按下
    /*public void ButtonRemove()
    {
        Debug.Log("有點到 X 按鈕");
        
        Item itemToDrop = GameManager.instance.itemManager.GetItemByType(player.inventory.slots[draggedSlot.slotID].type);

        if(itemToDrop == null)
        {
            Debug.LogWarning($"找不到 itemToDrop，無法移除。類型為：{player.inventory.slots[draggedSlot.slotID].type}");
            return;
        }   


        if(itemToDrop != null)
        {
            Vector3 originalPos = player.inventory.slots[draggedSlot.slotID].originalDropPosition;
            player.DropItem(itemToDrop, originalPos);
            player.inventory.Remove(draggedSlot.slotID);
            Refresh();
        }
        
        draggedSlot = null;
    }*/

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
            Vector3 originalPos = player.inventory.slots[draggedSlot.slotID].originalDropPosition;
            player.DropItem(slot.type, originalPos);
            player.inventory.Remove(draggedSlot.slotID);
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
        Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject); //圖標可以跟著滑鼠連續移動
        Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject); //鼠標放開，圖片消失
        draggedIcon = null;

        Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(Slot_UI slot)
    {
        Debug.Log("Dropped " + draggedSlot.name + " on " + slot.name);
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
}
