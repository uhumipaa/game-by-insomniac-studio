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
                Item item = GameManager.instance.itemManager.GetItemByType(player.inventory.slots[i].type);

                if (item != null && player.inventory.slots[i].count > 0)
                {
                    slots[i].SetItem(player.inventory.slots[i], item); // 把 item 傳進 UI slot
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    //當X被按下
    public void Remove(int slotID)
    {
        Debug.Log($"使用中的 GameManager：{GameManager.instance.name}, itemManager 是否為 null？{GameManager.instance.itemManager == null}");

        Debug.Log($"slot[{slotID}] 資訊 → type: {player.inventory.slots[slotID].type}, itemName: {player.inventory.slots[slotID].itemName}, count: {player.inventory.slots[slotID].count}");

        Debug.Log("Remove() 被按下，slotID = " + slotID);
        Debug.Log("欲移除物品名稱：" + player.inventory.slots[slotID].itemName);

        Item itemToDrop = GameManager.instance.itemManager.GetItemByType(player.inventory.slots[slotID].type);

        if(itemToDrop == null)
        {
            Debug.LogWarning($"找不到 itemToDrop，無法移除。類型為：{player.inventory.slots[slotID].type}");
            return;
        }   


        if(itemToDrop != null)
        {
            Vector3 originalPos = player.inventory.slots[slotID].originalDropPosition;
            player.DropItem(itemToDrop, originalPos);
            player.inventory.Remove(slotID);
            Refresh();
        }
        
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
