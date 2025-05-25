using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingUIManager : MonoBehaviour
{
    // 釣魚結果面板
    public GameObject fishingResultPanel;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI fixedMessageText;

    private bool isShowing = false;

    // 可釣物品清單（ItemData）
    public List<ItemData> possibleItems;

    public GameObject fishingAskPanel;

    // 原本的UI元件
    public GameObject startFishingText;
    public GameObject yesButton;
    public GameObject noButton;

    // 等待魚咬鉤階段用的UI
    public GameObject waitingFishPic;
    public GameObject waitingFishText;

    // 連接FishingTrigger腳本來控制isFishing狀態
    public FishingTrigger fishingTrigger;

    void Start()
    {

        fishingAskPanel.SetActive(false);
        startFishingText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        waitingFishPic.SetActive(false);
        waitingFishText.SetActive(false);
        fishingResultPanel.SetActive(false);
    }

    void Update()
    {
        if (isShowing && Input.GetMouseButtonDown(0))
        {
            fishingResultPanel.SetActive(false);
            isShowing = false;
            fishingTrigger.isFishing = false;
        }
    }

    public void OnClickYes()
    {
        Debug.Log("開始釣魚！");
        fishingTrigger.isFishing = true;

        startFishingText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        waitingFishPic.SetActive(true);
        waitingFishText.SetActive(true);

        StartCoroutine(ShowFishingResultAfterDelay(5f));
    }

    public void OnClickNo()
    {
        Debug.Log("取消釣魚。");
        fishingAskPanel.SetActive(false);
    }

    private IEnumerator ShowFishingResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        waitingFishPic.SetActive(false);
        waitingFishText.SetActive(false);

        fishingAskPanel.SetActive(false); // 把問答 UI 關掉

        // 隨機選擇一個釣到的物品
        int index = Random.Range(0, possibleItems.Count);
        ItemData item = possibleItems[index];

        // 顯示結果
        fishingResultPanel.SetActive(true);
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        fixedMessageText.text = "-點擊畫面任意處關閉視窗-";

        isShowing = true;
        Debug.Log("你釣到：" + item.itemName);

        InventoryManager.Instance.Add("Backpack", item, 1);
        CoinManager.instance.SpendCoins(20);
    }
}
