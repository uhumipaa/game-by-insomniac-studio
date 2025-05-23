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

        /*//set items' id
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
        shopItems[3, 4] = 0;*/

        //Debug.Log($"[ShopManagerScript] 我是 {gameObject.name}，Shop Item List 有 {shopItemList.Count} 個");
        InitShopData();
    }

    public void InitShopData()
    {
        //Debug.Log("[InitShop] 進入");
        itemDataByID.Clear();
        foreach (var shopItem in shopItemList)
        {
            if (!itemDataByID.ContainsKey(shopItem.itemID))
            {
                itemDataByID.Add(shopItem.itemID, shopItem);
                //Debug.Log($"[ShopManager] 加入商品 {shopItem.itemID} {shopItem.itemData.itemName} $ {shopItem.price}");
            }
        }
    }
    public void Buy()
    {

        //Debug.Log("按鈕被點擊了");


        if (itemDataByID == null || itemDataByID.Count == 0)
        {
            Debug.LogWarning("商店資料尚未初始化，嘗試重新初始化");
            InitShopData();
        }

        GameObject ButtonRef = EventSystem.current.currentSelectedGameObject;

        //0510 mod by bai
        int itemID = ButtonRef.GetComponent<ButtonInfo>().ItemID;
        //int price = shopItems[2, itemID];
        //end

        //0510 mod by bai
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
            //Debug.LogError($"ItemID {itemID} 不存在於商店資料中");
        }
        //end

    }
}
