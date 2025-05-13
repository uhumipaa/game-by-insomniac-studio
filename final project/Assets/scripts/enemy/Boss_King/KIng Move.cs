using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("目標 & 地圖範圍")]
    public Transform player;
    public Vector2 mapMinBounds;
    public Vector2 mapMaxBounds;

    [Header("追逐設定")]
    public float moveSpeed = 3f;
    public float stopDistance = 2f;

    [Header("瞬移設定")]
    public float teleportCooldown = 5f;
    public float teleportWarningTime = 1f;
    public GameObject teleportWarningPrefab;

    [Header("攻擊設定")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackDuration = 1f;

    [Header("攻擊動畫")]
    public Animator animator;
    public string attack1TriggerName = "Attack1";
    public string attack2TriggerName = "Attack2";
    public string attack3TriggerName = "Attack3";
    public string summonTriggerName = "Summon";

    [Header("攻擊Hitbox")]
    public Collider2D attackHitbox_1;
    public Collider2D attackHitbox_2;
    public Collider2D attackHitbox_3;
    public Player_Property player_Property;

    [Header("召喚騎士")]
    public GameObject knightPrefab;
    public Transform summonPoint;

    private float teleportTimer = 0f;
    private float attackTimer = 0f;
    private bool isTeleporting = false;
    private bool isAttacking = false;

    private Vector3 lastPosition;

    private enum AttackType { attack1, attack2, attack3, summon }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        teleportTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (!isTeleporting && teleportTimer >= teleportCooldown)
        {
            StartCoroutine(TeleportWithWarning());
            teleportTimer = 0f;
        }
        else if (!isTeleporting && !isAttacking)
        {
            ChasePlayer();

            // 靠近自動攻擊
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && attackTimer >= attackCooldown)
            {
                StartCoroutine(PerformAttack());
                attackTimer = 0f;
            }
        }

        FacePlayer();
        UpdateMovementAnimation();
    }

    void FacePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void ChasePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > stopDistance)
        {
            Vector3 targetPosition = player.position;
            targetPosition.x = Mathf.Clamp(targetPosition.x, mapMinBounds.x, mapMaxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mapMinBounds.y, mapMaxBounds.y);
            targetPosition.z = transform.position.z;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void UpdateMovementAnimation()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        float speed = velocity.magnitude;

        animator.SetFloat("Move", speed);

        lastPosition = transform.position;
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;  // ✅ 攻擊開始 → 停止移動

        AttackType selectedAttack = (AttackType)Random.Range(0, 4);

        if (animator != null)
        {
            switch (selectedAttack)
            {
                case AttackType.attack1:
                    animator.SetTrigger(attack1TriggerName);
                    break;
                case AttackType.attack2:
                    animator.SetTrigger(attack2TriggerName);
                    break;
                case AttackType.attack3:
                    animator.SetTrigger(attack3TriggerName);
                    break;
                case AttackType.summon:
                    animator.SetTrigger(summonTriggerName);
                    break;
            }
        }

        yield return new WaitForSeconds(0.8f);

        if (selectedAttack == AttackType.summon)
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(knightPrefab, summonPoint.position, Quaternion.identity);
        }
        else
        {
            EnableAttackHitbox(selectedAttack, true);
            yield return new WaitForSeconds(attackDuration);
            EnableAttackHitbox(selectedAttack, false);
        }

        isAttacking = false;  // ✅ 攻擊結束 → 恢復移動
    }

    IEnumerator TeleportWithWarning()
    {
        isTeleporting = true;

        Vector3 teleportTarget = player.position;
        teleportTarget.x = Mathf.Clamp(teleportTarget.x, mapMinBounds.x, mapMaxBounds.x);
        teleportTarget.y = Mathf.Clamp(teleportTarget.y, mapMinBounds.y, mapMaxBounds.y);
        teleportTarget.z = transform.position.z;

        GameObject warning = Instantiate(teleportWarningPrefab, teleportTarget, Quaternion.identity);

        yield return new WaitForSeconds(teleportWarningTime);

        Destroy(warning);

        transform.position = teleportTarget;

        // ✅ 瞬移完冷卻歸0
        attackTimer = attackCooldown;

        // ✅ 瞬移完後也要更新lastPosition，不然動畫會卡
        lastPosition = transform.position;

        // ✅ 瞬移完馬上攻擊
        StartCoroutine(PerformAttack());

        isTeleporting = false;
    }

    void EnableAttackHitbox(AttackType attackType, bool enable)
    {
        switch (attackType)
        {
            case AttackType.attack1:
                attackHitbox_1.enabled = enable;
                break;
            case AttackType.attack2:
                attackHitbox_2.enabled = enable;
                break;
            case AttackType.attack3:
                attackHitbox_3.enabled = enable;
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(player.position, attackRange);

            Gizmos.color = Color.green;
            Vector3 center = (mapMinBounds + mapMaxBounds) / 2;
            Vector3 size = new Vector3(
                mapMaxBounds.x - mapMinBounds.x,
                mapMaxBounds.y - mapMinBounds.y,
                0.1f
            );
            Gizmos.DrawWireCube(center, size);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                player_Property = collision.GetComponent<Player_Property>();
                player_Property.takedamage(player_Property.atk,transform.position);
            }
        }
}
