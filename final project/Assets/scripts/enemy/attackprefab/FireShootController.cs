using UnityEngine;
using System.Collections.Generic;
public class FireShootController : MonoBehaviour,IEnenmyResetInterface
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<PolygonCollider2D> hitboxs;
    private Animator ani;
    private enemy_property property;
    private int hitbox_count = 0;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        foreach(PolygonCollider2D hitbox in hitboxs)
        {
            hitbox.enabled = false;
        }
        property = GetComponentInParent<enemy_property>();
    }
    public void Reset()
    {
        hitbox_count = 0;
        ani.SetBool("cast", true);
        gameObject.SetActive(true);
    }
    public void enablehitbox()
    {
        if (hitbox_count != 0)
        {
            hitboxs[hitbox_count].enabled = false;
            hitbox_count++;
            hitboxs[hitbox_count].enabled = true;
        }
        else
        {
            hitboxs[hitbox_count].enabled = true;
            hitbox_count++;
        }
    }
    public void closehitbox()
    {
        ani.SetBool("cast", false);
        foreach (PolygonCollider2D hitbox in hitboxs)
        {
            hitbox.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Property>().takedamage(property.atk, transform.position);
        }
    }
}
