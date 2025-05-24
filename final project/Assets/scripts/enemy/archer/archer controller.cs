using UnityEngine;
using System.Collections;

public class ArcherEnemy : MonoBehaviour
{
    public float moveDistance = 1f;
    public float attackCooldown = 2f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public Animator animator;
    public Transform player;

    private float lastAttackTime;
    private SpriteRenderer spriteRenderer;

    // ✅ 新增：儲存攻擊方向給 Animator Event 使用
    private Vector2 lastAttackDirection;
    public float nextAttackTime = 0f;
    private bool canAttack = true;
    public bool isAttacking = false;
    public Player_Property player_Property;
    public enemy_property enemy_Property;

    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player_Property = playerGO.GetComponent<Player_Property>();
        }
        else
        {
            Debug.LogError("❌ 找不到 Player，請確認有設定 Tag 且場上有 Player！");
        }
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null || !canAttack) return;

        Vector2 toPlayer = player.position - transform.position;
        // if (toPlayer.magnitude < 0.1f)
        // {
        //     PlayIdle();
        //     return;
        // }

        Vector2 direction = GetEightDirection(toPlayer);

        if (CanAttackDirection(toPlayer))
        {
            Vector2 snappedDir = SnapToEightDirection(toPlayer);
            Attack(snappedDir);
            isAttacking = true;
        }
        // else if (!isAttacking)
        // {
        //     PlayIdle();
        // }
    }

    // ---------------------------- 攻擊邏輯 ----------------------------

    void Attack(Vector2 dir)
    {
        canAttack = false;

        DirectionInfo info = GetDirectionInfo(dir);
        spriteRenderer.flipX = info.flipX;

        animator.SetInteger("AttackType", info.attackIndex);
        animator.SetTrigger("Attack");

        lastAttackDirection = dir;
    }

    // ✅ 新增：動畫事件呼叫
    public void FireArrow()
    {
        if (arrowPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Arrow Prefab 或 FirePoint 未設定！");
            return;
        }

        GameObject arrowGO = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowGO.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.SetDirection(lastAttackDirection);

            enemy_property myProperty = GetComponent<enemy_property>();
            if (myProperty != null)
            {
                arrow.SetOwner(myProperty);
            }
        }
        else
        {
            Debug.LogWarning("箭矢 prefab 沒有掛 Arrow 腳本！");
        }
    }

    // ✅ 新增：動畫事件呼叫（延遲移動）
    public void MoveAfterAttack()
    {
        StartCoroutine(MoveSmoothAndReset(lastAttackDirection));
    }

    // ---------------------------- 移動邏輯 ----------------------------

    IEnumerator MoveSmoothAndReset(Vector2 dir)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + (Vector3)(dir.normalized * moveDistance);
        float duration = 0.3f;
        float elapsed = 0f;

        animator.SetTrigger("Move");

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        PlayIdle();
        isAttacking = false;
        // ✅ 等待冷卻時間後開啟攻擊
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    // ---------------------------- 動畫控制 ----------------------------

    void PlayIdle()
    {
        animator.SetTrigger("Idle");
    }

    // ---------------------------- 方向處理 ----------------------------

    struct DirectionInfo
    {
        public int attackIndex;
        public bool flipX;

        public DirectionInfo(int idx, bool flip)
        {
            attackIndex = idx;
            flipX = flip;
        }
    }

    DirectionInfo GetDirectionInfo(Vector2 dir)
    {
        dir = GetEightDirection(dir);

        if (dir == Vector2.up) return new DirectionInfo(0, false);   // N
        if (dir == new Vector2(1, 1).normalized) return new DirectionInfo(1, false);  // NE
        if (dir == Vector2.right) return new DirectionInfo(2, false); // E
        if (dir == new Vector2(1, -1).normalized) return new DirectionInfo(3, false); // SE
        if (dir == Vector2.down) return new DirectionInfo(4, false);  // S

        if (dir == new Vector2(-1, -1).normalized) return new DirectionInfo(3, true);  // SW => SE + Flip
        if (dir == Vector2.left) return new DirectionInfo(2, true);    // W => E + Flip
        if (dir == new Vector2(-1, 1).normalized) return new DirectionInfo(1, true);  // NW => NE + Flip

        return new DirectionInfo(4, false); // default S
    }

    Vector2 GetEightDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45f) * 45f;
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }

    bool CanAttackDirection(Vector2 toPlayer)
    {
        float angleThreshold = 10f; // 可調整：允許的角度誤差（越小越嚴格）

        foreach (var dir in allowedDirections)
        {
            float angle = Vector2.Angle(toPlayer.normalized, dir);
            if (angle <= angleThreshold)
            {
                return true;
            }
        }

        return false;
    }

    private readonly Vector2[] allowedDirections = new Vector2[]
    {
        Vector2.up,                          // N
        new Vector2(1, 1).normalized,        // NE
        Vector2.right,                       // E
        new Vector2(1, -1).normalized,       // SE
        Vector2.down,                        // S
        new Vector2(-1, -1).normalized,      // SW
        Vector2.left,                        // W
        new Vector2(-1, 1).normalized        // NW
    };

    Vector2 SnapToEightDirection(Vector2 input)
    {
        float minAngle = 999f;
        Vector2 closest = Vector2.zero;

        foreach (var dir in allowedDirections)
        {
            float angle = Vector2.Angle(input.normalized, dir);
            if (angle < minAngle)
            {
                minAngle = angle;
                closest = dir;
            }
        }

        return closest;
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
