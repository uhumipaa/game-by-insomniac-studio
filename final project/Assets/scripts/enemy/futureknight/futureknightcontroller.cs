using UnityEngine;
using System.Collections;

public class EnemyTeleportAttack : MonoBehaviour
{
    public float disappearInterval = 4f;
    public float teleportDelay = 1f;
    public float teleportDistanceFromPlayer = 2f;
    public Animator animator;
    public GameObject attackHitbox;

    private SpriteRenderer spriteRenderer;
    private Transform player;
    private float timer;
    private bool isTeleporting;
    private Vector3 originalPosition;
    public float attackMoveDistance = 1f; // 向前移動距離
    public Player_Property player_Property;
    public enemy_property enemy_Property;
    public Collider2D owncol;
    public GameObject healthBar;
    public GameObject EffectPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = disappearInterval;
        animator.SetBool("IsIdle", true); // 初始狀態為 Idle
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

    void Update()
    {
        timer -= Time.deltaTime;

        if (!isTeleporting && timer <= 0)
        {
            StartCoroutine(TeleportAndAttack());
            timer = disappearInterval + teleportDelay; // 重設計時器
        }
    }

    private IEnumerator TeleportAndAttack()
    {
        isTeleporting = true;
        animator.SetBool("IsIdle", false); // 停止播放 Idle

        // 1. 消失
        spriteRenderer.enabled = false;
        owncol.enabled = false;
        if (healthBar != null)
        healthBar.SetActive(false);

        // 3. 確認玩家方向並順移
        bool playerOnRight = player.position.x > transform.position.x;
        Vector3 teleportPosition = player.position + (playerOnRight ? Vector3.left : Vector3.right) * teleportDistanceFromPlayer;
        transform.position = teleportPosition;

        // 2. 等待 1 秒
        yield return new WaitForSeconds(teleportDelay);

        GameObject effect = Instantiate(EffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f); // 自動刪除

        // 4. 顯示
        spriteRenderer.enabled = true;
        owncol.enabled = true;
        if (healthBar != null)
        healthBar.SetActive(true);

        // 5. 面向玩家
        Vector3 scale = transform.localScale;
        float originalX = Mathf.Abs(scale.x); // 保留原本的大小
        scale.x = player.position.x < transform.position.x ? -originalX : originalX;
        transform.localScale = scale;

        // 6. 播放攻擊動畫
        animator.SetTrigger("Attack");

        // yield return new WaitForSeconds(1);

        // // 8. 回到 Idle 狀態
        // animator.SetBool("IsIdle", true);
        // isTeleporting = false;
    }

    public void idle()
    {
        animator.SetBool("IsIdle", true);
        isTeleporting = false;
    }

    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
        {
            Collider2D col = attackHitbox.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }
        // 記錄原始位置
        originalPosition = transform.position;

        // 根據面向方向移動一單位
        float direction = -Mathf.Sign(transform.localScale.x); // 左為 1，右為 -1（根據 scale 翻面）
        transform.position += new Vector3(-direction * attackMoveDistance, 0f, 0f);
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
