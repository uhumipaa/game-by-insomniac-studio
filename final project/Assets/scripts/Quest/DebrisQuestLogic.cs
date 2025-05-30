using UnityEngine;

[CreateAssetMenu(menuName = "QuestLogic/Debris Quest")]
public class DebrisQuestLogic : ScriptableObject, IQuestLogic
{
    public int requiredDebris = 3;
    public float coinReward = 99f;
    public ItemData rewardItem;

    private bool rewardGiven = false;

    public bool IsComplete()
    {
        return FishingUIManager.Instance != null &&
               FishingUIManager.Instance.DebrisCaughtCount >= requiredDebris;
    }

    public void GiveReward()
    {
        if (rewardGiven)
        {
            Debug.Log("Debris任務已領獎");
            return;
        }

        CoinManager.instance.AddCoins(coinReward);

        if (rewardItem != null)
        {
            InventoryManager.Instance.Add("Backpack", rewardItem, 1); // 固定給 1
        }

        rewardGiven = true;

        Debug.Log($"🪣 任務完成：撈到 {requiredDebris} 廢棄物，獲得 {coinReward} 金幣 + 1 個 {rewardItem.name}");
    }
}
