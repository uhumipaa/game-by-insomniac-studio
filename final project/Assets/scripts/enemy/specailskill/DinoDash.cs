using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
public class DinoDash : MonoBehaviour,IEnemySpecilskillBehavior
{
    [SerializeField] float duration;
    [SerializeField] float changetime;
    private float speed;
    private Transform Player;
    private Enemy_withdash_controller controller;
    private Rigidbody2D rb;
    private Vector2 direction;
    public PolygonCollider2D hitbox;
    private Animator ani;
    private bool meetwall;
    private enemy_property Property;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<Enemy_withdash_controller>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        hitbox.enabled = false;
    }
    public void usingskill(Transform self, Transform player, enemy_property property, float scale)
    {
        Player = player;
        speed = property.speed*1.5f;
        Property = property;
        direction = (player.position-self.position );
        StartCoroutine(dashing(scale));
        
    }
    IEnumerator dashing(float scale)
    {
        ani.SetBool("dashing", true);
        hitbox.enabled = true;
        float pasttime = 0;
        while (pasttime < duration)
        {
            pasttime += Time.deltaTime;
            if (pasttime % changetime == 0||meetwall)
            {
                direction = getdirection(scale);
                meetwall = false;
            }
            rb.linearVelocity = speed * direction;
            yield return null;
        }
        finishdash();
    }
    Vector2 getdirection(float scale)
    {
        if (transform.position.x < Player.position.x)
        {
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.localScale = new Vector2(-scale, scale);
        }
        Vector2 direction;
        direction = (Player.position - transform.position);
        return direction;
    }
    void finishdash()
    {
        hitbox.enabled = false;
        ani.SetBool("dashing", false);
        ani.SetBool("waiting", true);
        controller.Finishdash();
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Property>().takedamage(Property.atk, transform.position);
        }else if (collision.CompareTag("wall"))
        {
            meetwall = true;
        }
    }

}
