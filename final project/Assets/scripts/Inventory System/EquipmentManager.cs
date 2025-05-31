using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour,ISaveData
{
    [SerializeField] List<SaveEquippment> equipDatas;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    public static EquipmentManager instance;

    public void LoadData(SaveData saveData)
    {
        /*
        foreach (SaveEquippment equippment in equipDatas)
        {
            equipmentSlots[equippment.slotindex].itemData = null;
        }
        */
        equipDatas.Clear();
        List<SaveEquippment> equipDatass = new(saveData.equippmentItems);
        foreach (SaveEquippment saveequipment in equipDatass)
        {
            int index = saveequipment.slotindex;
            ItemData data = ItemManager.instance.GetItemDataByName(saveequipment.itemname);
            equipDatas.Add(saveequipment);
            if (data == null)
            {
                Debug.Log("nulldata");
            }
            else
            {
                equipmentSlots[index].isload = true;
            equipmentSlots[index].equip(data);
            equipmentSlots[index].isload = false;
            }
        }
    }

    public void SaveData(ref SaveData saveData)
    {
        saveData.equippmentItems = equipDatas;
    }

    public void loadequip(ItemData itemData,int index)
    {
        equipDatas.Add(new SaveEquippment(itemData.name, index));
    }
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("has other savesystem");
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        equipmentSlots = Resources.FindObjectsOfTypeAll<EquipmentSlot>();
    }



}
