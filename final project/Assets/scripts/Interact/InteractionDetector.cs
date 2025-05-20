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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out interactable Interactable) && Interactable.CanInteract())
        {
            interactableInRange = Interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.TryGetComponent(out interactable Interactable) && (Interactable == interactableInRange))
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}
