using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private interactable interactableInRange = null;

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
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.TryGetComponent(out interactable target) && (target == interactableInRange))
        {
            interactableInRange = null;
           
        }
    }
}
