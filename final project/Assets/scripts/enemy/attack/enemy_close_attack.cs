using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Close_Attack : MonoBehaviour,IEnemyAttackBehavior
{
    public GameObject[] hitbox;
    private int hitbox_num;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    
    public void Attack(Transform self, Transform player,float attack, float scale)
    {
        //調用陣列的hitbox，0為左右，1下2上
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
    //下面的記得掛在animation
    public void Enablehitbox()
    {
        hitbox[hitbox_num].GetComponent<Hitbox_Controller>().Enablecol();
    }
    public void Closehitbox()
    {
        hitbox[hitbox_num].GetComponent<Hitbox_Controller>().Closecol();
    }
}
