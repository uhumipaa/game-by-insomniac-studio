using UnityEngine;
using System.Collections;


public class FishingTrigger : MonoBehaviour
{
    public GameObject fishingAskPanel;
    public FishingUIManager fishingUIManager;
    public ItemData fishingFeelItem; // 檢查道具
    public bool isFishing = false; // 狀態鎖

    private bool isPlayerInZone = false;

void Start()
    {
        InventoryManager.Instance.Add("Backpack", fishingFeelItem, 1);
    }
    void Update()
    {
        if (isPlayerInZone && !isFishing && Input.GetKeyDown(KeyCode.F))
        {
            fishingAskPanel.SetActive(true);

            

            bool hasFishingFeel = InventoryManager.Instance.Contains("Backpack", fishingFeelItem);
            if (hasFishingFeel)
            {
                fishingAskPanel.transform.Find("StartFishing?Text").gameObject.SetActive(true);
                fishingAskPanel.transform.Find("YesButton").gameObject.SetActive(true);
                fishingAskPanel.transform.Find("NoButton").gameObject.SetActive(true);
                fishingAskPanel.transform.Find("NoFishingFeelText")?.gameObject.SetActive(false); // 若有提示文字就隱藏
                fishingAskPanel.transform.Find("NoFishingFeelPic")?.gameObject.SetActive(false);
            }
            else
            {
                // 顯示提示語句：沒有手感
                fishingAskPanel.transform.Find("NoFishingFeelText")?.gameObject.SetActive(true);
                fishingAskPanel.transform.Find("NoFishingFeelPic")?.gameObject.SetActive(true);
                fishingAskPanel.transform.Find("StartFishing?Text")?.gameObject.SetActive(false);
                fishingAskPanel.transform.Find("YesButton")?.gameObject.SetActive(false);
                fishingAskPanel.transform.Find("NoButton")?.gameObject.SetActive(false);

                // ✅ 顯示後 2.5 秒自動關閉
                StartCoroutine(HideNoFeelUIAfterDelay(2.5f));
            }
        }
    }

    private IEnumerator HideNoFeelUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        fishingAskPanel.SetActive(false);
        fishingAskPanel.transform.Find("NoFishingFeelText")?.gameObject.SetActive(false);
        fishingAskPanel.transform.Find("NoFishingFeelPic")?.gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}
