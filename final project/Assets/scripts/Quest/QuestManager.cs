using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    private int currentIndex = 0;

    void Start()
    {
        foreach (var slot in questSlots)
        {
            slot.nextButton.onClick.AddListener(OnNextClicked);
        }
    }

    public void RefreshQuests()
    {
        UpdateQuestSlots();
    }

    void OnNextClicked()
    {
        currentIndex++;

        // 最多只顯示到倒數第三筆
        if (currentIndex + 2 >= allQuests.Count)
        {
            currentIndex = 0;
        }

        UpdateQuestSlots();
    }

    void UpdateQuestSlots()
    {
        for (int i = 0; i < questSlots.Count; i++)
        {
            int questIdx = currentIndex + i;

            if (questIdx < allQuests.Count)
            {
                var data = allQuests[questIdx];
                var slot = questSlots[i];

                slot.rootObject.SetActive(true);
                slot.rewardIcon.sprite = data.rewardIcon;
                slot.goalText.text = data.goalText;
                slot.rewardText.text = data.rewardDescription;
            }
            else
            {
                questSlots[i].rootObject.SetActive(false);
            }
        }
    }
}
