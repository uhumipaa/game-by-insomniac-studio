using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static EnemyManager instance; 
    public List<GameObject> now_enemy = new List<GameObject>();
    private Maploaders maploader;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // �����
        }
        maploader = FindAnyObjectByType<Maploaders>();
    }

    public void Addenemy(GameObject newenemy)
    {
        now_enemy.Add(newenemy);
    }

    public void removeenemy(GameObject enemy)
    {
        if (now_enemy == null)
        {
            Debug.LogWarning("now_enemy �C���O null�I");
            return;
        }
        if (now_enemy.Contains(enemy))
        {
            now_enemy.Remove(enemy);
            Debug.Log("enemycount:"+now_enemy.Count);
            check();
        }
        
    }
    public void check()
    {
        if (now_enemy.Count <= 0)
        {
            finishlayer();
        }
    }

    public void finishlayer()
    {
        Debug.Log("yayaya");
        maploader.GetComponent<Maploaders>().generate_tele();
        maploader.GetComponent<Maploaders>().generate_chest();
    }

    public void Startfight()
    {
        int floor = TowerManager.Instance.currentTowerFloor;
        int random = Random.Range(0, 5);
        
    }
}
