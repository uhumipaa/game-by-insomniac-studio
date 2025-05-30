using UnityEngine;

[CreateAssetMenu(menuName = "QuestLogic/Tower Quest")]
public class TowerQuestLogic : ScriptableObject, IQuestLogic
{
    public int requiredFloor;    // 要完成的樓層數
    public float coinReward = 100f;

    

    public bool IsComplete()
    {
        return TowerManager.Instance != null &&
               TowerManager.Instance.finishfloorthistime >= requiredFloor;
    }

    public void GiveReward()
    {
        CoinManager.instance.AddCoins(coinReward);
        Debug.Log($"任務完成：通關 {requiredFloor} 層，獲得 {coinReward} 金幣");
    }

    public string GetProgressText()
    {
        int current = TowerManager.Instance != null ? TowerManager.Instance.finishfloorthistime : 0;
        return $"完成樓層：{current} / {requiredFloor}";
    }
}
