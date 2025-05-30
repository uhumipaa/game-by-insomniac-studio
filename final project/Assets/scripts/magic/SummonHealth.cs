using UnityEngine;

public class SummonHealth : MonoBehaviour, isMagic
{
    public GameObject healthAreaPrefab;

    public void CastHealArea(Vector2 targetPosition)
    {
        if (healthAreaPrefab == null)
        {
            Debug.LogWarning("尚未設定 Healing prefab！");
            return;
        }
        Debug.LogWarning("summon succes");
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
    }
}
