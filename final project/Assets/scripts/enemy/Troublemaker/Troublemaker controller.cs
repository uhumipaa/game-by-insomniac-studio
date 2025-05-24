using UnityEngine;

public class EnemyChaseLock : MonoBehaviour
{
    public Transform player;
    public Player_Property player_Property;
    public enemy_property enemy_Property;
    public float moveSpeed = 5f;
    public float stopDuration = 2f;

    private Animator animator;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isWaiting = false;
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
        spriteRenderer = GetComponent<SpriteRenderer>(); // 取得 SpriteRenderer
        StartCoroutine(MoveRoutine());
    }

    void Update()
    {
        if (isMoving && !isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ➤ 翻面處理：根據目標方向調整 sprite 水平翻轉
            if (targetPosition.x > transform.position.x)
                spriteRenderer.flipX = false; // 朝右
            else
                spriteRenderer.flipX = true;  // 朝左

            // 到達目標位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
                StartCoroutine(WaitThenMove());
            }
        }
    }

    System.Collections.IEnumerator MoveRoutine()
    {
        while (true)
        {
            // 鎖定玩家當前位置
            targetPosition = player.position;
            isMoving = true;

            // 播放 Move 動畫
            animator.Play("Move");

            // 等待移動完成
            yield return new WaitUntil(() => !isMoving);
        }
    }

    System.Collections.IEnumerator WaitThenMove()
    {
        // 播放 Idle 動畫
        animator.Play("Idle");

        isWaiting = true;
        yield return new WaitForSeconds(stopDuration);
        isWaiting = false;

        // 等待完 → 重新啟動移動
        StartCoroutine(MoveRoutine());
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