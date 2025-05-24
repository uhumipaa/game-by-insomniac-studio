using UnityEngine;

public class FishingTrigger : MonoBehaviour
{
    public GameObject fishingAskPanel;
    public bool isFishing = false; // 狀態鎖

    private bool isPlayerInZone = false;

    void Update()
    {
        if (isPlayerInZone && !isFishing && Input.GetKeyDown(KeyCode.F))
        {
            fishingAskPanel.SetActive(true);
            fishingAskPanel.transform.Find("StartFishing?Text").gameObject.SetActive(true);
            fishingAskPanel.transform.Find("YesButton").gameObject.SetActive(true);
            fishingAskPanel.transform.Find("NoButton").gameObject.SetActive(true);
        }
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
