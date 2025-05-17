using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int baseHP;
    public int baseatk;
    public int basedef;
    public int speed;
    public string enemyname;
    public int atkpara;
    public int defpara;
    public int HPpara;
    public GameObject enemyprefab;
}
