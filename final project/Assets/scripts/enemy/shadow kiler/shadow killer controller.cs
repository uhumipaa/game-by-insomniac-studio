using UnityEngine;
using System.Collections;

public class SmartShieldEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float directionNoise = 0.5f;
    public float attackRangeX = 1.5f;
    public float minDistanceFromPlayer = 1.2f;
    public float attackCooldown = 2f;
    public float attackPushDistance = 1f;
    private Vector3 originalPosition;
    private Vector2 currentNoisyDirection;
    private float noiseUpdateTimer = 0f;
    public float noiseUpdateInterval = 0.5f; // 幾秒更新一次方向

    public GameObject shieldObject;
    public GameObject attack1Hitbox;
    public GameObject attack2Hitbox;
    public Player_Property player_Property;
    public enemy_property enemy_Property;
    public float flySpeedMultiplier = 2f;
    public string flyAnimationName = "Fly";
    private float moveTime = 0f;
    private bool isFlying = false;

    private bool isAttacking = false;
    private bool canAttack = true;
    public bool isShielded = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
            player_Property = playerGO.GetComponent<Player_Property>();
        }
        else
        {
            Debug.LogError("❌ 找不到 Player，請確認有設定 Tag 且場上有 Player！");
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (shieldObject != null) shieldObject.SetActive(true);
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        Vector2 toPlayer = player.position - transform.position;
        float horizontalDistance = Mathf.Abs(toPlayer.x);
        float totalDistance = toPlayer.magnitude;

        // ➤ 翻面（面向玩家）
        spriteRenderer.flipX = toPlayer.x < 0;

        // ➤ 如果太靠近就停止移動
        if (totalDistance < minDistanceFromPlayer)
        {
            animator.Play("Idle");
            return;
        }

        // ➤ 可以攻擊且靠近，且在玩家左右側
        if (canAttack && horizontalDistance < attackRangeX && Mathf.Abs(toPlayer.y) < 1f)
        {
            if (Mathf.Abs(toPlayer.x) > 0.2f) // 避免正前
            {
                StartCoroutine(WaitAndAttack());
                return;
            }
        }

        // ➤ 不規則移動
        moveTime += Time.deltaTime;
        if (moveTime >= 5f && !isFlying)
        {
            isFlying = true;
        }

        // ➤ 不規則移動
        MoveIrregularly(toPlayer);
    }

    void MoveIrregularly(Vector2 directionToPlayer)
    {
        noiseUpdateTimer -= Time.deltaTime;
        if (noiseUpdateTimer <= 0f)
        {
            Vector2 noisyDir = directionToPlayer.normalized + new Vector2(Random.Range(-directionNoise, directionNoise), Random.Range(-directionNoise, directionNoise));
            currentNoisyDirection = noisyDir.normalized;
            noiseUpdateTimer = noiseUpdateInterval;
        }

        float actualSpeed = isFlying ? moveSpeed * flySpeedMultiplier : moveSpeed;
        transform.Translate(currentNoisyDirection * actualSpeed * Time.deltaTime);

        // 播動畫
        if (isFlying)
            animator.Play(flyAnimationName);
        else
            animator.Play("Move");

        // ➤ 開啟防護罩
        if (!isShielded)
        {
            shieldObject?.SetActive(true);
            isShielded = true;
        }
    }
    IEnumerator WaitAndAttack()
    {
        isFlying = false;
        moveTime = 0f;
        isAttacking = true;
        canAttack = false;

        // ➤ 停頓 + 解除防護罩
        animator.Play("Idle");
        if (isShielded)
        {
            shieldObject?.SetActive(false);
            isShielded = false;
        }

        yield return new WaitForSeconds(0.5f); // 攻擊前停頓

        // ➤ 攻擊動畫
        animator.Play("Attack");

        yield return new WaitForSeconds(1.5f); // 假設動畫時間

        animator.Play("Idle");

        yield return new WaitForSeconds(0.5f); // 攻擊後停頓

        // ➤ 恢復防護罩
        if (!isShielded)
        {
            shieldObject?.SetActive(true);
            isShielded = true;
        }

        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // 在 Attack 動畫的中段呼叫
    public void EnableAttack1Hitbox()
    {
        if (attack1Hitbox != null)
        {
            Collider2D col = attack1Hitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }
        // 紀錄原始位置
        originalPosition = transform.position;

        // 計算面朝方向（+1 是右，-1 是左）
        float facingDir = spriteRenderer.flipX ? -1f : 1f;

        // 向前推進一格
        transform.position += new Vector3(facingDir * attackPushDistance, 0f, 0f);
    }

    // 在 Attack 動畫的結束幀呼叫
    public void DisableAttack1Hitbox()
    {
        if (attack1Hitbox != null)
        {
            Collider2D col = attack1Hitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
    public void EnableAttack2Hitbox()
    {
        if (attack2Hitbox != null)
        {
            Collider2D col = attack2Hitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }

    }

    // 在 Attack 動畫的結束幀呼叫
    public void DisableAttack2Hitbox()
    {
        if (attack2Hitbox != null)
        {
            Collider2D col = attack2Hitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }

    public void turnback()
    {
        transform.position = originalPosition;
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
