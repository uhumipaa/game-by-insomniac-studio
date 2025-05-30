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
    public FloorData[] boosfloordata;
    public GameObject backteleport;
    private GameObject backteleport_instance;
    public GameObject shopsystem;
    public GameObject audio_managerPrefab;
    public Audio_manager audio_manager;
    string audioname;
    int audioindex;
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
                // nowfloor = Random.Range(0, floorDatas.Length);
                nowfloor = TowerManager.Instance.currentTowerFloor / 10;
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
        playbgm();
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
        TowerManager.Instance.currentfloorprefab = i;
        mapPrefabs = floorDatas[i].maps;
    }

    public void playbgm()
    {
        int lastaudio = audioindex;
        audioname = findname();
        audioindex = findindex();
        Debug.Log($"Clip Name: {audioname} | Index: {audioindex}");
        Audio_manager.Instance.Play(audioindex, audioname, true,lastaudio);
        //audio_manager.Play(index, name, true);
    }
    string findname()
    {
        bool boss = false;
        if (TowerManager.Instance.currentTowerFloor%10 == 0)
        {
            boss = true;
        } else boss = false;
        if (TowerManager.Instance.currentTowerFloor % 10 == 5)
        {
            return "restmap_bgm";
        }
        switch (TowerManager.Instance.currentfloorprefab, boss)
        {
            case (0, false):
                return "map1map10_bgm";
            case (0, true):
                return "Boss_king_bgm";

            case (1, false):
                return "map11map20_bgm";
            case (1, true):
                return "Boss_Dark_Magicion_bgm";

            case (2, false):
                return "map21map30_bgm";
            case (2, true):
                return "Boss_warrior_bgm";

            case (3, false):
                return "map31map40_bgm";
            case (3, true):
                return "Boss_dino_bgm";

            case (4, false):
                return "map41map50_bgm";
            case (4, true):
                return "finalboss_bgm";

            default:
                return "initial_bgm";

        }
    }

    int findindex()
    {        
        int boss;
        if (TowerManager.Instance.currentTowerFloor%10 == 0)
        {
            boss = 1;
        }
        else boss = 0;
        
        
        return 2 * TowerManager.Instance.currentfloorprefab + boss + 1;
        
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
        playbgm();

    }
    public void loadBossmap(int judge)
    {
        playbgm();
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
        mapPrefabs = boosfloordata[judge].maps;
        currentMap = Instantiate(boosfloordata[judge].maps[0],  Vector2.zero, Quaternion.identity, mapParen);
        Generate_player();
        SpawnMonsters(boosfloordata[judge]);
        shopsystem.SetActive(false);
    }
}
