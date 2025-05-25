using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dark_Magicion_Controller : MonoBehaviour
{
    [Header("魔法設定")]
    public List<GameObject> magicPrefabs;
    public Transform magicSpawnPoint;
    public float magicInterval = 2f;

    [Header("水晶設定")]
    public List<GameObject> crystalPrefabs;
    public List<Transform> crystalSpawnPoints;
    public List<float> crystalSpawnDelays = new List<float> { 3f, 4f, 5f, 6f };
    public int totalCrystals = 4;
    private int crystalsRemaining;

    [Header("元件參考")]
    public enemy_property enemyStats;
    private Animator animator; // ✅ 動畫元件

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

        // ✅ 抓 Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("沒有找到 Animator 元件！");
        }

        // 快取數值
        maxHealth = enemyStats.max_health;
        atk = enemyStats.atk;
        def = enemyStats.def;
        range = enemyStats.attack_range;
        speed = enemyStats.speed;
        stuncd = enemyStats.stuncd;

        crystalsRemaining = totalCrystals;

        StartCoroutine(SpawnCrystalsStaggered());
        StartCoroutine(CastMagicLoop());
    }

    IEnumerator SpawnCrystalsStaggered()
    {
        int spawnCount = Mathf.Min(crystalSpawnPoints.Count, crystalPrefabs.Count, crystalSpawnDelays.Count);
        float totalDelay = 0f;

        Debug.Log("StartCoroutine 啟動 SpawnCrystalsStaggered");
        Debug.Log($"總共預計生成 {spawnCount} 顆水晶");

        for (int i = 0; i < spawnCount; i++)
        {
            float delay = crystalSpawnDelays[i];
            float waitTime = (i == 0) ? delay : delay - crystalSpawnDelays[i - 1];
            yield return new WaitForSeconds(waitTime);
            totalDelay += waitTime;

            GameObject prefabToSpawn = crystalPrefabs[i];
            Transform spawnPoint = crystalSpawnPoints[i];

            GameObject crystal = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
            crystal.SetActive(true);
            Debug.Log($"✔ 第 {i + 1} 顆水晶已生成於 {spawnPoint.position}（總延遲 {totalDelay} 秒）");

            var script = crystal.GetComponent<Crystal>();
            if (script != null)
                script.boss = this;
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
        int damage = Mathf.CeilToInt((float)enemyStats.max_health / totalCrystals)+1;
        damage++; // 額外補正
        enemyStats.takedamage(damage, transform.position);

        Debug.Log($"水晶被破壞，Dark Magicion 扣 {damage} HP，目前剩餘 {enemyStats.current_health}");

        if (enemyStats.current_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Dark Magicion 被擊敗！");

        //  播放死亡動畫（Trigger）
        if (animator != null)
        {
            animator.SetTrigger("DarkMagicionDie");
        }

        //  延遲一點再處理 Disable（選擇性，確保動畫有播出來）
        StartCoroutine(DieAfterAnimation(2.2f)); // 1.5 秒後真正消失
    }

    IEnumerator DieAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        enemyStats.ForceDie(); // 關閉物件或其他死亡處理
    }
}

