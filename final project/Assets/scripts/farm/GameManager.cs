using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public TileManager tileManager;
    public Text CoinsTXT;

    void Start()
    {
        //設定UI TEXT
        CoinManager.instance.RegisterCoinText(CoinsTXT);
    }
    private void Awake()
    {
        if(instance != null && instance != this) //如果有其他GameManager存在
        { 
            Destroy(this.gameObject); //把現在這個GameManager刪掉
        }
        else{
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject); //切換場景時 不會刪掉現在這個物件

        itemManager = GetComponent<ItemManager>(); 
        tileManager = GetComponent<TileManager>();

    }

}
