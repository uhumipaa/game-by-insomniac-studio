using UnityEngine;
using System.Collections;
public class firegoblincontroller : MonoBehaviour
{
    private Transform player;
    private bool wait;
    private Animator ani;
    private bool attack;
    public float attack_time;
    private enemy_property property;
    public float waitcd;
    private bool isDoingCoroutine = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        property = GetComponent<enemy_property>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isDoingCoroutine) return;
        //§ï¤è¦V
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (!attack && !wait)
        {
            Move();
        }else if(attack&&!wait)
        {
            isDoingCoroutine = true;
            StartCoroutine(Attackwithcd(attack_time));
        }
        else 
        {
            isDoingCoroutine = true;
            StartCoroutine(Waitasecond(waitcd));
        }
    }
    void Move()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance >= property.attack_range)
        {
            ani.SetBool("running", true);
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = (Vector2)transform.position + (direction * property.speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
        else
        {
            attack = true;
        }
    }
    IEnumerator Attackwithcd(float delay)
    {
        Debug.Log("attack");
        if (transform.position.y <= player.position.y+0.1f&& transform.position.y <= player.position.y - 0.1f)
        {
            ani.SetFloat("horizontal", 1f);
            ani.SetFloat("vetcal", 0f);
        }
        else
        {
            ani.SetFloat("horizontal", 0f);
            if (transform.position.y > player.position.y)
            {
                ani.SetFloat("vetical", -1f);
            }
            else
            {
                ani.SetFloat("vetical", 1f);
            }
        }
            ani.SetBool("attacking", true);
        ani.SetBool("running", false);
        ani.SetBool("waiting", false);
        yield return new WaitForSeconds(delay);
        if (Vector2.Distance(transform.position, player.position) < property.attack_range)
        {
            player.GetComponent<Player_Property>()?.takedamage(property.atk, transform.position);
        }
        ani.SetBool("attacking", false);
        ani.SetBool("waiting", true);
        wait = true;
        attack = false;
        isDoingCoroutine = false;
    }
    IEnumerator Waitasecond(float delay)
    {
        Debug.Log("wait");
        yield return new WaitForSeconds(delay);
        wait = false;
        isDoingCoroutine = false;
    }
}
