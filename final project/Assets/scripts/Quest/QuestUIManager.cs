using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public GameObject questPanel;
    public Image questButtonImage; // 指向按鈕的 Image 組件
    public Sprite defaultSprite;   // 預設圖示
    public Sprite activeSprite;    // 啟用時圖示

    public QuestManager questManager; // ⭐ 加入這一行

    private bool isPanelOpen = false;

    public void ToggleQuestPanel()
    {
        isPanelOpen = !isPanelOpen;
        questPanel.SetActive(isPanelOpen);

        // 切換按鈕圖片
        if (questButtonImage != null)
        {
            questButtonImage.sprite = isPanelOpen ? activeSprite : defaultSprite;
        }

        // ⭐ 當面板開啟時刷新任務內容
        if (isPanelOpen && questManager != null)
        {
            questManager.RefreshQuests();
        }
    }
}
