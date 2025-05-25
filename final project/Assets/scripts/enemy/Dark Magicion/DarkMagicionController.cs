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
    public GameObject crystalPrefab;
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
        //  不再使用 GetComponent，自行在 Inspector 指派
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
        foreach (var point in crystalSpawnPoints)
        {
            GameObject crystal = Instantiate(crystalPrefab, point.position, Quaternion.identity);
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

        // 扣除一顆水晶
        crystalsRemaining--;

        // 每顆水晶扣總血量的 1/N
        int damage = Mathf.CeilToInt((float)enemyStats.max_health / totalCrystals);

        // 使用 enemy_property 中的正規傷害流程
        enemyStats.takedamage(damage, transform.position);

        Debug.Log($"水晶被破壞，Dark Magicion 扣 {damage} HP，目前剩餘 {enemyStats.current_health}");

        // 不用手動判斷血量是否為 0，因為 enemy_property 的 takedamage 會自動判斷並呼叫 ForceDie() 或死亡流程
        // 若你仍然想額外做處理，也可以檢查再執行 Die()
        if (enemyStats.current_health <= 0)
        {
            Die(); // 可選：讓 bossController 自己額外處理動畫或結束流程
        }
    }


    private void Die()
    {
        Debug.Log("Dark Magicion 被擊敗！");
        enemyStats.ForceDie(); // 使用 enemy_property 內建的死亡處理
    }
}

   