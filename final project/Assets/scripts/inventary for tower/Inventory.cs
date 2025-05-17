using NUnit.Framework.Interfaces;
using UnityEngine;

public class TowerBagItem
{
    public RewardData itemData;
    public int quantity;
    public TowerBagItem(RewardData data, int qty = 1)
    {
        itemData = data;
        quantity = qty;
    }
}
