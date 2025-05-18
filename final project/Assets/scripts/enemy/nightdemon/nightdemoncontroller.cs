using UnityEngine;

public class nightdemoncontroller : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public Vector2 leftBoundary;
    public Vector2 rightBoundary;

    public GameObject attackHitbox;

    private Vector2 moveDirection;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    public Transform playerDetectionPoint;
    public Transform selfDetectionPoint;
    public Player_Property player_Property;
    public enemy_property enemy_Property;

    private enum State { Patrol, Charging, Attacking }
    private State currentState = State.Patrol;

    private bool isFacingRight = true;
    private float directionChangeInterval = 1f;
    private float directionChangeTimer;

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

        PickRandomDirection();
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
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
        FaceDirection();
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, leftBoundary.x, rightBoundary.x);
        pos.y = Mathf.Clamp(pos.y, leftBoundary.y, rightBoundary.y);

        selfDetectionPoint.position = pos;
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

            // 面向玩家
            if (playerDetectionPoint.position.x < selfDetectionPoint.position.x)
                isFacingRight = false;
            else
                isFacingRight = true;
        }
    }

    void PickRandomDirection()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void FaceDirection()
    {
        if (playerDetectionPoint.position.x > selfDetectionPoint.position.x && !isFacingRight)
            Flip();
        else if (playerDetectionPoint.position.x < selfDetectionPoint.position.x && isFacingRight)
            Flip();

    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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

    // ===== 🎬 動畫事件函數區塊 =====

    // 在 Charge 動畫的最後一幀呼叫
    public void OnChargeFinished()
    {
        // FaceDirection();
        currentState = State.Attacking;
        animator.Play("Attack");
        // FaceDirection();
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

        currentState = State.Patrol;
        directionChangeTimer = directionChangeInterval;
        PickRandomDirection();
    }
}