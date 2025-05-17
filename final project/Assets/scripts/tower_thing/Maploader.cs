using Unity.VisualScripting;
using UnityEngine;

public class Maploader : MonoBehaviour
{
    public GameObject player;
    public GameObject chest;
    private GameObject player_instance;
    public GameObject transcircle;
    private GameObject transcircle_instance;
    private GameObject chest_instance;
    private GameObject currentMap;
    public GameObject[] mapPrefabs;
    public Transform mapParent;
    public FloorData[] floorDatas;
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
        if (player_instance == null)
        {
            player_instance = Instantiate(player, playerspawn.position, Quaternion.identity);
        }
        else
        {
            player_instance.transform.position = playerspawn.position;
        }

    }
    // Update is called once per frame
    public void LoadMap(int index)
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
        currentMap = Instantiate(mapPrefabs[index], Vector3.zero, Quaternion.identity, mapParent);
        Generate_player();
        FloorData floorData = floorDatas[index];
        SpawnMonsters(floorData);
    }

    Vector2 getrandomposition(FloorData floorData)
    {
        Vector2 pos;
        pos.x = Random.Range(floorData.spwan_range_min.x, floorData.spawn_range_max.x);
        pos.y = Random.Range(floorData.spwan_range_min.y, floorData.spawn_range_max.y);
        return pos;
    }

    void SpawnMonsters(FloorData floorData)
    {
        int count = 0;
        for (int i = 0; i < floorData.enemycount; i++)
        {
            if (i <4 )
            {
                int rmd = Random.Range(1, 4);
                if (rmd % 3==0)
                {
                    continue;
                }
            }
            int level = Random.Range((TowerManager.Instance.currentTowerFloor%5) + 2 - floorData.levelrange, (TowerManager.Instance.currentTowerFloor % 5) + 2 - floorData.levelrange);
            if (count < 3)
            {
                if (count < 2)
                {
                    level += 2;
                }else
                {
                    level += 1;
                }
            }
            EnemyData monsterPrefab = Getrandomenemy(floorData);
            Vector2 spawnPos = getrandomposition(floorData);
            Instantiate(monsterPrefab.enemyprefab, spawnPos, Quaternion.identity).GetComponent<enemy_property>()?.generaterandomstatus(monsterPrefab,level);
            count++;
        }
    }

    EnemyData Getrandomenemy(FloorData floorData)
    {
        int i = Random.Range(0, floorData.enemyprefab.Length);

        return floorData.enemyprefab[i];
    }
}
