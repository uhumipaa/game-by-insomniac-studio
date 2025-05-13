using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int maxAllowed;
        public Sprite icon;
        public ItemType type;
        public Vector3 originalDropPosition; //原始掉落位置

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        //判斷格子是否為空
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
        }
        
        public bool CanAddItem(string itemName)
        {
            if(this.itemName == itemName && count < maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(Item item)
        {
            this.itemName = item.data.itemName;
            this.type = item.data.type;
            this.icon = item.data.icon;
            //this.originalDropPosition = item.transform.position; // 記住撿起前的位置
            this.count++; 
        }

        public void AddItem(string itemName, Sprite icon, ItemType type, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.type = type;
            //this.originalDropPosition = item.transform.position; // 記住撿起前的位置
            this.count++; 
            this.maxAllowed = maxAllowed;
        }


        public void RemoveItem()
        {
            if(count > 0)
            {
                count--;
                if(count == 0)
                {
                    icon = null;
                    itemName = "";
                    type = ItemType.NONE;
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

    public void Add(Item item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach(Slot slot in slots)
        {
            if(slot.itemName == "")
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    //移除一項物品
    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    //移除多項物品
    public void Remove(int index, int numToRemove)
    {
        if(slots[index].count >= numToRemove)
        {
            for(int i = 0; i < numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    //在inventory視窗上的格子間挪動
    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)  //預設移動1個
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if(toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemName)) //如果格子為空或形態相符才可以挪
        {
            for(int i = 0; i < numToMove; i++)
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.type, fromSlot.maxAllowed);
                fromSlot.RemoveItem();
            }
            
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
