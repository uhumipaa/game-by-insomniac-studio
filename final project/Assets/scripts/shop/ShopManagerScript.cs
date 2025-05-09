using System. Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[5,5];//the number of items
    public Text CoinsTXT;
    void Start()
    {
        //設定UI TEXT
        CoinManager.instance.RegisterCoinText(CoinsTXT);

        //set items' id
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //set items' price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        //set items' quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    public void Buy(){

        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        //0510 add by bai
        int itemID = ButtonRef.GetComponent<ButtonInfo>().ItemID;
        int price = shopItems[2, itemID];
        //end

        //0510 mod by bai
        if (CoinManager.instance.SpendCoins(price)){
            shopItems[3, itemID]++;
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, itemID].ToString();
        }
        //end
    }
}
