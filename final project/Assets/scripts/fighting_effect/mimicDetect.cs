using UnityEngine;

public class MimicProximityTrigger : MonoBehaviour
{
    private MonoBehaviour chaseScript; // 這是你的 mimic 追逐腳本
    private bool hasActivated = false; // 確保只觸發一次

    void Start()
    {
        // mimic 自動偵測自身
        chaseScript = GetComponent<MonoBehaviour>(); // 這裡替換成你的追逐腳本名稱
        if (chaseScript == null)
        {
            Debug.LogError("追逐腳本未掛載在 mimic 上！");
            return;
        }

        // 確保一開始追逐腳本是關閉的
        chaseScript.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            chaseScript.enabled = true; // 啟動 mimic 追逐腳本
            hasActivated = true; // 確保只啟動一次
            Debug.Log("Player 進入偵測範圍，開始追逐！");
        }
    }
}
