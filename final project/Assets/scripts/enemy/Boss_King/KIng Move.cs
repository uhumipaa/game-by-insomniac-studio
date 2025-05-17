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
    public Player_Property Player_Property;
    public enemy_property enemy;

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
    public string deathTriggerName = "Death";   // 死亡動畫Trigger
    public float deathAnimationDuration = 2f;   // 死亡動畫持續秒數

    [Header("攻擊Hitbox")]
    public Collider2D attackHitbox_1;
    public Collider2D attackHitbox_2;
    public Collider2D attackHitbox_3;

    [Header("召喚騎士")]
    public GameObject knightPrefab;

    [Header("變身特效")]
    public GameObject phase2EffectPrefab;
    public float phase2TransformTime = 2f;
    public int phase2ExplosionDamage = 1;

    [Header("瞬移特效")]
    public GameObject teleportEffectPrefab;

    [Header("召換特效")]
    public GameObject summonEffectPrefab;
    private float teleportTimer = 0f;
    private float attackTimer = 0f;
    private bool isTeleporting = false;
    private bool isAttacking = false;
    private bool isTransforming = false;
    private Vector3 lastPosition;
    private List<GameObject> summonedKnights = new List<GameObject>();
    private enum AttackType { attack1, attack2, attack3, summon }
    private bool isBossKingDead = false;

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
            FacePlayer();
        }
        UpdateMovementAnimation();
    }
    public void ForceDie()
    {
        if (isBossKingDead) return;
        isBossKingDead = true;

        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.removeenemy(gameObject);
        }

        gameObject.SetActive(false);
    }
    void Phase2ExplosionAttack()
    {
        var player = FindFirstObjectByType<Player_Property>();
        if (player != null)
        {
            // 傳Boss位置過去給玩家（比如擊退方向），如果不需要可以傳 Vector2.zero
            player.takedamage(phase2ExplosionDamage, transform.position);
            Debug.Log("Phase2 爆氣攻擊命中玩家");
        }
    }

    public void ClearSummonedKnights()
    {
        foreach (GameObject knight in summonedKnights)
        {
            if (knight != null)
            {
                Destroy(knight);
            }
        }
        summonedKnights.Clear();
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
        if (isAttacking) return;

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
        if (isTransforming || isTeleporting || isAttacking)
        {
            // 強制停住不撥移動動畫
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
            if (isTransforming) yield break;
            int summonCount = currentState == BossState.Phase1 ? 1 : 2;
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < summonCount; i++)
            {
                Vector2 randomPos = new Vector2(
                Random.Range(mapMinBounds.x / 2, mapMaxBounds.x / 2),
                Random.Range(mapMinBounds.y / 2, mapMaxBounds.y / 2)
                );
                Transform attackPivot = knightPrefab.transform.Find("attack pivot");
                Vector3 offset = attackPivot.position - knightPrefab.transform.position;

                Vector3 spawneffectPos = new Vector3(randomPos.x, randomPos.y, 0f) + offset;
                if (summonEffectPrefab != null)
                {
                    GameObject effect = Instantiate(summonEffectPrefab, spawneffectPos, Quaternion.identity);
                    Destroy(effect, 1.5f); // 自動刪除
                }

                Vector3 spawnknightPoint = new Vector3(randomPos.x, randomPos.y, 0f);
                GameObject knight = Instantiate(knightPrefab, spawnknightPoint, Quaternion.identity);
                summonedKnights.Add(knight);
            }
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            EnableAttackHitbox(selectedAttack, true);
            yield return new WaitForSeconds(attackDuration);
            EnableAttackHitbox(selectedAttack, false);
        }
        yield return new WaitForSeconds(1);
        isAttacking = false;
    }

    IEnumerator TeleportWithWarning()
    {
        isTeleporting = true;

        Vector3 teleportTarget = player.position;
        teleportTarget.x = Mathf.Clamp(teleportTarget.x, mapMinBounds.x, mapMaxBounds.x);
        teleportTarget.y = Mathf.Clamp(teleportTarget.y, mapMinBounds.y, mapMaxBounds.y);

        GameObject warning = Instantiate(teleportWarningPrefab, teleportTarget, Quaternion.identity);
        yield return new WaitForSeconds(teleportWarningTime - 0.5f);

        if (teleportEffectPrefab != null)
        {
            GameObject teleportEffect = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
            //  讓特效 Animator 播放速度變快
            Animator fxAnimator = teleportEffect.GetComponent<Animator>();
            if (fxAnimator != null)
            {
                fxAnimator.speed = 2f; // 調成2倍速
                StartCoroutine(RestoreteleporteffectAnimatorSpeed(fxAnimator, 1f, 0.25f)); // 1.5秒後恢復
            }
            Destroy(teleportEffect, 0.25f);
        }
        yield return new WaitForSeconds(0.25f);

        Destroy(warning);

        transform.position = teleportTarget;

        if (teleportEffectPrefab != null)
        {
            GameObject teleportEffect = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
            //  讓特效 Animator 播放速度變快
            Animator fxAnimator = teleportEffect.GetComponent<Animator>();
            if (fxAnimator != null)
            {
                fxAnimator.speed = 2f; // 調成2倍速
                StartCoroutine(RestoreteleporteffectAnimatorSpeed(fxAnimator, 1f, 0.25f)); // 1.5秒後恢復
            }
            Destroy(teleportEffect, 0.25f);
        }
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
        {
            ClearSummonedKnights();  //進Phase2時清掉小怪
            StartCoroutine(Phase2TransformRoutine());
        }
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
        hp.max_health = targetHealth;

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
        {
            GameObject effect = Instantiate(phase2EffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f); // 讓這個物體在 0.5 秒後自動銷毀
        }
        ClearSummonedKnights();

        yield return new WaitForSeconds(0.5f);

        hp.current_health = targetHealth;
        hp.max_health = targetHealth;
        hp.atk = Mathf.RoundToInt(hp.atk * 1.5f);
        hp.def = Mathf.RoundToInt(hp.def * 1.2f);

        EnterPhase2();

        Phase2ExplosionAttack();

        Debug.Log("Boss 完成變身！");

        yield return new WaitForSeconds(1);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_Property = collision.GetComponent<Player_Property>();
            Player_Property.takedamage(enemy.atk, transform.position);
        }
    }

    public void StartPhase2Death()
    {
        StartCoroutine(Phase2DeathRoutine());
    }

    IEnumerator Phase2DeathRoutine()
    {
        Debug.Log("Boss 第二階段死亡，播放死亡動畫");

        isTeleporting = true;
        isAttacking = true;
        isTransforming = true; // 全部鎖住

        // 停止所有攻擊動作、清空協程、關閉Hitbox
        StopAllCoroutines();
        DisableAllAttackHitbox();
        animator.ResetTrigger(attack1TriggerName);
        animator.ResetTrigger(attack2TriggerName);
        animator.ResetTrigger(attack3TriggerName);
        animator.ResetTrigger(summonTriggerName);
        animator.Play("Idle", -1, 0f);

        if (animator != null)
        {
            animator.SetTrigger(deathTriggerName);
        }

        // 等待死亡動畫播完
        yield return new WaitForSeconds(deathAnimationDuration);

        // 清除召喚物
        ClearSummonedKnights();

        // 完成死亡
        var hp = GetComponent<enemy_property>();
        if (hp != null)
        {
            hp.ForceDie(); //
        }

        Debug.Log("Boss 第二階段死亡結束，移除物件");
    }
    IEnumerator RestoreteleporteffectAnimatorSpeed(Animator animator, float normalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (animator != null)
        {
            animator.speed = normalSpeed;
        }
    }
    void DisableAllAttackHitbox()
    {
        attackHitbox_1.enabled = false;
        attackHitbox_2.enabled = false;
        attackHitbox_3.enabled = false;
    }
}