using UnityEngine;
using System.Collections;

public class enemy_close_move : MonoBehaviour,IEnemyMoveBehavior
{
    private Transform player;
    public float turnSpeed = 5f;
    public float stuned_time=1f;
    private int offsetSign;//偏向方向
    public float offsetStrength = 0.5f;//偏向力度
    public float jitterStrength = 0.1f;//搖晃力度
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsetSign = Random.value > 0.5f ? 1 : -1;
    }

    // Update is called once per frame
    public void Move(Transform self, Transform player, Rigidbody2D rb, float speed)
    {
        // 1. 基本方向（朝玩家）
        Vector2 direction = (player.position - self.position).normalized;

        // 2. 包夾偏移方向（垂直於 direction）
        Vector2 sideOffset = Vector2.Perpendicular(direction) * offsetSign; // offsetSign 是 -1 或 1

        // 3. 隨機抖動（不規則感）
        Vector2 jitter = Random.insideUnitCircle * jitterStrength;

        // 4. 最終方向加總
        Vector2 desiredDirection = (direction + sideOffset * offsetStrength + jitter).normalized;

        // 5. 平滑轉向
        Vector2 currentDirection = rb.linearVelocity.magnitude > 0.1f ? rb.linearVelocity.normalized : direction;
        Vector2 newDirection = Vector2.Lerp(currentDirection, desiredDirection, turnSpeed * Time.fixedDeltaTime);
        rb.linearVelocity = speed * newDirection;
    }
}
