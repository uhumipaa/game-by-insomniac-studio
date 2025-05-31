using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTrigger : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {

        if (playerInRange)
        {
            Debug.Log("Player is in range");
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.Return)) // Enter鍵
        {
            Debug.Log("Enter pressed - Loading shop scene");
            FarmManager.instance.SaveFarmTilesToFile();//儲存田地狀態
            SceneManager.LoadScene("shop"); // 商店場景名稱
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
    public void BackToFarm()
    {
        SceneManager.LoadScene("farm");
    }
}
