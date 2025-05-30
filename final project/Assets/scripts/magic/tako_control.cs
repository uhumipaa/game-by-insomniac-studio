using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class tako_control : MonoBehaviour
{
    public float speed;
    private Vector2 actul_direction;
    private Rigidbody2D rb;
    private Animator ani;   
    private enemy_property property;
    private Player_Property player_Property;
    [SerializeField] float damagepara;
    private bool exploed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player_Property = GameObject.Find("player_battle").GetComponent<Player_Property>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        rb.gravityScale = 0f;
        Debug.Log("�����N��l�Ƨ���");
        StartCoroutine(natural_exlposion(3f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploed)
        {
            rb.linearVelocity = actul_direction * speed;
        }else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    public void setdirection(Vector2 direction)
    {
        actul_direction = direction;
        Debug.Log("�]�w�����N��V�G" + direction);
    }
    IEnumerator natural_exlposion(float delay)  
    {
        Audio_manager.Instance.Play(15, "player_tako", false, 0);
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
        explosion();
        Destroy(gameObject, 0.6f);
    }
    

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {   
            if (collision != null)
            {
                if (exploed)
                {
                    property = collision.GetComponent<enemy_property>();
                    if (property != null)
                    {
                        int damage = (int)(player_Property.magic_atk * damagepara);
                        property.takedamage(damage, transform.position);
                    }
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                    explosion();
                }
            }
        }else if (collision.CompareTag("wall"))
        {
            if (!exploed)
            {
                rb.linearVelocity = Vector2.zero;
                explosion();
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            if (collision != null)
            {
                if (exploed)
                {
                    property = collision.GetComponent<enemy_property>();
                    if (property != null)
                    {
                        Debug.Log("123");
                        int damage = (int)(player_Property.magic_atk * damagepara);
                        property.takedamage(damage, transform.position);
                    }
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                    explosion();
                }
            }
        }
    }
    void explosion()
    {
        exploed = true;
        ani.SetTrigger("isexploed");
        Destroy(gameObject, 0.6f);
    }   
}
