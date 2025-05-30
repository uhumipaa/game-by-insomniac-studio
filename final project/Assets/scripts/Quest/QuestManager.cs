using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;


public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestSlot
    {
        public GameObject rootObject;
        public Image rewardIcon;
        public Button rewardButton;
        public TextMeshProUGUI goalText;
        public TextMeshProUGUI rewardText;
        public Button nextButton;
    }

    public List<QuestSlot> questSlots = new List<QuestSlot>(); // 三個 slot
    public List<QuestData> allQuests = new List<QuestData>(); // 所有任務資料
    private List<QuestData> sortedQuests = new List<QuestData>();

    private int currentIndex = 0;
    public Sprite completedRewardIcon; // 完成任務時使用的圖示


    void Start()
    {
        foreach (var slot in questSlots)
        {
            slot.nextButton.onClick.AddListener(OnNextClicked);
        }

    }

    public void RefreshQuests()
    {

        // ⭐ 保留：未完成任務 + 完成但未領獎的任務
        List<QuestData> filteredQuests = allQuests
            .Where(q =>
            {
                if (q.questLogic is IQuestLogic logic)
                {
                    return !logic.IsComplete() || (logic.IsComplete() && !q.rewardClaimed);
                }
                return true; // 無邏輯也顯示
            })
            .OrderByDescending(q =>
            {
                if (q.questLogic is IQuestLogic logic)
                    return logic.IsComplete();
                return false;
            })
            .ToList();

        sortedQuests = filteredQuests;
        currentIndex = 0;
        UpdateQuestSlots();
    }



    void OnNextClicked()
    {
        if (sortedQuests.Count <= 3)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex = (currentIndex + 1) % (sortedQuests.Count - 2); // 避免溢位
        }

        UpdateQuestSlots();
    }

    public void GiveReward(int questIndex)
    {
        if (questIndex < 0 || questIndex >= sortedQuests.Count)
            return;

        QuestData quest = sortedQuests[questIndex];

        if (quest.rewardClaimed)
            return;

        if (quest.questLogic is IQuestLogic logic && logic.IsComplete())
        {
            logic.GiveReward();             // 發獎
            quest.rewardClaimed = true;     // 標記為已領
            RefreshQuests();                // 立即刷新畫面
        }
        else
        {
            Debug.LogWarning($"任務 [{quest.name}] 無法領獎，可能尚未完成。");
        }
    }



    void UpdateQuestSlots()
    {
        for (int i = 0; i < questSlots.Count; i++)
        {
            int questIndex = currentIndex + i;

            if (questIndex >= sortedQuests.Count)
            {
                questSlots[i].rootObject.SetActive(false);
                continue;
            }

            var quest = sortedQuests[questIndex];
            var slot = questSlots[i];

            slot.rootObject.SetActive(true);
            slot.goalText.text = quest.goalText;
            slot.rewardText.text = quest.rewardDescription;
            slot.rewardIcon.sprite = quest.rewardIcon;

            slot.rewardButton.onClick.RemoveAllListeners();

            if (quest.questLogic is IQuestLogic logic)
            {
                bool isComplete = logic.IsComplete();
                quest.isCompleted = isComplete;

                if (isComplete && !quest.rewardClaimed)
                {
                    slot.goalText.text += "（已完成）";
                    slot.rewardText.text += " 點擊圖標領取獎勵";
                    slot.rewardIcon.sprite = completedRewardIcon;

                    int localIndex = questIndex;
                    slot.rewardButton.onClick.RemoveAllListeners(); // 確保清除先前事件
                    slot.rewardButton.onClick.AddListener(() => GiveReward(localIndex));
                }
            }
        }

    }




}
