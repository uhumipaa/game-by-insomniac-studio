using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInfo : MonoBehaviour
{

    public int ItemID;
    public Text PirceTxt;
    public Text QuantityTxt;
    public GameObject ShopManager;
    // Update is called once per frame
    void Update()
    {
        PirceTxt.text = "Price: $" + ShopManager.GetComponent<ShopManagerScript>().shopItems[2, ItemID].ToString();
        QuantityTxt.text = " " +  ShopManager.GetComponent<ShopManagerScript>().shopItems[3, ItemID].ToString();
    }

    public void BackToFarm()
    { 
        SceneManager.LoadSceneAsync("farm");        
    }
}
