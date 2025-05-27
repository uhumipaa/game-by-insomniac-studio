using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Slot_UI : MonoBehaviour//,IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int slotID;
    public Inventory inventory;
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    [SerializeField] InventoryInfo info;
    [SerializeField] private GameObject highlight;
    private float lastClickTime;
    [SerializeField] private const float doubleClickThreshold = 0.3f;
    [SerializeField] private EquipmentSlot[] equipmentSlots;
    void Awake()
    {
        Debug.Log($"🟢 Slot_UI 初始化: {gameObject.name}");
    if (GetComponent<Image>() == null)
        Debug.LogError("❌ 缺少 Image 元件！");
    else
        Debug.Log($"Image Raycast Target: {GetComponent<Image>().raycastTarget}");
        if (info == null)
            info = FindAnyObjectByType<InventoryInfo>();
    }
    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null && slot.count > 0)
        {
            if (slot.itemData.icon != null)
            {
                itemIcon.sprite = slot.itemData.icon;
                itemIcon.color = new Color(1, 1, 1, 1);
                quantityText.text = slot.count.ToString();
                Debug.Log($"SetItem() → 設定 icon：{slot.itemData.icon.name}，數量：{slot.count}");
            }
            else
            {
                Debug.LogWarning("❌ Slot 的 icon 為 null，無法顯示圖示。");
                itemIcon.color = new Color(1, 1, 1, 0); // 隱藏
                quantityText.text = "";
            }
        }
        else
        {
            Debug.LogWarning("❌ SetItem() 收到空 slot 或 count <= 0。");
            SetEmpty();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);

    }
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("🟡 OnPointerEnter 被觸發");
        if (info == null)
        {
            return;
        }

        if (inventory != null && slotID >= 0 && slotID < inventory.slots.Count)
        {
            var slot = inventory.slots[slotID];
            if (!slot.IsEmpty)
            {
                Debug.Log("🟡 OnPointerEnter 被觸發2");
                info.showinfo(slot.itemData);
            }
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (info == null)
        {
            return;
        }
        info.hideinfo();
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (info == null)
        {
            return;
        }

        if (inventory != null && slotID >= 0 && slotID < inventory.slots.Count)
        {
            var slot = inventory.slots[slotID];
            if (!slot.IsEmpty)
            {
                info.followmouse();
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            trytoequip();
        }

        lastClickTime = Time.time;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventory == null)
        {
            Debug.LogError($"❌ 無法拖曳，inventory 為 null (slot {slotID})");
                return;
        }

        FindFirstObjectByType<Inventory_UI>()?.SlotBeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        FindFirstObjectByType<Inventory_UI>()?.SlotDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        FindFirstObjectByType<Inventory_UI>()?.SlotEndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        FindFirstObjectByType<Inventory_UI>()?.SlotDrop(this);
    }
    */
    private void trytoequip()
    {
        if (inventory != null && slotID >= 0 && slotID < inventory.slots.Count)
        {
            var slot = inventory.slots[slotID];
            if (!slot.IsEmpty)
            {
                int findemptyaccesaor = 0;
                ItemData item = slot.itemData;
                foreach (EquipmentSlot equipmentSlot in equipmentSlots)
                {
                    if (item.type == equipmentSlot.type)
                    {
                        if (item.type == ItemType.accesasor && equipmentSlot.itemData != null)
                        {
                            if (findemptyaccesaor < 2)
                            {
                                findemptyaccesaor++;
                                continue;
                            }
                        }
                        equipmentSlot.equip(item, slotID);
                    }
                }
            }
        }
    }
    
}
