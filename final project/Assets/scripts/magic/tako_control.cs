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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        rb.gravityScale = 0f;
        Debug.Log("章魚燒初始化完成");
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
        Debug.Log("設定章魚燒方向：" + direction);
    }
    IEnumerator natural_exlposion(float delay)  
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
        explosion();
        Destroy(gameObject, 0.3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Debug.Log("碰到：" + collision.name);
            property = collision.GetComponent<enemy_property>();
            property.takedamage(damage);
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
        Destroy(gameObject, 0.3f);
    }
}
