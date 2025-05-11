using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class throw_thing_controller : MonoBehaviour
{
    private float total_distance;
    private Vector2 Start;
    private Vector2 End;
    public AnimationCurve heightCurve;
    private Animator ani;
    private Rigidbody2D rb;
    private bool explosioning=false;
    private Vector2 Dir;
    private float dmg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (explosioning) return;
        rb.linearVelocity = Dir * 1f;
        float now_distance = Vector2.Distance(Start, transform.position);
        float t = now_distance / total_distance;
        if (t >= 1f)
        {
            rb.linearVelocity = Vector2.zero;
            explosion();
            explosioning = true;
            return;
        }
        Vector2 pos = Vector2.Lerp(Start, End, t);
        float height = heightCurve.Evaluate(t);
        pos.y += height;
        transform.position = pos;
        Debug.Log($"{t}");
        float scale = 1.5f - height * 0.5f;
        
        transform.localScale = new Vector3(scale, scale, 1f);
    }
    void explosion()
    {
        ani.SetBool("explosion", true);
    }
    public void Bomb()
    {
        Destroy(gameObject);
    }
    public void Set_parabola(Vector2 end,Vector2 direction,float damage)
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
        Dir = direction;
        Start = transform.position;
        End = end;
        total_distance = Vector2.Distance(Start, end);
        Debug.Log("setfinish");
        dmg = damage;
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("sdasd");
            other.GetComponent<Player_Property>().takedamage((int)dmg, transform.position);
            explosion();
        }
    }
    */
}
