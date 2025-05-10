using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{

    public int ItemID;
    public Text PirceTxt;
    public Text QuantityTxt;
    public GameObject ShopManager;
    
    void Update()
    {
        if (ShopManager != null)
    {
        var shop = ShopManager.GetComponent<ShopManagerScript>();
        if (shop != null)
        {
            PirceTxt.text = "Price: $" + shop.shopItems[2, ItemID].ToString();
            QuantityTxt.text = " " + shop.shopItems[3, ItemID].ToString();
        }
    }
    }
}
