using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{

    public int ItemID;
    public Text PriceTxt;
    public Text QuantityTxt;
    //public GameObject ShopManager;
    private ShopManagerScript shopManager;

    //0517 mod by bai
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);  // 確保 ShopManagerScript 已經執行完 Start()
        if (shopManager != null)
        {
            shopManager = FindFirstObjectByType<ShopManagerScript>();
            Debug.Log($"ButtonInfo 嘗試取得 ItemID: {ItemID}，ShopManager 是否存在: {shopManager != null}");
            if (shopManager != null)
            {
                Debug.Log($"ShopManager itemDataByID Count: {shopManager.itemDataByID.Count}");
            }

            UpdateButtonUI();
        }
    }
    public void UpdateButtonUI()
    {
        Debug.Log($"[ButtonInfo] 目前綁的 ShopManager 是 {shopManager.name}");
        
        if (shopManager != null)
        {
            if (shopManager != null && shopManager.itemDataByID.TryGetValue(ItemID, out ShopItemData itemData))
            {
                PriceTxt.text = "Price: $" + itemData.price;
                QuantityTxt.text = " " + itemData.quantity;
            }
            else
            {
                PriceTxt.text = "N/A";
                QuantityTxt.text = "N/A";
            }
        }
    }
}
