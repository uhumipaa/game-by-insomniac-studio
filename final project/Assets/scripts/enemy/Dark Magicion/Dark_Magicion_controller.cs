using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dark_Magicion_Controller : MonoBehaviour
{
    // 🔷 這個 enum 用來定義魔法邏輯類型
    public enum MagicPattern
    {
        Pattern1, // 漩渦
        Pattern2, // 叫小怪 
        Pattern3, // 閃電風暴
        Pattern4
    }

    // 🔷 每種邏輯會對應一組 prefab 清單
    [System.Serializable]
    public class MagicPatternGroup
    {
        public MagicPattern patternType;
        public List<GameObject> prefabs;
    }

    [Header("魔法設定（分類邏輯）")]
    public List<MagicPatternGroup> magicPatternGroups;
    public Transform magicSpawnPoint;
    public float magicInterval = 5f;

    [Header("召喚小怪設定")]
    public List<GameObject> summonPrefabs;             // 可召喚的小怪 prefab 列表
    public List<Transform> summonPoints;               // 可能的生成點
    public int summonCount = 3;                        // 要召喚幾次
    public float summonDelay = 0.5f;                   // 每次召喚的間隔
    public float summonOffsetRange = 0.5f;              // 生成時隨機偏移距離


    [Header("水晶設定")]
    public List<GameObject> crystalPrefabs;
    public List<Transform> crystalSpawnPoints;
    public List<float> crystalSpawnDelays = new List<float> { 3f, 4f, 5f, 6f };
    public int totalCrystals = 4;
    private int crystalsRemaining;

    [Header("元件參考")]
    public enemy_property enemyStats;
    private Animator animator;

    [Header("Boss 死亡時要關閉的物件")]
    public List<GameObject> objectsToDisableOnDeath;

    // 快取數值（可選）
    private int maxHealth;
    private int atk;
    private int def;
    private float range;
    private float speed;
    private float stuncd;
    private GameObject darkSparkPrefab;
    private GameObject ThunderPrefab;

    private void Start()
    {
        // 使用通用方法取得 Pattern1 的 prefab 並指定給 darkSparkPrefab
        var pattern1Prefabs = GetPatternPrefabs(MagicPattern.Pattern1);
        if (pattern1Prefabs != null && pattern1Prefabs.Count > 0)
        {
            darkSparkPrefab = pattern1Prefabs[0];
        }
        else
        {
            Debug.LogWarning(" Pattern1 的 prefab 未正確設定！");
        }

        // 如果你還要使用 Pattern2、Pattern3 也可以這樣：
        var pattern3Prefabs = GetPatternPrefabs(MagicPattern.Pattern3);
        if (pattern3Prefabs != null && pattern3Prefabs.Count > 0)
        {
            ThunderPrefab = pattern3Prefabs[0];
        }
        else
        {
            Debug.LogWarning(" Pattern3 的 prefab 未正確設定！");
        }


        // 抓 Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError(" 沒有找到 Animator 元件！");
        }

        // 初始化屬性
        maxHealth = enemyStats.max_health;
        atk = enemyStats.atk;
        def = enemyStats.def;
        range = enemyStats.attack_range;
        speed = enemyStats.speed;
        stuncd = enemyStats.stuncd;

        crystalsRemaining = totalCrystals;

        // 啟動協程
        StartCoroutine(SpawnCrystalsStaggered());
        StartCoroutine(CastMagicLoop());
    }
    private List<GameObject> GetPatternPrefabs(MagicPattern pattern)
    {
        foreach (var group in magicPatternGroups)
        {
            if (group.patternType == pattern)
                return group.prefabs;
        }
        return null;
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

            // 範例邏輯：隨機選一種模式
            int pattern = Random.Range(0, 4);

            if (pattern == 0)
            {
                StartCoroutine(CastPattern1()); // 多方向 Dark_Spark
                Debug.Log("釋放Caspattern1");
                animator.SetTrigger("attack1");
            }
            else if (pattern == 1)
            {
                StartCoroutine(CastPattern2()); // 烙人
                Debug.Log("釋放Caspattern2");
                animator.SetTrigger("attack2");
            }
            else if (pattern == 2)
            {
                StartCoroutine(CastPattern3()); // 傘電
                Debug.Log("釋放Caspattern3");
                animator.SetTrigger("attack2");
            }
            else
            {

            }
        }
    }


    private IEnumerator CastPattern1()
    {
        int bulletCount = 12;
        int totalWaves = 5;
        float angleOffsetPerWave = 7f;
        float interval = 1.5f;

        for (int wave = 0; wave < totalWaves; wave++)
        {
            float startAngle = wave * angleOffsetPerWave;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + i * (360f / bulletCount);
                float rad = angle * Mathf.Deg2Rad;

                Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                GameObject spark = Instantiate(darkSparkPrefab, magicSpawnPoint.position, Quaternion.identity);
                spark.SetActive(true);
                spark.GetComponent<Dark_Spark_Controller>().SetDirection(dir);
            }

            yield return new WaitForSeconds(interval);
        }

        Debug.Log("Dark Magicion 施放螺旋彈幕 Pattern 1！");
        yield return new WaitForSeconds(8f);
    }


    private IEnumerator CastPattern2()
    {
        for (int i = 0; i < summonCount; i++)
        {
            // 隨機選擇一個生成點
            Transform basePoint = summonPoints[Random.Range(0, summonPoints.Count)];

            // 隨機偏移位置
            Vector3 offset = new Vector3(
                Random.Range(-summonOffsetRange, summonOffsetRange),
                Random.Range(-summonOffsetRange, summonOffsetRange),
                0f
            );

            Vector3 spawnPosition = basePoint.position + offset;

            // 隨機選一種小怪 prefab
            if (summonPrefabs.Count == 0)
            {
                Debug.LogWarning("沒有設定任何可召喚的小怪 prefab！");
                yield break;
            }

            GameObject chosenEnemy = summonPrefabs[Random.Range(0, summonPrefabs.Count)];

            Instantiate(chosenEnemy, spawnPosition, Quaternion.identity);

            GameObject enemy = Instantiate(chosenEnemy, spawnPosition, Quaternion.identity);
            enemy.SetActive(true); // 強制啟用

            Debug.Log($"Dark Magicion 召喚小怪：{chosenEnemy.name}，位置：{spawnPosition}");

            yield return new WaitForSeconds(summonDelay);
        }
        Debug.Log("Dark Magicion 叫出小怪 Pattern 1！");
        yield return new WaitForSeconds(8f);

    }

    private IEnumerator CastPattern3()
    {
        int totalRounds = 3;
        int spawnPerRound = 2;
        float interval = 2f;

        float xMin = -8f; // 根據你地圖範圍自行調整
        float xMax = 8f;
        float yMin = -4f;
        float yMax = 4f;

        for (int round = 0; round < totalRounds; round++)
        {
            for (int i = 0; i < spawnPerRound; i++)
            {
                Vector2 randomPos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
                GameObject selectedPrefab = ThunderPrefab;
                GameObject obj = Instantiate(selectedPrefab, randomPos, Quaternion.identity);
                obj.SetActive(true); // 確保啟用

                Debug.Log($"Pattern3 第 {round + 1} 輪：生成 {selectedPrefab.name} 於 {randomPos}");
            }

            yield return new WaitForSeconds(interval);
        }

        Debug.Log("Dark Magicion 完成 Pattern3！");
        yield return new WaitForSeconds(8f);
    }


    public void TakeDamageFromCrystal()
    {

        crystalsRemaining--;
        int damage = Mathf.CeilToInt((float)enemyStats.max_health / totalCrystals) + 3;
        damage++;
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

        // 先處理要關閉的物件
        DisableObjectsOnDeath();

        if (animator != null)
        {
            animator.SetTrigger("DarkMagicionDie");
        }
        StartCoroutine(DieAfterAnimation(2.2f));
    }

    IEnumerator DieAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyStats.ForceDie();
    }
    
    private void DisableObjectsOnDeath()
    {
        foreach (GameObject obj in objectsToDisableOnDeath)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
