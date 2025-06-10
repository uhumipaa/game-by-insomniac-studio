using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private interactable interactableInRange = null;

    private void Update()
    {
        // 檢查是否按下 E 鍵和偵測物有沒有進入範圍
        if (Keyboard.current.eKey.wasPressedThisFrame && interactableInRange != null)
        {
            Debug.Log("按下 E 鍵，觸發互動！");
            interactableInRange.Interact();
        }
    }

    //偵測物進入範圍
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out interactable target) && target.CanInteract())
        {
            interactableInRange = target;

        }
    }

    //偵測物退出範圍
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out interactable target) && (target == interactableInRange))
        {
            interactableInRange = null;
        }
    }
}
