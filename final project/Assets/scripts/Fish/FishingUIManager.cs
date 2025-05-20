using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingUIManager : MonoBehaviour
{
    // 釣魚結果面板
    public GameObject fishingResultPanel;
    public Image resultImage;
    public Image resultNameImage;
    public Image resultDescriptionImage;
    public Image fixedMessageImage;

    // 可釣物品清單
    public List<FishingItem> possibleItems;
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
        // 一開始就確保等待用的UI是隱藏的
        waitingFishPic.SetActive(false);
        waitingFishText.SetActive(false);
    }

    private IEnumerator ShowFishingResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 隱藏等待動畫
        waitingFishPic.SetActive(false);
        waitingFishText.SetActive(false);

        // 顯示結果面板
        fishingResultPanel.SetActive(true);

        // 隨機選一個物品
        int index = Random.Range(0, possibleItems.Count);
        FishingItem item = possibleItems[index];

        // 設定 UI 圖片
        resultImage.sprite = item.itemSprite;
        resultNameImage.sprite = item.nameSprite;
        resultDescriptionImage.sprite = item.descriptionSprite;

        // 顯示固定訊息圖片
        fixedMessageImage.gameObject.SetActive(true);

        Debug.Log("你釣到：" + item.nameSprite.name);

        // （第3步可在這裡加“點一下結束”的邏輯）
    }

    public void OnClickYes()
    {
        Debug.Log("開始釣魚！");

        // 設定釣魚狀態（鎖住F鍵觸發）
        fishingTrigger.isFishing = true;

        // 隱藏原本對話內容
        startFishingText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        // 顯示等待魚咬鉤畫面
        waitingFishPic.SetActive(true);
        waitingFishText.SetActive(true);

        // ➔ 這裡可以開始釣魚邏輯（例如等3秒後釣到魚）
        StartCoroutine(ShowFishingResultAfterDelay(7f));

    }



    public void OnClickNo()
    {
        Debug.Log("取消釣魚。");
        fishingAskPanel.SetActive(false);
    }
}
