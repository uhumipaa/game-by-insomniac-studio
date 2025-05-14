using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public enum BossState { Phase1, Phase2 }
    public BossState currentState = BossState.Phase1;

    [Header("目標 & 地圖範圍")]
    public Transform player;
    public Vector2 mapMinBounds;
    public Vector2 mapMaxBounds;

    [Header("數值設定")]
    public float moveSpeedPhase1 = 3f;
    public float moveSpeedPhase2 = 5f;
    public float attackCooldownPhase1 = 2f;
    public float attackCooldownPhase2 = 1f;

    [Header("攻擊設定")]
    public float stopDistance = 2f;
    public float attackRange = 2f;
    public float attackDuration = 1f;

    [Header("瞬移設定 (只在Phase2生效)")]
    public float teleportCooldown = 5f;
    public float teleportWarningTime = 1f;
    public GameObject teleportWarningPrefab;

    [Header("攻擊動畫")]
    public Animator animator;
    public string attack1TriggerName = "Attack1";
    public string attack2TriggerName = "Attack2";
    public string attack3TriggerName = "Attack3";
    public string summonTriggerName = "Summon";
    public string moveAnimPhase2Bool = "Phase2Move";

    [Header("攻擊Hitbox")]
    public Collider2D attackHitbox_1;
    public Collider2D attackHitbox_2;
    public Collider2D attackHitbox_3;

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

    // 找到自身的Health腳本
    enemy_property health = GetComponent<enemy_property>();

    // 訂閱死亡事件 (進入Phase2)
    if (health != null)
    {
        health.Boss_King_Death += EnterPhase2;
    }
}

    void Update()
    {
        if (player == null) return;

        teleportTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (currentState == BossState.Phase2 && !isTeleporting && teleportTimer >= teleportCooldown)
        {
            StartCoroutine(TeleportWithWarning());
            teleportTimer = 0f;
        }
        else if (!isTeleporting && !isAttacking)
        {
            ChasePlayer();

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float currentAttackCooldown = currentState == BossState.Phase1 ? attackCooldownPhase1 : attackCooldownPhase2;

            if (distanceToPlayer <= attackRange && attackTimer >= currentAttackCooldown)
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

            float currentMoveSpeed = currentState == BossState.Phase1 ? moveSpeedPhase1 : moveSpeedPhase2;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);
        }
    }

    void UpdateMovementAnimation()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        float speed = velocity.magnitude;

        animator.SetFloat("Move", speed);

        // 第二型態移動動畫 (假設你有特殊動畫)
        animator.SetBool(moveAnimPhase2Bool, currentState == BossState.Phase2);

        lastPosition = transform.position;
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;

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
            yield return new WaitForSeconds(0.8f);
            int summonCount = currentState == BossState.Phase1 ? 1 : 2;

            for (int i = 0; i < summonCount; i++)
            {
                Instantiate(knightPrefab, summonPoint.position, Quaternion.identity);
            }
        }
        else
        {
            EnableAttackHitbox(selectedAttack, true);
            yield return new WaitForSeconds(attackDuration);
            EnableAttackHitbox(selectedAttack, false);
        }

        isAttacking = false;
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

        attackTimer = currentState == BossState.Phase1 ? attackCooldownPhase1 : attackCooldownPhase2;

        lastPosition = transform.position;

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

    // ✅ 呼叫這個讓Boss進入第二型態
    public void EnterPhase2()
    {
        if (currentState == BossState.Phase2) return;

        currentState = BossState.Phase2;

        Debug.Log("Boss進入第二型態！");
        
        animator.SetBool("Phase2Move", true);
        // 你可以在這邊觸發變身特效、音效等
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
}
