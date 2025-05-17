using UnityEngine;
[System.Serializable]
public class ShopItemData
{
    public int itemID;
    public ItemData itemData;
    public int price;
    public int quantity = 0;//記錄買了幾次
}
