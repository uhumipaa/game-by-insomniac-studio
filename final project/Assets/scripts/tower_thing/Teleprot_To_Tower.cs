using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleprot_To_Tower : MonoBehaviour
{
    public GameObject towerEnterPanel;
    public GameObject floorChoicePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] bool playerInRange = false;


    void Start()
    {
        towerEnterPanel.SetActive(false);
        floorChoicePanel.SetActive(false);
        
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space)) // Enter鍵
        {
            OpenEnterPanel();
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


    public void OpenEnterPanel()
    {
        towerEnterPanel.SetActive(true);
    }

    public void OnClickEnterYes()
    {
        towerEnterPanel.SetActive(false);
        floorChoicePanel.SetActive(true);
    }

    public void OnClickEnterNo()
    {
        towerEnterPanel.SetActive(false);
    }

    public void OnClickStartFromBeginning()
    {
        TowerManager.Instance.currentTowerFloor= 1;
        SceneManager.LoadScene("TowerScene");
    }

    public void OnClickStartFromMiddle()
    {
        for (int i = 0; i <10; i++)
        {
            if (TowerManager.Instance.currentTowerFloor % 10 == 5)
            {
                break;
            }
            TowerManager.Instance.currentTowerFloor--;
        }
            // 比如從第 11 層開始
            SceneManager.LoadScene("TowerScene");
    }

}
