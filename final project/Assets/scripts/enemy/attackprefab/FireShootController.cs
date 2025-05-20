using UnityEngine;
using System.Collections.Generic;
public class FireShootController : MonoBehaviour,IEnenmyResetInterface
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<PolygonCollider2D> hitboxs;
    private Animator ani;
    private enemy_property property;
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
        ani.SetBool("cast", true);
        gameObject.SetActive(true);
        foreach (PolygonCollider2D hitbox in hitboxs)
        {
            hitbox.enabled = false;
        }
    }
    public void EnableHitboxAt(int index)
    {
        if (index < 0 || index >= hitboxs.Count)
        {
            Debug.LogWarning($"Hitbox ���� {index} �W�X�d��");
            return;
        }

        // �����A�קK����
        foreach (var hitbox in hitboxs)
        {
            hitbox.enabled = false;
        }
        // �}�ҫ��w hitbox
        hitboxs[index].enabled = true;
        Debug.Log($"�ҥβ� {index} �q Hitbox");
    }
    public void closehitbox()
    {
        foreach (PolygonCollider2D hitbox in hitboxs)
        {
            hitbox.enabled = false;
        }
    }
    public void finish()
    {
        ani.SetBool("cast", false);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Property>().takedamage(property.atk, transform.position);
        }
    }
}
