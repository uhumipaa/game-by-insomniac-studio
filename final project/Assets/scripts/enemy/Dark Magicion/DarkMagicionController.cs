using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkMagicionController : MonoBehaviour
{
    [Header("魔法設定")]
    public List<GameObject> magicPrefabs;
    public Transform magicSpawnPoint;
    public float magicInterval = 2f;

    [Header("水晶設定")]
    public List<GameObject> crystalPrefabs; // ✅ 支援多個水晶 prefab
    public List<Transform> crystalSpawnPoints;
    public int totalCrystals = 4;
    private int crystalsRemaining;

    [Header("元件參考")]
    public enemy_property enemyStats;

    // 快取數值（可選）
    private int maxHealth;
    private int atk;
    private int def;
    private float range;
    private float speed;
    private float stuncd;

    private void Start()
    {
        if (enemyStats == null)
        {
            Debug.LogError("enemyStats 沒有指定，請拖拉 enemy_property 元件進來！");
            return;
        }

        maxHealth = enemyStats.max_health;
        atk = enemyStats.atk;
        def = enemyStats.def;
        range = enemyStats.attack_range;
        speed = enemyStats.speed;
        stuncd = enemyStats.stuncd;

        crystalsRemaining = totalCrystals;

        SpawnCrystals();
        StartCoroutine(CastMagicLoop());
    }

    private void SpawnCrystals()
    {
        for (int i = 0; i < crystalSpawnPoints.Count; i++)
        {
            // ✅ 安全檢查：水晶 prefab 有設定，且不超出索引
            if (crystalPrefabs.Count == 0)
            {
                Debug.LogError("未設定任何水晶 prefab！");
                return;
            }

            GameObject prefabToSpawn = crystalPrefabs[Mathf.Min(i, crystalPrefabs.Count - 1)];
            GameObject crystal = Instantiate(prefabToSpawn, crystalSpawnPoints[i].position, Quaternion.identity);

            Crystal crystalScript = crystal.GetComponent<Crystal>();
            if (crystalScript != null)
            {
                crystalScript.boss = this;
            }
        }
    }

    IEnumerator CastMagicLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(magicInterval);

            if (magicPrefabs.Count == 0) yield break;

            int randomIndex = Random.Range(0, magicPrefabs.Count);
            GameObject selectedMagic = magicPrefabs[randomIndex];
            Instantiate(selectedMagic, magicSpawnPoint.position, Quaternion.identity);

            Debug.Log($"Dark Magicion 施放了魔法：{selectedMagic.name}，使用攻擊力：{enemyStats.atk}");
        }
    }

    public void TakeDamageFromCrystal()
    {
        if (enemyStats == null)
        {
            Debug.LogWarning("無法對 Boss 扣血，因為 enemy_property 尚未指定！");
            return;
        }

        crystalsRemaining--;
        int damage = Mathf.CeilToInt((float)enemyStats.max_health / totalCrystals);
        damage++;
        enemyStats.takedamage(damage, transform.position);

        Debug.Log($"水晶被破壞，Dark Magicion 扣 {damage} HP，目前剩餘 {enemyStats.current_health}");

        if (enemyStats.current_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dark Magicion 被擊敗！");
        enemyStats.ForceDie();
    }
}
