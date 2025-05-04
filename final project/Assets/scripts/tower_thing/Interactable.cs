using UnityEngine;

public class Interactable : MonoBehaviour
{
    // 確保拼字完全正確：MonoBehaviour，不要打錯成 MonoBehavior 或其他
    public string interactionText = "This is empty";

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(interactionText);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
