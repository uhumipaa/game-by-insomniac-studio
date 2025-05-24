using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //public static GameManager instance;

    [Header("玩家資料")]
    //public InventoryManager inventoryManager;
    //public CoinManager coin;

    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public player_trigger player;

    private void Awake()
    {
        /*if (instance != null && instance != this) //如果有其他GameManager存在
        {
            Destroy(this.gameObject); //把現在這個GameManager刪掉
            return;
        }*/

        //instance = this;
        //DontDestroyOnLoad(gameObject);

        // 初始化不隨場景消失的資料
        //inventoryManager = new InventoryManager();
        Debug.Log($"🟢 GameManager 啟動，場景名為：{gameObject.scene.name}");

        if (transform.root.name == "DontDestroyOnLoad")
        {
            Debug.LogWarning("這個 GameManager 是殘留的，會自動銷毀");
            Destroy(gameObject);
            return;
        }

        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();

    }
}
