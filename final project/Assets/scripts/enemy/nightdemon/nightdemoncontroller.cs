using UnityEngine;

public class nightdemoncontroller : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public Vector2 leftBoundary;
    public Vector2 rightBoundary;

    public GameObject attackHitbox;
    public GameObject DeathHitbox;
    public GameObject healthBarUI;

    private Vector2 moveDirection;
    private Transform player;
    private Rigidbody2D rb;
    public Animator animator;
    public Transform playerDetectionPoint;
    public Transform selfDetectionPoint;
    public Player_Property player_Property;
    public enemy_property enemy_Property;

    private enum State { Patrol, Charging, Attacking }
    private State currentState = State.Patrol;
    private float directionChangeInterval = 1f;
    private float directionChangeTimer;
    private bool isdead = false;
    public bool isattack = false;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            // 如果你有指定 playerDetectionPoint，就用它；否則 fallback 用 player 本身
            if (playerDetectionPoint == null)
            {
                playerDetectionPoint = player; // 預防沒設就用本體
            }
        }
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player_Property = playerGO.GetComponent<Player_Property>();
        }
        else
        {
            Debug.LogError("❌ 找不到 Player，請確認有設定 Tag 且場上有 Player！");
        }

        PickRandomDirection();
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
        if (!isattack)
        {
            FaceDirection();
        }
        if (isdead) return;
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Charging:
                // 蓄力動畫由動畫事件控制，不做任何事
                rb.linearVelocity = Vector2.zero;
                break;
            case State.Attacking:
                // 攻擊時停下，邏輯由動畫事件控制
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    void LateUpdate()
    {
        Vector3 clampedPos = transform.position;

        clampedPos.x = Mathf.Clamp(clampedPos.x, leftBoundary.x, rightBoundary.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, leftBoundary.y, rightBoundary.y);

        transform.position = clampedPos; // ✅ 直接修正主體位置

        if (selfDetectionPoint != null)
            selfDetectionPoint.position = clampedPos; // 同步自訂偵測點位置
    }

    void Patrol()
    {
        animator.Play("Run");
        rb.linearVelocity = moveDirection * moveSpeed;

        // 隨機方向倒數
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            PickRandomDirection();
            directionChangeTimer = directionChangeInterval + Random.Range(-1f, 1f);
        }

        // 邊界反向處理
        if (selfDetectionPoint.position.x < leftBoundary.x && moveDirection.x < 0)
        {
            moveDirection.x *= -1;
            directionChangeTimer = directionChangeInterval;
        }
        else if (selfDetectionPoint.position.x > rightBoundary.x && moveDirection.x > 0)
        {
            moveDirection.x *= -1;
            directionChangeTimer = directionChangeInterval;
        }

        if (selfDetectionPoint.position.y < leftBoundary.y && moveDirection.y < 0)
        {
            moveDirection.y *= -1;
            directionChangeTimer = directionChangeInterval;
        }
        else if (selfDetectionPoint.position.y > rightBoundary.y && moveDirection.y > 0)
        {
            moveDirection.y *= -1;
            directionChangeTimer = directionChangeInterval;
        }

        // 玩家進入範圍
        float distanceToPlayer = Vector2.Distance(selfDetectionPoint.position, playerDetectionPoint.position);
        if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Charging;
            rb.linearVelocity = Vector2.zero;
            animator.Play("Charge");

            FaceDirectionToPlayer();
        }
    }

    void PickRandomDirection()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void FaceDirection()
    {
        Vector3 scale = selfDetectionPoint.localScale;
        if (moveDirection.x >= 0)
        {
            scale.x = Mathf.Abs(scale.x);
            selfDetectionPoint.localScale = scale;
        }
        else if (moveDirection.x < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            selfDetectionPoint.localScale = scale;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 center = (leftBoundary + rightBoundary) / 2f;
        Vector2 size = new Vector2(Mathf.Abs(rightBoundary.x - leftBoundary.x), Mathf.Abs(rightBoundary.y - leftBoundary.y));
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.green;
        Vector2 c = (leftBoundary + rightBoundary) / 2f;
        Vector2 s = new Vector2(Mathf.Abs(rightBoundary.x - leftBoundary.x), Mathf.Abs(rightBoundary.y - leftBoundary.y));
        Gizmos.DrawWireCube(c, s);

        // 顯示偵測距離
        if (Application.isPlaying && playerDetectionPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(selfDetectionPoint.position, detectionRange);
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

    public void nightdemondie()
    {
        isdead = true;
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
        rb.linearVelocity = Vector2.zero;
        animator.Play("Death");

    }
    void FaceDirectionToPlayer()
    {
        bool shouldFaceRight = playerDetectionPoint.position.x >= selfDetectionPoint.position.x;
        Vector3 scale = selfDetectionPoint.localScale;
        if (!shouldFaceRight)
        {
            scale.x = Mathf.Abs(scale.x);
            selfDetectionPoint.localScale = scale;
        }
        else if (shouldFaceRight)
        {
            scale.x = -Mathf.Abs(scale.x);
            selfDetectionPoint.localScale = scale;
        }
    }

    // ===== 🎬 動畫事件函數區塊 =====

    // 在 Charge 動畫的最後一幀呼叫
    public void OnChargeFinished()
    {
        currentState = State.Attacking;
        animator.Play("Attack");
        isattack = true;
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
    public void FinalAttack()
    {
        isattack = false;
        currentState = State.Patrol;
        directionChangeTimer = directionChangeInterval;
        PickRandomDirection();
    }
    public void EnableDeathHitbox()
    {
        if (DeathHitbox != null)
        {
            Collider2D col = DeathHitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
            Vector3 scale = selfDetectionPoint.localScale;
            scale.x = 7;
            scale.y = 7;
            selfDetectionPoint.localScale = scale;
        }
    }
    public void DisableDeathHitbox()
    {
        if (DeathHitbox != null)
        {
            Collider2D col = DeathHitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
    public void Destroynightdemon()
    {
        Vector3 scale = selfDetectionPoint.localScale;
        scale.x = 4;
        scale.y = 4;
        selfDetectionPoint.localScale = scale;
        Collider2D col = DeathHitbox.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
        EnemyManager.instance.removeenemy(gameObject);
        gameObject.SetActive(false);
        
    }
}