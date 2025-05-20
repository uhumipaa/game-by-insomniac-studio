using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleprot_To_Tower : MonoBehaviour
{
    private Maploaders loader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Return)) // Enter鍵
        {
            SceneManager.LoadScene("tower");
            loader.LoadMaps(TowerManager.Instance.currentTowerFloor/10,Random.Range(0,4)); 
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
