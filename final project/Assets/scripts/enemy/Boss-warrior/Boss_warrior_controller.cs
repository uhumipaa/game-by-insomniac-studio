using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Boss_warrior_controller : MonoBehaviour, IEnemyControllerInterface
{
    public enum enemystate { Idle, Moving, Attack1, Attack2, Attack3, Jump, Stunning, stunafterattack }
    private enemystate currentstate;
    [Header("模組")]
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player;
    private enemy_property property;

    [Header("腳本")]
    private IEnemyAttackBehavior attack;
    private IEnemyAnimatorBehavior animator;

    [Header("偵測")]
    public float detectRange = 20f;
    public float attackRange = 6f;

    [Header("數值參數")]
    public float moveSpeed = 8f;

    [Header("Hitbox")]
    public WarriorHitbox attack1Hitbox;
    public WarriorHitbox attack2Hitbox;
    public WarriorHitbox attack3Hitbox;

    [Header("屬性")]
    [SerializeField] private float scale;
    [SerializeField] protected float stunAfterAttackDuration = 0.5f;

    private bool isJumping = false;
    private bool isAttacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = FindAnyObjectByType<Player_Property>().transform;
        property = GetComponent<enemy_property>();
    }

    void Update()
    {
        switch (currentstate)
        {
            case enemystate.Idle:
            case enemystate.Moving:
                AttackorMove();
                break;
            case enemystate.Attack1:
                //�ʵe����
                break;
            case enemystate.stunafterattack:
                //��{����
                break;
            case enemystate.Attack2:
                break;
            case enemystate.Attack3:
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
        if (currentstate != enemystate.Attack1 && currentstate != enemystate.Attack2 && currentstate != enemystate.Attack3)
            return;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(AttackToRunDelay(stunAfterAttackDuration));
        animator.PlayIdle(ani);
        Debug.Log("finish2");
    }
    IEnumerator Stun(float cd)
    {
        currentstate = enemystate.Stunning;
        Debug.Log("stuned");
        yield return new WaitForSecondsRealtime(cd);
        currentstate = enemystate.Idle;
    }

    IEnumerator AttackToRunDelay(float cd)
    {
        Debug.Log("stunafterattack");
        currentstate = enemystate.stunafterattack;
        yield return new WaitForSecondsRealtime(cd);
    }
    void AttackorMove()
    {
        flip();
        if(isAttacking == true)
        {

        }
        //currentstate = enemystate.Attacking;
        rb.linearVelocity = Vector2.zero;
        animator.PlayAttack((player.position - transform.position).normalized, ani);
        attack.Attack(transform, player, property.atk, scale);
    }

    void startattack()
    {
        ani.SetBool("waiting", false);
        currentstate = enemystate.Attack1;
        
        int rmd = Random.Range(0, 3);
        //usingskill = skillscripts[rmd] as IEnemySpecilskillBehavior;
        //usingskill.usingskill(transform, player, property, scale);
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


    public void StartJump()
    {
        isJumping = true;
        //currentState = enemystate.Jump;
        //animator.SetTrigger("Jump");
    }

    public void EndJump()
    {
        //transform.position = originalPosition;
        isJumping = false;
        //currentState = enemystate.Idle;
    }

    // 給動畫事件使用
    public void EnableHitbox1() => attack1Hitbox.EnableHitbox();
    public void DisableHitbox1() => attack1Hitbox.DisableHitbox();
    public void EnableHitbox2() => attack2Hitbox.EnableHitbox();
    public void DisableHitbox2() => attack2Hitbox.DisableHitbox();
    public void EnableHitbox3() => attack3Hitbox.EnableHitbox();
    public void DisableHitbox3() => attack3Hitbox.DisableHitbox();

    public void DisableAllHitboxes()
    {
        DisableHitbox1();
        DisableHitbox2();
        DisableHitbox3();
    }
}
