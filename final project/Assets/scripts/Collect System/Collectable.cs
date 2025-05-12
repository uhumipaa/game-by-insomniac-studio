using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    /*player walks into collectable*/
    /*add collectable to player*/
    /*delete collectable from the screen*/
    public Rigidbody2D rb2D;
    public Item item;
    private void Awake()
    {
       rb2D = GetComponent<Rigidbody2D>(); 
       item = GetComponent<Item>();  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        player_trigger player = collision.GetComponent<player_trigger>();

        if (player)
        {
            //Debug.Log("【OnTriggerEnter2D】撞到物件：" + player.gameObject.name);

            var ui = FindFirstObjectByType<Inventory_UI>();
            if (ui == null)
            {
                Debug.LogError("⚠ 找不到 Inventory_UI");
            }
            else
            {
                Debug.Log("✅ 找到 Inventory_UI，開始刷新");
                Debug.Log("UI slot 數量：" + ui.slots.Count);  // ← 應該要是 28
            }

            if (item == null)
            {
                Debug.LogError("【錯誤】Collectable 上沒有 Item 元件！");
            }
            
            if (item != null)
            {
                if (player.inventory == null)
                {
                    Debug.LogError("【錯誤】player.inventory 還是 null！");
                }
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }    
        }
    }
}
