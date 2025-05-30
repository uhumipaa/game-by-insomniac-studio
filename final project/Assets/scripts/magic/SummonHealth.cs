using UnityEngine;

public class SummonHealth : MonoBehaviour, isMagic
{
    public GameObject healthAreaPrefab;
    public Animator HealthAreaCD; // 在 Inspector 連到 CooldownIcon 的 Animator

    public void CastHealArea(Vector2 targetPosition)
    {
        if (healthAreaPrefab == null)
        {
            Debug.LogWarning("尚未設定 Healing prefab！");
            return;
        }
        Debug.LogWarning("summon succes");
        HealthAreaCD.SetTrigger("StartCD");
        GameObject instance = Instantiate(healthAreaPrefab, targetPosition, Quaternion.identity);
        instance.SetActive(true); // 保險啟用
    }

    public void cast()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CastHealArea(player.transform.position);
        }
        HealthAreaCD.SetTrigger("StartCD");
    }
}
