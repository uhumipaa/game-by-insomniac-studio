using UnityEngine;

public class General_transport_controller : MonoBehaviour
{
    private Towermanager tower;
    private Maploader maploader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maploader = FindAnyObjectByType<Maploader>();
        tower = FindAnyObjectByType<Towermanager>();
        if(tower == null)
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
        if (collision.CompareTag("Player")){
            nextfloar();
        }
    }
    void nextfloar()
    {
        Debug.Log("floor:" + Towermanager.Instance.currentTowerFloor);
        Towermanager.Instance.currentTowerFloor++;
        Debug.Log("floor:" + Towermanager.Instance.currentTowerFloor);
        int roomVariant = Random.Range(0, 4);
        int floor = Towermanager.Instance.currentTowerFloor;
        floor = (floor - 1) / 10*4 + roomVariant;
        maploader.LoadMap(floor);
    }
}
