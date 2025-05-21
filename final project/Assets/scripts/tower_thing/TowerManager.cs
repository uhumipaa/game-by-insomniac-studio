using UnityEngine;
using UnityEngine.SceneManagement;
public class TowerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static TowerManager Instance { get; set; }
    public int currentTowerFloor = 1;
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

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void retrun_to_town()
    {
        SceneManager.LoadScene("farm");
    }
}
