using UnityEngine;

[CreateAssetMenu(menuName = "QuestLogic/Fishing Quest")]
public class FishingQuestLogic : ScriptableObject, IQuestLogic
{
    public int requiredAttempts;        // 所需釣魚次數
    public float coinReward;            // 金幣獎勵
    public ItemData rewardItem;         // ⭐ 改成 ItemData
    public int itemAmount = 1;

    private bool isRewardGiven = false;

    public bool IsComplete()
    {
        return FishingUIManager.Instance.TotalFishingAttempts >= requiredAttempts;
    }

    public void GiveReward()
    {
        if (isRewardGiven)
        {
            Debug.Log("獎勵已發送過");
            return;
        }

        CoinManager.instance.AddCoins(coinReward);

        if (rewardItem != null)
        {
            InventoryManager.Instance.Add("Backpack", rewardItem, itemAmount); // ⭐ 傳 ItemData
        }

        isRewardGiven = true;

        Debug.Log($"獲得獎勵：{coinReward} 金幣 + {itemAmount} 個 {rewardItem.itemName}");
    }

    public string GetProgressText()
    {
        return $"釣魚次數：{FishingUIManager.Instance.TotalFishingAttempts}/{requiredAttempts}";
    }
}
