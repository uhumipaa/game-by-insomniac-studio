using UnityEngine;

public class NPCInteractionIcon : MonoBehaviour
{
    public GameObject interactionIcon; // 星星圖示
    public string playerTag = "Player"; // 主角Tag"Player"

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false); // 一開始關閉圖示
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (interactionIcon != null)
                interactionIcon.SetActive(true); // 主角進入，顯示提示
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (interactionIcon != null)
                interactionIcon.SetActive(false); // 離開，關閉提示
        }
    }
}
