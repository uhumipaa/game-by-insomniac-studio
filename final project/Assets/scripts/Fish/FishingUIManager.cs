using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class FishingUIManager : MonoBehaviour
{
    // Singleton
    public static FishingUIManager Instance { get; private set; }

    // 統計用變數
    public int TotalFishingAttempts => FishingStats.TotalFishingAttempts;
    public int DebrisCaughtCount => FishingStats.DebrisCaughtCount;

    // 釣魚結果面板
    public GameObject fishingResultPanel;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI fixedMessageText;



    private bool isShowing = false; // 釣魚結果
    public bool fromTrigger = false; // 從進入釣魚的途徑判定後續是否扣除手感
    public float duration = 5f;
    public bool fishingcheat = false;



    // 可釣物品清單（ItemData）
    public List<ItemData> possibleItems;

    public GameObject fishingAskPanel;

    //詢問是否釣魚的UI元件
    public GameObject startFishingText;
    public GameObject yesButton;
    public GameObject noButton;

    // 等待魚咬鉤階段用的UI
    public GameObject waitingFishPic;
    public GameObject waitingFishText;

    // 連接FishingTrigger腳本來控制isFishing狀態
    public FishingTrigger fishingTrigger;

    private void Awake()
    {
        // 設定 Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        // 關閉結果面板
        if (isShowing && Input.GetMouseButtonDown(0))
        {
            fishingResultPanel.SetActive(false);
            isShowing = false;
            fishingTrigger.isFishing = false;
        }
    }
    public void Fishgoldfinger()
    {
        fishingcheat = true;
    }

    public void OnClickYes()
    {
        if (fromTrigger)
        {
            // 扣掉一個 Fishing_feel
            if (InventoryManager.Instance.Contains("Backpack", fishingTrigger.fishingFeelItem))
            {
                InventoryManager.Instance.TryRemove("Backpack", fishingTrigger.fishingFeelItem, 1);
            }
        }


        Debug.Log("開始釣魚！");
        fishingTrigger.isFishing = true;

        startFishingText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        waitingFishPic.SetActive(true);
        waitingFishText.SetActive(true);

        StartCoroutine(ShowFishingResultAfterDelay(duration));
    }

    public void OnClickNo()
    {
        Debug.Log("取消釣魚。");
        fishingAskPanel.SetActive(false);
    }

    private IEnumerator ShowFishingResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        FishingStats.TotalFishingAttempts++; // 統計總釣魚次數

        waitingFishPic.SetActive(false);
        waitingFishText.SetActive(false);
        fishingAskPanel.SetActive(false);

        // 隨機選擇一個釣到的物品
        int index = Random.Range(0, possibleItems.Count);
        ItemData item = possibleItems[index];

        // gold finger
        if (fishingcheat)
        {
            // 只從這些選項中選一個
            string[] forcedNames = new string[] { "鯉魚王", "暴鯉龍", "鋼彈" };

            //itemName 對應是唯一的
            var forcedItems = possibleItems.Where(i => forcedNames.Contains(i.itemName)).ToList();

            if (forcedItems.Count > 0)
            {
                index = Random.Range(0, forcedItems.Count);
                item = forcedItems[index];
            }
            else
            {
                Debug.LogWarning("❗ 強制釣魚找不到指定物品，改為正常隨機");
                index = Random.Range(0, possibleItems.Count);
                item = possibleItems[index];
            }
        }

        // 統計是否釣到汙染物
        if (item.itemName == "塑膠袋" || item.itemName == "漂流瓶" || item.itemName == "鋼彈" || item.itemName == "金幣")
        {
            FishingStats.DebrisCaughtCount++;
        }

        // 顯示結果
        fishingResultPanel.SetActive(true);
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        fixedMessageText.text = "-點擊畫面任意處關閉視窗-";

        isShowing = true;
        Debug.Log("你釣到：" + item.itemName);
        Debug.Log($"目前總釣魚次數：{FishingStats.TotalFishingAttempts}，汙染物次數：{FishingStats.DebrisCaughtCount}");

        InventoryManager.Instance.Add("Backpack", item, 1);

    }
}
