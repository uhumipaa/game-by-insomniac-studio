using System;
using UnityEngine;

public class Maploader : MonoBehaviour
{
    public GameObject player;
    public GameObject player_instance;
    public GameObject transcircle;
    public GameObject transcircle_instance;
    private GameObject currentMap;
    public GameObject[] mapPrefabs;
    public Transform mapParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    
    public void LoadMap(int index)
    {
        if (currentMap != null)
            Destroy(currentMap);
        if (transcircle_instance != null)
        {
            Destroy(transcircle_instance);
        }
        currentMap = Instantiate(mapPrefabs[index], Vector3.zero, Quaternion.identity, mapParent);
        Transform Telespawn = GameObject.Find("trans_spawn_point")?.transform;
        Transform playerspawn = GameObject.Find("player_spawn_point")?.transform;
        if (player_instance == null)
        {
            player_instance = Instantiate(player, playerspawn.position, Quaternion.identity);
        }
        else
        {
            player_instance.transform.position = playerspawn.position;
        }
        transcircle_instance = Instantiate(transcircle, Telespawn.position, Quaternion.identity);

    }
}
