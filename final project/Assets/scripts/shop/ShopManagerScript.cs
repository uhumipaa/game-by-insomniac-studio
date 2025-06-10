using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    //add by bai
    public List<ShopItemData> shopItemList;
    public Dictionary<int, ShopItemData> itemDataByID = new Dictionary<int, ShopItemData>();
    //end 0517
    //public int[,] shopItems = new int[5, 5];//the number of items
    public Text CoinsTXT;
    void Awake()
    {
        //Debug.Log($"[ShopManagerScript] Awake 被呼叫，我是 {gameObject.name}");
    }
    void Start()
    {
        //設定UI TEXT
        CoinManager.instance.RegisterCoinText(CoinsTXT);

        InitShopData();
    }

    public void InitShopData()
    {
        itemDataByID.Clear();
        foreach (var shopItem in shopItemList)
        {
            if (!itemDataByID.ContainsKey(shopItem.itemID))
            {
                itemDataByID.Add(shopItem.itemID, shopItem);
            }
        }
    }
    public void Buy()
    {

        if (itemDataByID == null || itemDataByID.Count == 0)
        {
            Debug.LogWarning("商店資料尚未初始化，嘗試重新初始化");
            InitShopData();
        }

        GameObject ButtonRef = EventSystem.current.currentSelectedGameObject;

        int itemID = ButtonRef.GetComponent<ButtonInfo>().ItemID;


        if (itemDataByID.TryGetValue(itemID, out ShopItemData itemData))
        {
            if (CoinManager.instance.SpendCoins(itemData.price))
            {
                itemData.quantity++;
                ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = itemData.quantity.ToString();

                //把買到的商品加入backpack
                InventoryManager.Instance.Add("Backpack", itemData.itemData, 1);
            }
        }
        else
        {
          
        }
    }
}
