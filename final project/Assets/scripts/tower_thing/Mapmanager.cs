using UnityEngine;

public class Mapmanager : MonoBehaviour
{
    private Maploaders loader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        loader = FindAnyObjectByType<Maploaders>();
        if (TowerManager.Instance.currentTowerFloor % 10 == 5)
        {
            loader.changemap(Random.Range(0, 5));
            loader.loadrestmap();
        }
        else
        {
            loader.LoadMaps((int)Random.Range(0,4));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
