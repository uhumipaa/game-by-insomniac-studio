using UnityEngine;

public class attackall : MonoBehaviour
{
    Vector2 v;
    public void DealMassiveDamage()
    {
        // 找出場景中所有帶有 Enemy_Property 的物件
        enemy_property[] enemies = FindObjectsOfType<enemy_property>();

        foreach (enemy_property enemy in enemies)
        {
            enemy.takedamage(5000, v);
        }

        Debug.Log("對 " + enemies.Length + " 名敵人造成了 5000 點傷害！");
    }
}
