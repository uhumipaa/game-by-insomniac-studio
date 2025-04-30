using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    /*player walks into collectable*/
    /*add collectable to player*/
    /*delete collectable from the screen*/
    private void OnTriggerEnter2D(Collider2D collision)
    {

        player_trigger player = collision.GetComponent<player_trigger>();

        if (player)
        {
            Debug.Log("【OnTriggerEnter2D】撞到物件：" + player.gameObject.name);

            Item item = GetComponent<Item>();

            
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
                player.inventory.Add(item);
                Destroy(this.gameObject);
            }    
        }
    }
}
