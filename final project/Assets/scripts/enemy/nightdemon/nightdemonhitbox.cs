using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Collider2D col;
    public Player_Property player;
    public enemy_property enemy;
    void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<enemy_property>();
        }
        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                player = playerGO.GetComponent<Player_Property>();;
            }
        }

        // Player 不一定需要預先設定，OnTriggerEnter2D 時再抓即可
        col.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player_Property>();
            if (enemy == null)
                enemy = GetComponentInParent<enemy_property>();
            player.takedamage(enemy.atk, transform.position);
        }
    }
}

