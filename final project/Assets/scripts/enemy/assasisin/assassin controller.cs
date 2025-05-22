using UnityEngine;

public class EnemyAI2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime;
    public GameObject attackHitbox;
    private Vector3 originalPosition;
    public float attackPushDistance = 1f;
    public Player_Property player_Property;
    public enemy_property enemy_Property;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        // 實時取得方向與距離
        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector2 moveDir = toPlayer.normalized;

        float verticalDifference = Mathf.Abs(player.position.y - transform.position.y);
        float attackHeightTolerance = 0.5f; // 垂直方向不能相差太多
        bool isInAttackRange = distance <= attackRange && verticalDifference <= attackHeightTolerance;
        bool isFacingEachOther = IsFacingTarget(player, transform) && IsFacingTarget(transform, player);

        Debug.Log($"距離: {distance}, Y差距: {verticalDifference}, 攻擊條件: {isInAttackRange}");

        // ✅ 正在攻擊時，完全鎖定不移動
        if (animator.GetBool("isAttacking"))
        {
            if (isInAttackRange)
            {
                if (!isFacingEachOther)
                {
                    rb.linearVelocity = Vector2.zero;
                    return;
                }
            }
        }

        // ✅ 玩家正面盯著敵人，且不在攻擊範圍內 → 停下
        if (isFacingEachOther)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            animator.Play("Idle", 0, 0);
        }
        // ✅ 攻擊
        else if (isInAttackRange)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", true);
            lastAttackTime = Time.time;
        }
        // ✅ 追蹤
        else
        {
            if (animator.GetBool("isAttacking"))
            {
                animator.Play("Move", 0, 0);
            }
            rb.linearVelocity = moveDir * moveSpeed;
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
        }

        // ✅ 面向修正
        if (moveDir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDir.x);
            transform.localScale = scale;
        }
    }

    // 工具函式：檢查 A 是否面對 B
    private bool IsFacingTarget(Transform source, Transform target)
    {
        float facingDir = Mathf.Sign(source.localScale.x != 0 ? source.localScale.x : 0.01f);
        float toTargetDir = Mathf.Sign(target.position.x - source.position.x);
        return facingDir == toTargetDir;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // 在 Attack 動畫的中段呼叫
    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
        {
            Collider2D col = attackHitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }
        // 紀錄原始位置
        originalPosition = transform.position;

        // 計算面朝方向（+1 是右，-1 是左）
        float facingDir = Mathf.Sign(transform.localScale.x);

        // 向前推進一格
        transform.position += new Vector3(facingDir * attackPushDistance, 0f, 0f);
    }

    // 在 Attack 動畫的結束幀呼叫
    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
        {
            Collider2D col = attackHitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_Property = collision.GetComponent<Player_Property>();
            player_Property.takedamage(enemy_Property.atk, transform.position);
        }
    }
}