using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour
{
    public int level = 1; // 經驗等級
    public int currentExp = 0; // 目前經驗
    public int expToNextLevel; //到下一擊所需的經驗值

    [SerializeField] private float expGrowthRate = 2f;  // 經驗成長倍率
    public ExpAddUI expAddUI;

    //初始化經驗值值
    void Start()
    {
        expToNextLevel = CalculateExpToNextLevel(level);
    }
    public void initial()
    {
        level = 1;
        currentExp = 0;
        expToNextLevel = CalculateExpToNextLevel(level);
        GameObject exp = GameObject.FindGameObjectWithTag("exp");
        if (exp != null)
        {
            expAddUI = exp.GetComponent<ExpAddUI>();
            expAddUI.point = 10;
        }
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name=="tower"&&expAddUI==null)
            expAddUI = FindAnyObjectByType<ExpAddUI>();
    }

    //計算經驗和經驗等級
    public void GainExp(int amount)
    {
        GameObject exp = GameObject.FindGameObjectWithTag("exp");
        if (exp != null)
        {
            expAddUI = exp.GetComponent<ExpAddUI>();
        }
        currentExp += amount;
        Debug.Log($"獲得經驗值：{amount}，距離下次升級所需經驗：{expToNextLevel - currentExp}");

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            expToNextLevel = CalculateExpToNextLevel(level);
            Debug.Log($"升級了！目前等級：{level}");
            if(expAddUI!=null)
                expAddUI.addpoint();
            // expAddUI.point++;
        }
    }

    //計算每個等級所需的經驗值
    private int CalculateExpToNextLevel(int level)
    {
        return Mathf.RoundToInt(expToNextLevel * Mathf.Pow(expGrowthRate, level - 1));
    }
}
