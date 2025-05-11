using UnityEngine;
using System.Collections;

public class Enemy_close_controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player;
    private enemy_property property;
    public MonoBehaviour[] scripts;
    public IEnemyAttackBehavior attack;
    public IEnemyAnimatorBehavior animator;
    public IEnemyMoveBehavior move;
    private bool isstuned;
    private bool canmove=true;
    private bool isinattackerange;
    private bool isattacking;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attack = scripts[0] as IEnemyAttackBehavior;
        animator = scripts[1] as IEnemyAnimatorBehavior;
        move = scripts[2] as IEnemyMoveBehavior;
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = FindAnyObjectByType<Player_Property>().transform;
        property = GetComponent<enemy_property>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isattacking==true)
        {
            return;
        }
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        }
        else
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }
        if (canmove&&!isstuned)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance < property.attack_range)
            {
                rb.linearVelocity = Vector2.zero;
                canmove = false;
                isinattackerange = true;
                return;
            }
            else
            {
                isinattackerange = false;
                animator.PlayMove((player.position - transform.position).normalized, ani);
                move.Move(transform, player, rb, property.speed);
            }
        }
        if (isinattackerange&&!isstuned)
        {
            animator.PlayAttack((player.position - transform.position).normalized, ani);
            attack.Attack(transform, player, property.atk);
            isattacking = true;
            canmove = false;
        }
        if (isstuned)
        {
            Debug.Log("stuned");
            animator.PlayIdle(ani);
            StartCoroutine(Stun(property.stuncd));
        }

    }
    public void FinishAttack()
    {
        Debug.Log("finish2");
        canmove = false;
        isattacking = false;
        isstuned = true;
    }
    IEnumerator Stun(float cd)
    {
        yield return new WaitForSecondsRealtime(cd);
        isstuned = false;
        canmove = true;
    }
}
