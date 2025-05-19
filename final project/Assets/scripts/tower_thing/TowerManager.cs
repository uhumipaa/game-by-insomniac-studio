using UnityEngine;

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

    private void Start()
    {
        loader = FindAnyObjectByType<Maploaders>();
        loader.LoadMaps(currentTowerFloor,0);
    }
    void retrun_to_tower()
    {

    }
    // Update is called once per frame
    void retrun_to_town()
    {

    }
}
