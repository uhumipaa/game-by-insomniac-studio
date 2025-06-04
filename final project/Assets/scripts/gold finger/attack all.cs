using UnityEngine;

public class attackall : MonoBehaviour
{
    Vector2 v;
    public void DealMassiveDamage()
    {
        // 找出場景中所有帶有 Enemy_Property 的物件
        enemy_property[] enemies = FindObjectsByType<enemy_property>(FindObjectsSortMode.None);
        foreach (enemy_property enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<Dark_Magicion_Controller>() != null)
            {
                enemy.gameObject.GetComponent<Dark_Magicion_Controller>().Die();
                continue;
            }
            else
            {
                enemy.die();
            }
        }

        // Debug.Log("對 " + enemies.Length + " 名敵人造成了 5000 點傷害！");
    }
}
