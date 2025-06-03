using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Teleprot_To_Tower : MonoBehaviour
{
    public GameObject towerEnterPanel;
    public GameObject floorChoicePanel;
    public TMP_Text currentFloorText;
    private Player_Property player_Property;
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
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player_Property = playerGO.GetComponent<Player_Property>();
            player_Property.current_health = player_Property.max_health;
        }
        if (TowerManager.Instance.currentTowerFloor < 5)
        {
            TowerManager.Instance.currentTowerFloor = 1;

            FarmManager.instance.SaveFarmTilesToFile();//儲存田地狀態
            Audio_manager.Instance.Stop();
            SceneManager.LoadScene("tower");
        }
        else
        {
            int max = TowerManager.Instance.currentTowerFloor;
            currentFloorText.text = $" {max} ";
            floorChoicePanel.SetActive(true);
        }
    }

    public void OnClickEnterNo()
    {
        towerEnterPanel.SetActive(false);
    }

    public void OnClickStartFromBeginning()
    {
        TowerManager.Instance.currentTowerFloor= 1;
        TowerManager.Instance.backtotower = true;

        FarmManager.instance.SaveFarmTilesToFile();//儲存田地狀態
        Audio_manager.Instance.Stop();
        SceneManager.LoadScene("tower");
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
            FarmManager.instance.SaveFarmTilesToFile();//儲存田地狀態
        Audio_manager.Instance.Stop();
        SceneManager.LoadScene("tower");
    }

}
