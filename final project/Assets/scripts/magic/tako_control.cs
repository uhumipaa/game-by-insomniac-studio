using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class tako_control : MonoBehaviour
{
    public float speed;
    private Vector2 actul_direction;
    public int damage;
    private Rigidbody2D rb;
    private Animator ani;   
    private enemy_property property;
    private Transform attackerTransform;//紀錄釋放者位置
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        rb.gravityScale = 0f;
        Debug.Log("�����N��l�Ƨ���");
        StartCoroutine(natural_exlposion(3f));
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = actul_direction * speed;
    }
    public void setdirection(Vector2 direction)
    {
        actul_direction = direction;
        Debug.Log("�]�w�����N��V�G" + direction);
    }
    IEnumerator natural_exlposion(float delay)  
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
        explosion();
        Destroy(gameObject, 0.6f);
    }
    

    public void SetAttacker(Transform attacker)
    {
        attackerTransform = attacker;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {   
            if (collision != null)
            {
                property = collision.GetComponent<enemy_property>();
                if (property != null)
                {
                    Debug.Log("gg" + collision.name);
                    property.takedamage(damage, transform.position);
                }
            }
            rb.linearVelocity = Vector2.zero;
            explosion();
        }else if (collision.CompareTag("wall"))
        {
            rb.linearVelocity = Vector2.zero;
            explosion();
        }
    }
    void explosion()
    {
        ani.SetBool("isexploed", true);
        Destroy(gameObject, 0.6f);
    }
}
