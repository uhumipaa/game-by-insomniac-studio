using UnityEngine;

public class Mapmanager : MonoBehaviour
{
    private Maploaders loader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        loader = FindAnyObjectByType<Maploaders>();
        loader.LoadMaps(TowerManager.Instance.currentTowerFloor/10,Random.Range(0,4)); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
