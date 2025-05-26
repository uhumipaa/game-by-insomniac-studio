using UnityEngine;

public class WarriorHitbox : MonoBehaviour
{
    private Collider2D hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 傷害處理（呼叫 player_property）
            var property = collision.GetComponent<Player_Property>();
            if (property != null)
            {
                property.takedamage(10, transform.position); // 正確：符合方法參數與命名

            }

            // 擊退處理（呼叫 Knockback）
            var knock = collision.GetComponent<Knockback>();
            if (knock != null)
            {
                knock.ApplyKnockback(transform.position); // 傳入 Boss 的位置
            }

            Debug.Log("Boss_warrior 命中玩家！");
        }
    }

    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }
}
