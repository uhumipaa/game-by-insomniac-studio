using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel;

    [SerializeField]
    private float expGrowthRate = 2f;  // 成長倍率

    void Start()
    {
        expToNextLevel = CalculateExpToNextLevel(level);
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"獲得經驗值：{amount}，距離下次升級所需經驗：{expToNextLevel - currentExp}");

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            expToNextLevel = CalculateExpToNextLevel(level);
            Debug.Log($"升級了！目前等級：{level}");
        }
    }

    private int CalculateExpToNextLevel(int level)
    {
        return Mathf.RoundToInt(expToNextLevel * Mathf.Pow(expGrowthRate, level - 1));
    }
}
