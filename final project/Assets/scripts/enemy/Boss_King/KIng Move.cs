using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("瞬移設定 (Phase2)")]
    public float teleportCooldown = 5f;
    public float teleportWarningTime = 1f;
    public GameObject teleportWarningPrefab;

    [Header("動畫")]
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

    [Header("變身特效")]
    public GameObject phase2EffectPrefab;
    public float phase2TransformTime = 2f;

    private float teleportTimer = 0f;
    private float attackTimer = 0f;
    private bool isTeleporting = false;
    private bool isAttacking = false;
    private bool isTransforming = false;

    private Vector3 lastPosition;

    private enum AttackType { attack1, attack2, attack3, summon }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPosition = transform.position;

        var health = GetComponent<enemy_property>();
        if (health != null)
        {
            health.Boss_King_Death += StartPhase2Transform; // 改成呼叫變身協程
        }
    }

    void Update()
    {
        if (player == null || isTransforming) return; // 變身時完全不動作

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
        Vector3 newScale = transform.localScale;

        // 根據玩家左右位置，只翻轉X軸方向，不改變大小
        if (toPlayer.x > 0)
        {
            newScale.x = Mathf.Abs(newScale.x);
        }
        else
        {
            newScale.x = -Mathf.Abs(newScale.x);
        }

        transform.localScale = newScale;
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
        if (isTransforming)
        {
            // 變身中 → 強制停住不撥移動動畫
            animator.SetFloat("Move", 0f);
            return;
        }
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        float speed = velocity.magnitude;

        animator.SetFloat("Move", speed);
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
                case AttackType.attack1: animator.SetTrigger(attack1TriggerName); break;
                case AttackType.attack2: animator.SetTrigger(attack2TriggerName); break;
                case AttackType.attack3: animator.SetTrigger(attack3TriggerName); break;
                case AttackType.summon: animator.SetTrigger(summonTriggerName); break;
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
            case AttackType.attack1: attackHitbox_1.enabled = enable; break;
            case AttackType.attack2: attackHitbox_2.enabled = enable; break;
            case AttackType.attack3: attackHitbox_3.enabled = enable; break;
        }
    }

    public void StartPhase2Transform()
    {
        if (!isTransforming)
            StartCoroutine(Phase2TransformRoutine());
    }

    IEnumerator Phase2TransformRoutine()
    {
        isTransforming = true;

        Debug.Log("Boss 開始變身第二型態...");

        isTeleporting = true;
        isAttacking = true;

        var hp = GetComponent<enemy_property>();
        var nanoMachine = GetComponent<NanoMachine_Son>();

        if (nanoMachine != null)
            nanoMachine.StartInvincibility();

        int startHealth = hp.current_health;
        int targetHealth = hp.max_health * 2;

        float elapsed = 0f;
        while (elapsed < phase2TransformTime)
        {
            UpdateMovementAnimation();
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / phase2TransformTime);

            hp.current_health = Mathf.RoundToInt(Mathf.Lerp(startHealth, targetHealth, t));
            hp.healthChanged.Invoke();
            yield return null;
        }
        // yield return new WaitForSeconds(0.5f);

        if (phase2EffectPrefab != null)
            Instantiate(phase2EffectPrefab, transform.position, Quaternion.identity);
            
        yield return new WaitForSeconds(0.5f);

        hp.current_health = targetHealth;
        hp.max_health = targetHealth;
        hp.atk = Mathf.RoundToInt(hp.atk * 1.5f);
        hp.def = Mathf.RoundToInt(hp.def * 1.2f);

        EnterPhase2();

        Debug.Log("Boss 完成變身！");

        isTeleporting = false;
        isAttacking = false;
        isTransforming = false;
    }

    public bool IsTransforming()
    {
        return isTransforming;
    }

    public void EnterPhase2()
    {
        if (currentState == BossState.Phase2) return;

        currentState = BossState.Phase2;

        animator.SetBool(moveAnimPhase2Bool, true);
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