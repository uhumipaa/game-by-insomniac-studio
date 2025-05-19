using UnityEngine;

[CreateAssetMenu(fileName = "FloorData", menuName = "Scriptable Objects/FloorData")]
public class FloorData : ScriptableObject
{
    public EnemyData[] enemyprefab;
    public int enemycount;
    public Vector2 spwan_range_min= new Vector2 (-7f,5f);
    public Vector2 spawn_range_max= new Vector2 (9f,9f);
    public GameObject[] maps;
    public int levelrange;
}
