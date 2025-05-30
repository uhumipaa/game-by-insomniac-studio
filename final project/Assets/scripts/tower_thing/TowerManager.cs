using UnityEngine;
using UnityEngine.SceneManagement;
public class TowerManager : MonoBehaviour,ISaveData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static TowerManager Instance { get; set; }
    public bool backtotower;
    public int currentTowerFloor = 1;
    public int finishfloorthistime = 1;
    public int currentfloorprefab=0;
    private Maploaders loader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        finishfloorthistime = currentTowerFloor;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void retrun_to_town()
    {
        SceneManager.LoadScene("farm");

        FarmManager.instance?.AutoGrowAllTiles(); // 自動成長
    }

    public void SaveData(ref SaveData saveData)
    {
        saveData.currentfloor = currentTowerFloor;
    }

    public void LoadData(SaveData saveData)
    {
        currentTowerFloor = saveData.currentfloor;
    }

}
