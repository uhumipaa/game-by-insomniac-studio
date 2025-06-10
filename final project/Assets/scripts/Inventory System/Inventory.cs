using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        
        public ItemData itemData;
        public int count;
        public bool IsEmpty => itemData == null || count <= 0;
    
        //判斷能不能在格子裡加物品
        public bool CanAddItem(ItemData data)
        {
            return itemData == data && count < data.maxAllowed;
        }

        //在格子裡加物品
        public void AddItem(ItemData data, int amount = 1)
        {
            if (data == null) return;
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

        //減少格子裡的物品
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
        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    //新增物品
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

    // 嘗試移除指定物品（數量不足就不移除）
    public bool TryRemoveItem(ItemData data, int amount)
    {
        int total = GetItemCount(data);
        if (total < amount)
        {
            Debug.LogWarning($"❌ 無法移除 {amount} 個 {data.itemName}，因為只有 {total} 個");
            return false;
        }

        for (int i = 0; i < slots.Count && amount > 0; i++)
        {
            var slot = slots[i];
            if (!slot.IsEmpty && slot.itemData == data)
            {
                int removeAmount = Mathf.Min(slot.count, amount);
                slot.RemoveItem(removeAmount);
                amount -= removeAmount;
            }
        }
        
        return true;
    }

    //在inventory視窗上的格子間挪動
    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)  //預設移動1個
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if (toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemData)) //如果格子為空或形態相符才可以挪
        {
            toSlot.AddItem(fromSlot.itemData, numToMove);
            fromSlot.RemoveItem(numToMove);
        }
    }

    //得到選擇的格子
    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }

    //inventory清空
    public void ClearInventory()
    {
        foreach (var slot in slots)
        {
            slot.itemData = null;
            slot.count = 0;
        }
    }

    //尋找是否有特定物品
    public bool Contains(ItemData data)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.itemData == data)
            {
                return true;
            }
        }
        return false;
    }

    //總共有該物品幾個
    public int GetItemCount(ItemData data)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.itemData == data)
            {
                total += slot.count;
            }
        }
        return total;
    }

    //找出第一個包含該物品的slot
    public Slot FindFirstSlotWith(ItemData data)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.itemData == data)
            {
                return slot;
            }
        }
        return null;
    }

}
