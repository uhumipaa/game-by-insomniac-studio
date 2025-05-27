using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToFarm : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        //主角進入範圍並按下enter
        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("farm"); //進入playerHome
        }      
    }

    //player接近
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
                playerInRange = true;
        }
    }
        
    //player遠離
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
                playerInRange = false;
        }   
    }  

    //按下home回到農場
    public void bottonToFarm()
    {
        SceneManager.LoadScene("farm");
    }
}
