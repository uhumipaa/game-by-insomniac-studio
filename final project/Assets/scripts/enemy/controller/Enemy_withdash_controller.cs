using UnityEngine;
using System.Collections;

public class Enemy_withdash_controller : MonoBehaviour,IEnemyControllerInterface
{
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player;
    private enemy_property property;
    public MonoBehaviour[] scripts;
    public IEnemyAttackBehavior attack;
    public IEnemyAnimatorBehavior animator;
    public IEnemyMoveBehavior move;
    public IEnemySpecilskillBehavior sp;
    private bool isstuned;
    private bool isattacking;
    private bool dashing;
    public float scale;
    private bool has_attack;

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
        if (dashing || isattacking||isstuned) return;
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.localScale = new Vector2(-scale, scale);
        }
        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < property.attack_range&&!has_attack)
        {
            rb.linearVelocity = Vector2.zero;
            animator.PlayAttack((player.position - transform.position).normalized, ani);
            attack.Attack(transform, player, property.atk,scale);
            isattacking = true;
            has_attack = true;
        }
        else if (has_attack)
        {
            sp.usingskill(transform, player, property,scale);
            dashing = true;
            has_attack = false;
        }
        else 
        {
            animator.PlayMove((player.position - transform.position).normalized, ani);
            move.Move(transform, player, rb, property.speed);
        }

    }
    public void FinishAttack()
    {
        animator.PlayIdle(ani);
        Debug.Log("finish2");
        isattacking = false;
        isstuned = true;
        StartCoroutine(Stun(property.stuncd));
    }
    public void Finishdash()
    {
        animator.PlayIdle(ani);
        dashing = false;
        isstuned = true;
        StartCoroutine(Stun(property.stuncd));
    }
    IEnumerator Stun(float cd)
    {
        Debug.Log("stuned");
        yield return new WaitForSecondsRealtime(cd);
        Debug.Log("stuned");
        isstuned = false;
    }
}
