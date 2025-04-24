using UnityEngine;

public class General_teleport : MonoBehaviour
{
    private TowerManager tower;
    private Maploader maploader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maploader = FindAnyObjectByType<Maploader>();
        tower = FindAnyObjectByType<TowerManager>();
        if (tower == null)
        {
            Debug.LogError("tower 找不到！");
        }
        if (maploader == null)
        {
            Debug.LogError("MapLoader 找不到！");
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nextfloar();
        }
    }
    void nextfloar()
    {
        Debug.Log("floor:" + TowerManager.Instance.currentTowerFloor);
        TowerManager.Instance.currentTowerFloor++;
        Debug.Log("floor:" + TowerManager.Instance.currentTowerFloor);
        int roomVariant = Random.Range(0, 4);
        int floor = TowerManager.Instance.currentTowerFloor;
        floor = (floor - 1) / 10 * 4 + roomVariant;
        maploader.LoadMap(floor);
    }
}
