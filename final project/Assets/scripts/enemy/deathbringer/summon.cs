using UnityEngine;

public class SummonHitboxController : MonoBehaviour
{
    public Collider2D hitbox;
    public Player_Property player;
    public enemy_property enemy;
    void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<enemy_property>();
            Debug.LogError("❗ Enemy is null on hitbox! Check if it was assigned.");
        }
        if (player == null)
        {
            player = GetComponentInParent<Player_Property>();
            Debug.LogError("❗ player is null on hitbox! Check if it was assigned.");
        }

        // Player 不一定需要預先設定，OnTriggerEnter2D 時再抓即可
        hitbox.enabled = false;
    }
    public void Initialize(Player_Property playerRef, enemy_property enemyRef)
    {
        player = playerRef;
        enemy = enemyRef;
    }
    public void EnableHitbox()
    {
        if (hitbox != null)
            hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitbox != null)
            hitbox.enabled = false;
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player_Property>();
            player.takedamage(enemy.atk, transform.position);
            Debug.Log("ffff");
        }
    }
}