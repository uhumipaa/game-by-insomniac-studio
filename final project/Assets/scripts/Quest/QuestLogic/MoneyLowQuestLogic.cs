using UnityEngine;

[CreateAssetMenu(menuName = "QuestLogic/Money Low Quest")]
public class MoneyLowQuestLogic : ScriptableObject, IQuestLogic
{
    public float coinThreshold = 20f;    // 判定門檻（<=）
    public float rewardAmount = 250f;    // 給予金幣
    private bool rewardGiven = false;

    public bool IsComplete()
    {
        return CoinManager.instance != null &&
               CoinManager.instance.GetCoins() <= coinThreshold;
    }

    public void GiveReward()
    {
        CoinManager.instance.AddCoins(rewardAmount);
        Debug.Log($"🪙 任務完成：金錢低於 {coinThreshold}，發送補助 {rewardAmount} 金幣");
    }

}
