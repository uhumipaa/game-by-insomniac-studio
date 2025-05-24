using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingResultUIManager : MonoBehaviour
{
    public GameObject fishingResultPanel;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI fixedMessageText;

    private bool isShowing = false;

    void Update()
    {
        if (isShowing && Input.GetMouseButtonDown(0))
        {
            fishingResultPanel.SetActive(false);
            isShowing = false;
        }
    }

    public void ShowResult(ItemData item)
    {
        fishingResultPanel.SetActive(true);
        isShowing = true;

        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        fixedMessageText.text = "-點擊畫面任意處關閉視窗-";
    }
}
