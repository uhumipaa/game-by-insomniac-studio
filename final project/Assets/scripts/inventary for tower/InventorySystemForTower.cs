using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class BagSystemForTower : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static BagSystemForTower Instance;
    public List<TowerBagItem> items = new List<TowerBagItem>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void AddItem(RewardData ItemData)
    {
        var existing = items.Find(i => i.itemData == ItemData);
        if (existing != null)
        {
            existing.quantity++;
        }
        else
        {
            items.Add(new TowerBagItem(ItemData));
        }
        Debug.Log($"­I¥]·s¼W¡G{ItemData.rewardname}");
        TowerBagUI.instance.Refresh();
    }
}
