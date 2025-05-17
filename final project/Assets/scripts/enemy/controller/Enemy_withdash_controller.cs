using UnityEngine;
using System.Collections;
using static Enemy_withdash_controller;

public class Enemy_withdash_controller : MonoBehaviour,IEnemyControllerInterface
{
    public enum enemystate { Idle,Moving,Attacking,Dashing,Stunning,stunafterattack }
    private enemystate currentstate;
    [Header("基礎屬性")]
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player;
    private enemy_property property;
    public MonoBehaviour[] scripts;

    [Header("模組")]
    public IEnemyAttackBehavior attack;
    public IEnemyAnimatorBehavior animator;
    public IEnemyMoveBehavior move;
    public IEnemySpecilskillBehavior sp;

    [SerializeField]private float scale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attack = scripts[0] as IEnemyAttackBehavior;
        animator = scripts[1] as IEnemyAnimatorBehavior;
        move = scripts[2] as IEnemyMoveBehavior;
        sp = scripts[3] as IEnemySpecilskillBehavior;
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = FindAnyObjectByType<Player_Property>().transform;
        property = GetComponent<enemy_property>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentstate)
        {
            case enemystate.Idle:
            case enemystate.Moving:
                AttackorMove();
                break;
            case enemystate.Attacking:
                //動畫控制
                break;
            case enemystate.stunafterattack:
                //協程控制
                break;
            case enemystate.Dashing:
                break;
            case enemystate.Stunning:
                break;
        }        
    }
    void flip()
    {
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.localScale = new Vector2(-scale, scale);
        }
    }
    public void FinishAttack()
    {
        animator.PlayIdle(ani);
        //StartCoroutine(AttackToDashDelay(0.5f));
        Debug.Log("finish2");
        dash();
    }
    public void Finishdash()
    {
        animator.PlayIdle(ani);
        StartCoroutine(Stun(property.stuncd));
    }
    IEnumerator Stun(float cd)
    {
        currentstate = enemystate.Stunning;
        Debug.Log("stuned");
        yield return new WaitForSecondsRealtime(cd);
        Debug.Log("stuned");
        currentstate = enemystate.Idle;
    }

    void AttackorMove()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        flip();
        if (distance < property.attack_range)
        {
            currentstate = enemystate.Attacking;
            rb.linearVelocity = Vector2.zero;
            animator.PlayAttack((player.position - transform.position).normalized, ani);
            attack.Attack(transform, player, property.atk, scale);
        }
        else
        {
            currentstate = enemystate.Moving;
            animator.PlayMove((player.position - transform.position).normalized, ani);
            move.Move(transform, player, rb, property.speed);
        }
    }
    IEnumerator AttackToDashDelay(float delay)
    {
        Debug.Log("stunafterattack");
        currentstate = enemystate.stunafterattack;
        yield return new WaitForSecondsRealtime(delay);
        dash();
    }
    void dash()
    {
        sp.usingskill(transform, player, property, scale);
        currentstate = enemystate.Dashing;
    }
}
