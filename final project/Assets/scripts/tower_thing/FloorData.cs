using UnityEngine;

[CreateAssetMenu(fileName = "FloorData", menuName = "Scriptable Objects/FloorData")]
public class FloorData : ScriptableObject
{
    public EnemyData[] enemyprefab;
    public int enemycount;
    public Vector2 spwan_range_min;
    public Vector2 spawn_range_max;
    public int levelrange;
}
