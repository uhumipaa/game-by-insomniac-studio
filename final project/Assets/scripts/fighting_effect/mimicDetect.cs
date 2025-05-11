using UnityEngine;

public class MimicProximityTrigger : MonoBehaviour
{
    private MonoBehaviour chaseScript; // 你的 mimic 追逐腳本
    private Animator animator;         // 控制 mimic 動畫
    private bool hasActivated = false; // 確保只觸發一次
    public float detectionRange = 3f;  // 觸發距離，玩家需要接近多少才觸發
    private Transform player;          // 玩家物件

    void Start()
    {
        // 確保 mimic 本身有 Animator 和 追逐腳本
        chaseScript = GetComponent<MonoBehaviour>(); // 替換成你的追逐腳本名稱 (如 MimicChase)
        animator = GetComponent<Animator>();

        if (chaseScript == null)
        {
            Debug.LogError("追逐腳本未掛載在 mimic 上！");
            return;
        }

        // 停止 mimic 的動畫播放
        if (animator != null)
        {
            animator.enabled = false; // 一開始禁用 Animator
        }

        // 確保一開始追逐腳本是關閉的
        chaseScript.enabled = false;

        // 自動尋找玩家（必須標記 Player）
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player 物件未找到，請確保玩家標籤為 Player！");
        }
    }

    void Update()
    {
        // 如果已經觸發過或玩家不存在，不檢查距離
        if (hasActivated || player == null) return;

        // 計算 mimic 與玩家的距離
        float distance = Vector2.Distance(transform.position, player.position);
        Debug.Log($"與玩家距離：{distance}");

        // 只有在玩家距離小於設定範圍時觸發
        if (distance <= detectionRange)
        {
            ActivateChase();
        }
    }

    private void ActivateChase()
    {
        hasActivated = true;        // 確保只觸發一次
        chaseScript.enabled = true; // 啟動 mimic 追逐腳本

        // 啟動 mimic 動畫
        if (animator != null)
        {
            animator.enabled = true;       // 開啟 Animator
            animator.SetBool("IsChasing", true); // 播放追逐動畫（確保 Animator 有 IsChasing 參數）
        }

        Debug.Log("玩家進入觸發範圍，mimic 開始追逐！");
    }
}
