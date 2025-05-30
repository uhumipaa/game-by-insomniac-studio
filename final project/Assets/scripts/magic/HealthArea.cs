using UnityEngine;
using System.Collections;

public class HealthArea : MonoBehaviour
{
    public int healAmountPerSecond = 10;   // 每秒補血量
    public float duration = 10f;           // 持續存在時間（秒）

    private void Start()
    {
        // 過指定時間自動銷毀
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 當玩家進入時開始回血協程
            StartCoroutine(HealOverTime(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 玩家離開時停止回血
            StopAllCoroutines();
        }
    }

    private IEnumerator HealOverTime(Collider2D playerCollider)
    {
        var player = playerCollider.GetComponent<Player_Property>();

        if (player == null)
        {
            Debug.LogWarning("⚠ 玩家身上找不到 player_property 腳本！");
            yield break;
        }

        while (true)
        {
            player.heal(healAmountPerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
}
