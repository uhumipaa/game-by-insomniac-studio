using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveEquippment
{
    public string itemname;
    public int slotindex;
    public SaveEquippment(string name, int index)
    {
        itemname = name;
        slotindex = index;
    }
}
public class EquipmentSlot : MonoBehaviour, ISaveData
{
    [SerializeField] int slotindex;
    public ItemType type;
    public ItemData itemData;
    [SerializeField] private SpriteRenderer sword;
    [SerializeField] private SpriteRenderer staff;
    public Image icon;
    bool isload;
    [SerializeField] Slot_UI[] slot_UIs;
    void Start()
    {
        sword = GameObject.Find("sword").GetComponent<SpriteRenderer>();
        staff = GameObject.Find("staff").GetComponent<SpriteRenderer>();
    }

    public void equip(ItemData newitem)
    {
        if (itemData != null)
        {
            if (!isload)
            {

                PlayerStatusManager.instance.diff_status(itemData);
                InventoryManager.Instance.Add("Backpack", itemData, 1);
                Debug.Log("4das5f4af");
            }
        }
        itemData = newitem;
        icon.sprite = itemData.icon;
        if (!isload)
        {
            InventoryManager.Instance.TryRemove("Backpack", itemData, 1);
            PlayerStatusManager.instance.add_status(itemData);
        }
        if (type == ItemType.sword)
        {
            sword.sprite = itemData.icon;
        }
        else if (type == ItemType.staff)
        {
            staff.sprite = itemData.icon;
        }
        foreach (Slot_UI ui in slot_UIs)
        {
                    ui.Refresh();
        }
    }

    public void SaveData(ref SaveData saveData)
    {
        Debug.Log("儲存裝備資料中");
        if (itemData != null)
        {
            Debug.Log("545fa");
            saveData.equippmentItems.Add(new SaveEquippment(itemData.itemName, slotindex));
        }
        else
        {
            Debug.Log("166654a");
        }
    }

    public void LoadData(SaveData saveData)
    {
        itemData = null;
        List<SaveEquippment> saveequipment = new(saveData.equippmentItems);
        for (int i = 0; i < saveequipment.Count; i++)
        {
            SaveEquippment saved = saveequipment[i];
            int index = saved.slotindex;
            ItemData data = ItemManager.instance.GetItemDataByName(saved.itemname);
            if (data.type == type&&index==slotindex)
            {
                isload = true;
                equip(data);
                isload = false;
            }
        }
    }

}
