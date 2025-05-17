using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        //public string itemName;
        public ItemData itemData;
        public int count;
        public bool IsEmpty => itemData == null || count <= 0;
        //public int maxAllowed;
        //public Sprite icon;
        //public ItemType type;
        //public Vector3 originalDropPosition; //原始掉落位置

        /*public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }*/

        /*//判斷格子是否為空
        public bool IsEmpty
        {
            get
            {
                if(itemName == "" && count == 0)
                {
                    return true;
                }
                
                return false;
            }
        }*/
        
        public bool CanAddItem(ItemData data)
        {
            return itemData == data && count < data.maxAllowed;
        }

        public void AddItem(ItemData data, int amount = 1)
        {
            //如果格子是空的
            if (IsEmpty)
            {
                itemData = data;
                count = Mathf.Min(amount, data.maxAllowed);
            }
            //如果格子裡和要加的物品是同樣的
            else if (itemData == data)
            {
                count = Mathf.Min(count + amount, data.maxAllowed);
            }
        }

        /*public void AddItem(string itemName, Sprite icon, ItemType type, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.type = type;
            //this.originalDropPosition = item.transform.position; // 記住撿起前的位置
            this.count++; 
            this.maxAllowed = maxAllowed;
        }*/


        public void RemoveItem(int amount = 1)
        {
            if (!IsEmpty)
            {
                count -= amount;
                if (count <= 0)
                {
                    itemData = null;
                    count = 0;
                }
            }
        }
    }

    public List<Slot> slots = new List<Slot>();
    public Slot selectedSlot = null;

    public Inventory(int numSlots)
    {
        for(int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    public void Add(ItemData data, int amount = 1)
    {
        //找格子裡相同data的
        foreach (Slot slot in slots)
        {
            if (slot.CanAddItem(data))
            {
                slot.AddItem(data, amount);
                return;
            }
        }

        //如果沒有再找空格子
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(data, amount);
                return;
            }
        }
        Debug.LogWarning("沒有空格可以放置物品！");
    }

    public void Remove(int index, int amount = 1)
    {
        if (index >= 0 && index < slots.Count)
        {
            slots[index].RemoveItem(amount);
        }
    }

    //在inventory視窗上的格子間挪動
    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)  //預設移動1個
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if(toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemData)) //如果格子為空或形態相符才可以挪
        {
            toSlot.AddItem(fromSlot.itemData, numToMove);
            fromSlot.RemoveItem(numToMove);    
        }
    }

    //得到選擇的格子
    public void SelectSlot(int index)
    {
        if(slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }
}
