using UnityEngine;

public class Collectable : MonoBehaviour
{
    /*player walks into collectable*/
    /*add collectable to player*/
    /*delete collectable from the screen*/
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        player_trigger player = collision.GetComponent<player_trigger>();

        if (player != null)
        {
        Debug.Log("成功撿取！");
        player.numPotatoSeed++;
        Debug.Log("Destroying: " + this.gameObject.name);
        Destroy(this.gameObject); // 銷毀物件
        }
        else
        {
        Debug.LogWarning("這個物件沒有掛 player_trigger 腳本！");
        }
    }


}
