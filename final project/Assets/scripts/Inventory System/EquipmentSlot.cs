using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public ItemType type;
    public ItemData itemData;
    [SerializeField]private SpriteRenderer sword;
    [SerializeField]private SpriteRenderer staff;
    public Image icon;
    [SerializeField] Inventory inventory;
    void Start()
    {
        sword = GameObject.Find("sword").GetComponent<SpriteRenderer>();
        staff = GameObject.Find("staff").GetComponent<SpriteRenderer>();
    }

    public void equip(ItemData newitem,int slotID)
    {
        if (itemData != null)
        {
            PlayerStatusManager.instance.diff_status(itemData);
            InventoryManager.Instance.Add("Backpack", itemData, 1);
        }
        
        itemData = newitem;
        icon.sprite = itemData.icon;
        PlayerStatusManager.instance.add_status(itemData);
        if (type == ItemType.sword)
        {
            sword.sprite = itemData.icon;
        }
        else if (type == ItemType.staff)
        {
            staff.sprite = itemData.icon;
        }
    }
}
