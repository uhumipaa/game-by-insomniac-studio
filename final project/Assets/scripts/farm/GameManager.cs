using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public player_trigger player;
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
        uiManager = GetComponent<UI_Manager>();
        
    }

}
