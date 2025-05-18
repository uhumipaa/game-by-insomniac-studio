using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Boss_dino_controller : MonoBehaviour,IEnemyControllerInterface,IEnemySkillContollerInterface
{
    public enum enemystate { Idle, Moving, Attacking, usingskill, Stunning, stunafterattack }
    private enemystate currentstate;
    [Header("模組")]
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player;
    private enemy_property property;
    public MonoBehaviour[] scripts;
    public MonoBehaviour[] skillscripts;

    [Header("腳本")]
    private IEnemyAttackBehavior attack;
    private IEnemyAnimatorBehavior animator;
    private IEnemySpecilskillBehavior usingskill;

    [Header("屬性")]
    [SerializeField] private float scale;
    [SerializeField] protected float stunAfterAttackDuration = 0.5f;
    private bool rage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attack = GetComponent<Enemy_8DireactionCast_Attack>();
        if (attack == null)
        {
            Debug.Log("fasf");
        }
        animator = scripts[1] as IEnemyAnimatorBehavior;
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
                //�ʵe����
                break;
            case enemystate.stunafterattack:
                //��{����
                break;
            case enemystate.usingskill:
                break;
            case enemystate.Stunning:
                break;
        }
        if (!rage)
        {
            if (property.current_health < property.max_health / 2)
            {
                rage = true;
            }
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void FinishAttack()
    {
        if (currentstate != enemystate.Attacking)
            return;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(AttackToDashDelay(stunAfterAttackDuration));
        animator.PlayIdle(ani);
        Debug.Log("finish2");
    }
    public void Finishskill()
    {
        Debug.Log("finishskill");
        animator.PlayIdle(ani);
        ani.SetBool("waiting", true);
        ani.SetBool("dashing", false);
        ani.SetBool("jumping", false);
        ani.SetBool("casting", false);
        StartCoroutine(Stun(property.stuncd));
    }
    IEnumerator Stun(float cd)
    {
        currentstate = enemystate.Stunning;
        Debug.Log("stuned");
        yield return new WaitForSecondsRealtime(cd);
        currentstate = enemystate.Idle;
    }

    void AttackorMove()
    {
        flip();
        currentstate = enemystate.Attacking;
        rb.linearVelocity = Vector2.zero;
        animator.PlayAttack((player.position - transform.position).normalized, ani);
        attack.Attack(transform, player, property.atk, scale);
    }
    IEnumerator AttackToDashDelay(float cd)
    {
        Debug.Log("stunafterattack");
        currentstate = enemystate.stunafterattack;
        yield return new WaitForSecondsRealtime(cd);
        useskill();
    }
    void useskill()
    {
        ani.SetBool("waiting", false);
        currentstate = enemystate.usingskill;
        if (rage)
        {
            int rmd = Random.Range(0, 3);
            usingskill = skillscripts[rmd] as IEnemySpecilskillBehavior;
            usingskill.usingskill(transform, player, property, scale);
            switch (rmd)
            {
                case 1:
                    ani.SetBool("dashing", true);
                    break;
                case 2:
                    ani.SetBool("jumping", true);
                    break;
                case 3:
                    ani.SetBool("casting", true);
                    break;
            }
        }
        else
        {
            int rmd = Random.Range(0, 2);
            usingskill = skillscripts[rmd] as IEnemySpecilskillBehavior;
            usingskill.usingskill(transform, player, property, scale);
            switch (rmd)
            {
                case 0:
                    ani.SetBool("dashing", true);
                    break;
                case 1:
                    ani.SetBool("jumping", true);
                    break;
            }
        }
        
        
        
    }
}
