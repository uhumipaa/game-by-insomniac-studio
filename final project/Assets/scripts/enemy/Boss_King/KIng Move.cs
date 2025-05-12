using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("目標 & 地圖範圍")]
    public Transform player;                 // 玩家Transform
    public Vector2 mapMinBounds;             // 地圖左下界限
    public Vector2 mapMaxBounds;             // 地圖右上界限

    [Header("追逐設定")]
    public float moveSpeed = 3f;             // 追逐速度
    public float stopDistance = 2f;          // 與玩家保持距離 (不貼臉)

    [Header("瞬移設定")]
    public float teleportDistance = 3f;      // 瞬移時與玩家距離
    public float teleportCooldown = 5f;      // 瞬移冷卻時間
    public float teleportWarningTime = 1f;   // 警告標記顯示時間
    public GameObject teleportWarningPrefab; // 瞬移警告Prefab (紅圈、特效等)

    [Header("攻擊動畫")]
    public Animator animator;            // Boss Animator
    public string attack1TriggerName = "Attack1";  // 攻擊動畫觸發器
    public string attack2TriggerName = "Attack2";
    public string attack3TriggerName = "Attack3";
    public float attackDuration = 1f;    // 攻擊動畫持續時間

    [Header("攻擊hitbox")]
    public Collider2D attackHitbox_1;
    public Collider2D attackHitbox_2;
    public Collider2D attackHitbox_3;

    private float teleportTimer = 0f;
    private bool isTeleporting = false;
    private enum AttackType { attack1, attack2, attack3 }

    void Update()
    {
        teleportTimer += Time.deltaTime;

        if (!isTeleporting && teleportTimer >= teleportCooldown)
        {
            // 到時間了，開始瞬移前的警告
            StartCoroutine(TeleportWithWarning());
            teleportTimer = 0f;
        }
        else if (!isTeleporting)
        {
            // 追蹤玩家 (保持距離)
            ChasePlayer();
        }
        FacePlayer();
    }

    /// <summary>
    /// 讓Boss面對玩家 (2D左右翻轉)
    /// </summary>
    void FacePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;

        // 如果玩家在右側 → scaleX 為正，左側則為負
        if (toPlayer.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// 追蹤玩家，但與玩家保持一定距離 (stopDistance)
    /// </summary>
    void ChasePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        // 只有超出停止距離時才移動
        if (distance > stopDistance)
        {
            Vector3 targetPosition = player.position;

            // 限制目標位置不會超出地圖範圍
            targetPosition.x = Mathf.Clamp(targetPosition.x, mapMinBounds.x, mapMaxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mapMinBounds.y, mapMaxBounds.y);
            targetPosition.z = transform.position.z; // 保持Z軸 (2D場景)

            // 慢慢接近目標位置
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 瞬移前的警告流程 (先顯示警告，等待，再瞬移)
    /// </summary>
    IEnumerator TeleportWithWarning()
    {
        isTeleporting = true;

        // 計算瞬移的目標位置 (玩家背後)
        Vector3 teleportTarget = GetTeleportToPlayerPosition();

        // 生成警告標記
        GameObject warning = Instantiate(teleportWarningPrefab, teleportTarget, Quaternion.identity);

        // 等待警告時間 (這段期間可以做閃爍特效)
        yield return new WaitForSeconds(teleportWarningTime);

        // 移除警告標記
        Destroy(warning);

        // 瞬移到該位置
        transform.position = teleportTarget;

        Debug.Log("Boss瞬移到玩家並準備攻擊：" + teleportTarget);

        // 隨機選擇攻擊方式
        AttackType selectedAttack = (AttackType)Random.Range(0, 3);

        // 撥放攻擊動畫
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
            }
        }


        // 等待播放動畫
         yield return new WaitForSeconds(0.8f);

        // 攻擊期間啟動碰撞檢測 (用Enable Collider等方式)
        EnableAttackHitbox(selectedAttack, true);

        // 等待攻擊持續時間
        yield return new WaitForSeconds(attackDuration);

        // 關閉攻擊Hitbox
        EnableAttackHitbox(selectedAttack, false);

        isTeleporting = false;
    }

    /// <summary>
    /// 瞬移到玩家當前位置 (限制邊界)
    /// </summary>
    Vector3 GetTeleportToPlayerPosition()
    {
        Vector3 targetPosition = player.position;

        // Clamp 限制瞬移後的位置不會超出地圖邊界
        targetPosition.x = Mathf.Clamp(targetPosition.x, mapMinBounds.x, mapMaxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, mapMinBounds.y, mapMaxBounds.y);
        targetPosition.z = transform.position.z; // 保持Z軸

        return targetPosition;
    }

    /// <summary>
    /// Gizmos：在Scene視窗顯示瞬移範圍與地圖邊界
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            // 瞬移範圍顯示 (紅色半透明圓)
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(player.position, teleportDistance);

            // 地圖邊界顯示 (綠色框)
            Gizmos.color = Color.green;
            Vector3 center = (mapMinBounds + mapMaxBounds) / 2;
            Vector3 size = new Vector3(
                mapMaxBounds.x - mapMinBounds.x,
                mapMaxBounds.y - mapMinBounds.y,
                0.1f // Z軸厚度 (2D)
            );
            Gizmos.DrawWireCube(center, size);
        }
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
}
