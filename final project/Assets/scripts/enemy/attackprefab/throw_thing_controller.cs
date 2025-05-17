
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class throw_thing_controller : MonoBehaviour
{
    private float total_distance;
    //private Vector2 Start;
    //private Vector2 End;
    public AnimationCurve heightCurve;
    private Animator ani;
    private Rigidbody2D rb;
    private bool explosioning=false;
    private Vector2 Dir;
    private float dmg;
    private PolygonCollider2D col;
    [SerializeField] float duration;
    [SerializeField] float maxHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<PolygonCollider2D>();
        col.enabled = false;
    }

    // Update is called once per frame
    public IEnumerator Cruve(Vector3 start,Vector3 end)
    {
        float timepast = 0f;
        while (timepast<duration)
        {
            if (transform.position == end)
            {
                break;
            }
            timepast += Time.deltaTime;
            var linearTime = timepast / duration;
            var heightTime = heightCurve.Evaluate(linearTime);
            var height = Mathf.Lerp(0f,maxHeight , heightTime);
            transform.position = Vector3.Lerp(start, end, linearTime) + new Vector3(0f, height, 0f);
            float scale = 1.5f + 0.5f*height;
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        explosion();
        
    }
    /*
    void Update()
    {
        
        if (explosioning) return;
        //rb.linearVelocity = Dir * 0.5f;
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
        float scale = 1.5f + height;
        
        transform.localScale = new Vector3(scale, scale, 1f);
    }
    */
    void explosion()
    {
        ani.SetBool("explosion", true);
    }
    public void Enablehitbox()
    {
        col.enabled = true;
        transform.localScale = new Vector3(2f, 2f, 1f);
    }
    public void Bomb()
    {
        Destroy(gameObject);
    }
    public void Set_parabola(Vector2 end,Vector2 direction,float damage)
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
        //Dir = direction;
        total_distance = Vector2.Distance(transform.position, end);
        Debug.Log("setfinish");
        dmg = damage;
        StartCoroutine(Cruve(transform.position, end));
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("sdasd");
            collision.GetComponent<Player_Property>().takedamage((int)dmg, transform.position);
            explosion();
        }
    }
}
