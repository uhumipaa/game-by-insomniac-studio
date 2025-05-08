using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Close_Attack : MonoBehaviour,IEnemyAttackBehavior
{
    public GameObject[] hitbox;
    private int hitbox_num;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    
    public void Attack(Transform self, Transform player,float attack)
    {
        Vector2 diff = player.position - transform.position;
        if (Mathf.Abs(diff.y) < 0.2f)
        {
            
            hitbox_num = 0;
        }
        else
        {
            hitbox_num = diff.y > 0 ? 1 : 2;
        }
    }
    public void Enablehitbox()
    {
        hitbox[hitbox_num].GetComponent<Hitbox_Controller>().Enablecol();
    }
    public void Closehitbox()
    {
        hitbox[hitbox_num].GetComponent<Hitbox_Controller>().Closecol();
    }
}
