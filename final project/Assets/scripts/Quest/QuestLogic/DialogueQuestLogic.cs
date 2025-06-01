using UnityEngine;

[CreateAssetMenu(menuName = "QuestLogic/Dialogue Quest")]
public class DialogueQuestLogic : ScriptableObject, IQuestLogic
{
    public int requiredTalks = 1; // 大於等於 1 才算完成
    public float coinReward = 831f;

    private bool rewardGiven = false;

    public bool IsComplete()
    {
        return FishingStats.DialogueCount >= requiredTalks;
    }

    public void GiveReward()
    {
        if (rewardGiven)
        {
            Debug.Log("🗨️ 對話任務已領獎");
            return;
        }

        CoinManager.instance.AddCoins(coinReward);
        rewardGiven = true;
        Debug.Log($"✅ 任務完成：對話次數達 {requiredTalks}，獲得 {coinReward} 金幣");
    }

    public string GetProgressText()
    {
        return $"對話次數：{FishingStats.DialogueCount}/{requiredTalks}";
    }
}
