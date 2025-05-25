using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 5f;                     // 飛行速度
    public float lifetime = 7f;                  // 最長存在時間，避免卡住不刪除
    public int damage = 1;                       // 傷害數值

    public string playerTag = "Player";          // 玩家標籤
    public LayerMask wallLayer;                  // 牆壁的圖層

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 如果有 Rigidbody2D，用 velocity 控制直線移動（假設面朝右）
        rb.linearVelocity = transform.right * speed;

        // 保險起見：一段時間後自動銷毀
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 撞到玩家
        if (collision.CompareTag(playerTag))
        {
            // 如果有 Player_Property 類別，就執行傷害
            Player_Property player = collision.GetComponent<Player_Property>();
            if (player != null)
            {
                player.takedamage(damage, transform.position);
            }

            Destroy(gameObject); // 刪除魔法
        }

        // 撞到牆壁（牆要設定在特定 Layer，例如 Wall）
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
