using UnityEngine;
using UnityEngine.SceneManagement;

public class playerHouseTrigger : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        //主角進入範圍並按下enter
        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("playerHome"); //進入playerHome
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
