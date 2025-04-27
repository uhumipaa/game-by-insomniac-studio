using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static EnemyManager instance; 
    public GameObject[] enemyprefab;
    private List<GameObject> now_enemy = new List<GameObject>();
    private Maploader maploader;

    void Awake()
    {
        instance = this;
        maploader = FindAnyObjectByType<Maploader>();
    }

    public void Addenemy(GameObject newenemy)
    {
        now_enemy.Add(newenemy);
    }

    public void removeenemy(GameObject enemy)
    {
        if (now_enemy.Contains(enemy))
        {
            now_enemy.Remove(enemy);
            check();
        }
        
    }
    private void check()
    {
        if (now_enemy.Count <= 0)
        {
            finishlayer();
        }
    }
    private void finishlayer()
    {
        maploader.GetComponent<Maploader>().generate_tele();
    }

    public void Startfight()
    {
        int floor = TowerManager.Instance.currentTowerFloor;
        int random = Random.Range(0, 5);
        
    }
}
