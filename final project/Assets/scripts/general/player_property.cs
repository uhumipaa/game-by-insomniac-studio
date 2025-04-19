using System;
using UnityEngine;

public class Player_Property : MonoBehaviour
{
    
    public int max_health;
    public int current_health;
    public int atk;
    public int def;
    public float attack_time;
    public float speed;
    public float attackrange;

    private Knockback knockback;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        current_health = max_health;
        knockback = GetComponent<Knockback>();
    }

    public void takedamage(int damage , Vector2 attackerPos)
    {
        int actual_def = UnityEngine.Random.Range(def - 5, def + 6);
        int actual_damage = Mathf.Max(damage - actual_def,0);
        current_health -= actual_damage;
        Debug.Log($"���a���� {actual_damage} �ˮ`�A�ثe��q {current_health}");
        
        // 擊退效果
        if (knockback != null)
        {
            knockback.ApplyKnockback(attackerPos);
        }
        
        if (current_health < 0)
        {
            die();
        }
    }
    void die()
    {
        Debug.Log("���`�G�G");
    }
}
