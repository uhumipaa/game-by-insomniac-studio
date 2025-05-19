using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int baseHP;
    public int baseatk;
    public int basedef;
    public int speed;
    public string enemyname;
    public float atkpara;
    public float defpara;
    public float HPpara;
    public GameObject enemyprefab;
}
