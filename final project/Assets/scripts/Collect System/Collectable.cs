using UnityEngine;

public class Collectable : MonoBehaviour
{
    /*player walks into collectable*/
    /*add collectable to player*/
    /*delete collectable from the screen*/

    public CollectableType type;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        player_trigger player = collision.GetComponent<player_trigger>();

        if (player)
        {
            player.inventory.Add(type);
            Destroy(this.gameObject); // 銷毀物件
        }
    }
}

public enum CollectableType
{
    NONE, POTATO_SEED
}
