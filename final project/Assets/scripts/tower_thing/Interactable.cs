using UnityEngine;

public class Interactable : MonoBehaviour
{
    // �T�O���r�������T�GMonoBehaviour�A���n������ MonoBehavior �Ψ�L
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
