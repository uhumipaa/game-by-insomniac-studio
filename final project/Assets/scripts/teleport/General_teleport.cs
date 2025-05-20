using UnityEngine;
using UnityEngine.Rendering;

public class General_teleport : MonoBehaviour
{
    private TowerManager tower;
    private Maploaders maploader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maploader = FindAnyObjectByType<Maploaders>();
        tower = FindAnyObjectByType<TowerManager>();
        if (tower == null)
        {
            Debug.LogError("tower �䤣��I");
        }
        if (maploader == null)
        {
            Debug.LogError("MapLoader �䤣��I");
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
        if (TowerManager.Instance.currentTowerFloor % 10 == 0)//boss
        {

        }
        else if (TowerManager.Instance.currentTowerFloor % 10 == 5)//rest
        {

        }
        else
        {
            int roomVariant = Random.Range(0, 4);
            int floor = TowerManager.Instance.currentTowerFloor;
            if (floor % 10 == 1)
            {
                maploader.changemap(floor / 10);
            }
            maploader.LoadMaps(floor/10,roomVariant);
        }
    }
}
