using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Maploaders : MonoBehaviour
{
    public GameObject player;
    public GameObject chest;
    int nowfloor;
    [SerializeField] GameObject player_instance;
    public GameObject transcircle;
    private GameObject transcircle_instance;
    private GameObject chest_instance;
    private GameObject currentMap;
    public GameObject[] mapPrefabs;
    public Transform mapParen;
    public FloorData[] floorDatas;
    public FloorData restdata;
    public FloorData boosfloordata;
    public GameObject backteleport;
    private GameObject backteleport_instance;
    public GameObject shopsystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void generate_tele()
    {
        Transform Telespawn = GameObject.Find("trans_spawn_point")?.transform;
        if (Telespawn == null)
        {
            Debug.Log("123");
        }
        transcircle_instance = Instantiate(transcircle, Telespawn.position, Quaternion.identity);
    }

    public void generate_chest()
    {
        Transform chestpawn = GameObject.Find("chest_spawn_point")?.transform;
        if (chestpawn == null)
        {
            Debug.Log("123");
        }
        chest_instance = Instantiate(chest, chestpawn.position, Quaternion.identity);
    }
    void Generate_player()
    {
        Transform playerspawn = GameObject.Find("player_spawn_point")?.transform;
        if (playerspawn == null)
    {
        Debug.LogError("找不到 player_spawn_point");
        return;
    }
        if (player_instance == null)
        {
            player_instance = FindAnyObjectByType<player_controler>().gameObject;
            if (player_instance == null)
            {
                player_instance = Instantiate(player, playerspawn);
            }
        }
        
            player_instance.transform.position = playerspawn.position;
    }
    // Update is called once per frame
    public void LoadMaps(int room)
    {

        if (TowerManager.Instance.currentTowerFloor % 10 == 1)
        {
            if (TowerManager.Instance.currentTowerFloor == 1)
            {
                nowfloor = 0;
            }
            else
            {
                nowfloor = Random.Range(0, floorDatas.Length);
            }
            changemap(nowfloor);
        } 
        shopsystem.SetActive(false);
        if (backteleport_instance != null)
        {
            Destroy(backteleport_instance);
        }
        if (currentMap != null)
            Destroy(currentMap);
        if (transcircle_instance != null)
        {
            Destroy(transcircle_instance);
        }
        if (chest_instance != null)
        {
            Destroy(chest_instance);
        }
        currentMap = Instantiate(mapPrefabs[room], Vector3.zero, Quaternion.identity, mapParen);
        Generate_player();
        SpawnMonsters(floorDatas[nowfloor]);
    }

    Vector2 getrandomposition(FloorData floorData)
    {
        Vector2 pos;
        pos.x = Random.Range(floorData.spwan_range_min.x, floorData.spawn_range_max.x);
        pos.y = Random.Range(floorData.spwan_range_min.y, floorData.spawn_range_max.y);
        return pos;
    }
    public void changemap(int i)
    {
        mapPrefabs = floorDatas[i].maps;
    }

    void SpawnMonsters(FloorData floorData)
    {
        int count = 0;
        for (int i = 0; i < floorData.enemycount; i++)
        {
            if (i < floorData.enemycount - 1)
            {
                int rmd = Random.Range(1, 4);
                if (rmd % 3 == 0)
                {
                    continue;
                }
            }
            int level = Random.Range((TowerManager.Instance.currentTowerFloor % 3) + 4 - floorData.levelrange, (TowerManager.Instance.currentTowerFloor % 3) + 4 - floorData.levelrange);
            if (count < floorData.enemycount - 2)
            {
                if (count < floorData.enemycount - 3)
                {
                    level += 2;
                }
                else
                {
                    level += 1;
                }
            }
            EnemyData monsterPrefab = Getrandomenemy(floorData);
            Vector2 spawnPos = getrandomposition(floorData);
            Instantiate(monsterPrefab.enemyprefab, spawnPos, Quaternion.identity).GetComponent<enemy_property>()?.generaterandomstatus(monsterPrefab, level);
            count++;
        }
    }

    EnemyData Getrandomenemy(FloorData floorData)
    {
        int i = Random.Range(0, floorData.enemyprefab.Length);

        return floorData.enemyprefab[i];
    }
    public void loadrestmap()
    {
        if (currentMap != null)
            Destroy(currentMap);
        if (transcircle_instance != null)
        {
            Destroy(transcircle_instance);
        }
        if (chest_instance != null)
        {
            Destroy(chest_instance);
        }
        currentMap = Instantiate(restdata.maps[0], Vector2.zero, Quaternion.identity, mapParen);
        Generate_player();
        generate_tele();
        Transform Telespawn = GameObject.Find("backtele_spawn_point")?.transform;
        if (Telespawn == null)
        {
            Debug.Log("123");
        }
        backteleport_instance = Instantiate(backteleport, Telespawn.position, Quaternion.identity);
        shopsystem.SetActive(true);
        GetComponent<SetSpecialMap>().setrest();
    }
}
