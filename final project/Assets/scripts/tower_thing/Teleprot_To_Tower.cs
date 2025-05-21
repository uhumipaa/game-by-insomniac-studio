using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleprot_To_Tower : MonoBehaviour
{
    private Maploaders loader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space)) // Enter鍵
        {
            SceneManager.LoadScene("tower");  
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
