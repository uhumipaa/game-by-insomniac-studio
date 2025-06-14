using UnityEngine;

public class KingHitbox : MonoBehaviour
{
    public Collider2D col;
    public Player_Property player;
    public enemy_property enemy;
    void Start()
    {
        col.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                player = collision.GetComponent<Player_Property>();
                player.takedamage(enemy.atk,transform.position);
            }
        }
}
