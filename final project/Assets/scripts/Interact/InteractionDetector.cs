using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private interactable interactableInRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        //一開始先關閉圖示
        interactionIcon.SetActive(false);
    }

    private void Update()
    {
        // 檢查是否按下 E 鍵
        if (Keyboard.current.eKey.wasPressedThisFrame && interactableInRange != null)
        {
            Debug.Log("按下 E 鍵，觸發互動！");
            interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out interactable target) && target.CanInteract())
        {
            interactableInRange = target;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.TryGetComponent(out interactable target) && (target == interactableInRange))
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}
