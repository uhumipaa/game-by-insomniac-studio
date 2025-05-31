using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class Backpackcheat : MonoBehaviour
{
    public List<ItemData> goldfingerItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void goldfingerbackpack()
    {
        for (int i = 0; i < 28; i++)
        {
            ItemData item = goldfingerItems[i];
            InventoryManager.Instance.Add("Backpack", item, 1);
        }
    }
}
