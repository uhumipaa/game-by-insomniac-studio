using UnityEngine;

public class FishingSignClick : MonoBehaviour
{
    public GameObject fishingAskPanel;
    public FishingTrigger fishingTrigger; // 控制鎖定狀態

    private void OnMouseDown()
    {
        if (fishingTrigger != null && fishingTrigger.isFishing)
        {
            // 已經在釣魚中就不再觸發
            return;
        }

        if (fishingAskPanel != null)
        {
            fishingAskPanel.SetActive(true);
            fishingAskPanel.transform.Find("StartFishing?Text").gameObject.SetActive(true);
            fishingAskPanel.transform.Find("YesButton").gameObject.SetActive(true);
            fishingAskPanel.transform.Find("NoButton").gameObject.SetActive(true);

        }
    }
}
