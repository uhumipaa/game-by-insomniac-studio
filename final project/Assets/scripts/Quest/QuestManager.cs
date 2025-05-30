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
        currentIndex++;

        // 最多只顯示到倒數第三筆
        if (currentIndex + 2 >= sortedQuests.Count)
        {
            currentIndex = 0;
        }

        UpdateQuestSlots();
    }
    void GiveReward(int questIndex)
    {
        if (questIndex < 0 || questIndex >= sortedQuests.Count)
            return;

        QuestData quest = sortedQuests[questIndex];

        if (quest.questLogic is IQuestLogic logic && logic.IsComplete() && !quest.rewardClaimed)
        {
            logic.GiveReward();
            quest.rewardClaimed = true;
            RefreshQuests();
        }
    }


    void UpdateQuestSlots()
    {
        for (int i = 0; i < questSlots.Count; i++)
        {
            if (i + currentIndex >= sortedQuests.Count)
            {
                questSlots[i].rootObject.SetActive(false);
                continue;
            }

            QuestData quest = sortedQuests[i + currentIndex];
            var slot = questSlots[i];

            slot.rootObject.SetActive(true);

            // 重設預設內容
            slot.goalText.text = quest.goalText;
            slot.rewardText.text = quest.rewardDescription;
            slot.rewardIcon.sprite = quest.rewardIcon;
            slot.rewardIcon.GetComponent<Button>().onClick.RemoveAllListeners(); // 保險清除

            bool isComplete = false;

            if (quest.questLogic is IQuestLogic logic)
            {
                try
                {
                    isComplete = logic.IsComplete();
                }
                catch
                {
                    Debug.LogWarning($"任務 {quest.name} 的 IsComplete() 發生錯誤，請檢查邏輯");
                }

                quest.isCompleted = isComplete;

                if (isComplete && !quest.rewardClaimed)
                {
                    slot.goalText.text += "（已完成）";
                    slot.rewardText.text += " 點擊圖標領取獎勵";
                    slot.rewardIcon.sprite = completedRewardIcon;

                    int questIndex = i + currentIndex;
                    slot.rewardIcon.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GiveReward(questIndex);
                    });
                }
            }
            else
            {
                Debug.LogWarning($"任務 {quest.name} 沒有指派 questLogic，請檢查 QuestData");
            }
        }
    }



}
