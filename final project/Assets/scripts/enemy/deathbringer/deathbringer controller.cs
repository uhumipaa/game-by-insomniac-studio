using System.Collections;
using UnityEngine;

public class EnemySummoner : MonoBehaviour
{
    public GameObject attackHitbox;
    public Player_Property player_Property;
    public enemy_property enemy_Property;

    [Header("參考設定")]
    public Animator animator;
    public Transform player;
    public GameObject summonPrefab;
    private Transform summonPoint;

    [Header("行為設定")]
    public float summonInterval = 5f;
    public float playerDetectRange = 3f;

    private bool isAttacking = false;
    public Vector3 originalPosition;
    private bool canSummon = true;

    // 在動畫中呼叫：前進
    public void StepForward()
    {
        originalPosition = transform.position;

        float faceDirX = Mathf.Sign(transform.localScale.x); // -1 = 右, 1 = 左（你的Facing反轉邏輯）
        Vector3 offset = new Vector3(-faceDirX * 1f, 0.6f, 0f);  // 移動 1 單位 + 向上 1 單位
        transform.position += offset;
    }

    // 在動畫中呼叫：退回
    public void StepBack()
    {
        transform.position = originalPosition;
    }

    void Start()
    {
        originalPosition = transform.position;
        if (player == null)
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
        }
        summonPoint = player;
        StartCoroutine(SummonRoutine());
    }

    void Update()
    {
        HandleFacing();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isPlayerNear = distanceToPlayer <= playerDetectRange;
        bool isPlayerAbove = player.position.y >= transform.position.y;

        if (isPlayerNear && isPlayerAbove && !isAttacking)
        {
            StartCoroutine(DoAttack1());
        }
        else if (isPlayerNear && !isPlayerAbove && !isAttacking && canSummon)
        {
            StartCoroutine(DoAttack2WithCooldown());
        }
    }

    IEnumerator DoAttack1()
    {
        isAttacking = true;
        animator.SetTrigger("triggerAttack1");

        yield return new WaitForSeconds(1f); // 攻擊1動畫長度（可調整）

        isAttacking = false;
    }

    IEnumerator SummonRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(summonInterval);

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > playerDetectRange && !isAttacking && canSummon)
            {
                StartCoroutine(DoAttack2WithCooldown());
            }
        }
    }

    IEnumerator DoAttack2()
    {
        isAttacking = true;
        animator.SetTrigger("triggerAttack2");

        yield return new WaitForSeconds(1f); // 攻擊2動畫長度（可調整）

        GameObject summon = Instantiate(summonPrefab, summonPoint.position, Quaternion.identity);
        SummonHitboxController ctrl = summon.GetComponent<SummonHitboxController>();
        if (ctrl != null)
        {
            ctrl.Initialize(player.GetComponent<Player_Property>(), this.GetComponent<enemy_property>());
        }
        Animator summonAnim = summon.GetComponent<Animator>();

        if (summonAnim != null)
        {
            summonAnim.Play("Summon");
        }
        isAttacking = false;
    }

    IEnumerator DoAttack2WithCooldown()
    {
        canSummon = false;
        yield return StartCoroutine(DoAttack2()); // 播放召喚動畫 + 出招
        yield return new WaitForSeconds(summonInterval); // 等待冷卻
        canSummon = true;
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_Property = collision.GetComponent<Player_Property>();
            player_Property.takedamage(enemy_Property.atk, transform.position);
        }
    }

    void HandleFacing()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 只有當前不是 Attack1 或 Attack2 時才允許轉向
        if (!stateInfo.IsName("Attack1") && !stateInfo.IsName("Attack2"))
        {
            Vector3 dir = player.position - transform.position;
            if (dir.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (dir.x > 0 ? -1 : 1);
                transform.localScale = scale;
            }
        }
    }
}