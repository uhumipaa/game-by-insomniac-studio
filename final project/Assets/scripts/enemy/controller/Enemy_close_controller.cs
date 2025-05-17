using UnityEngine;
using System.Collections;

public class Enemy_close_controller : MonoBehaviour, IEnemyControllerInterface
{
    [Header("模組")]
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player;
    private enemy_property property;
    public MonoBehaviour[] scripts;
    [Header("腳本")]
    public IEnemyAttackBehavior attack;
    public IEnemyAnimatorBehavior animator;
    public IEnemyMoveBehavior move;
    [Header("屬性")]
    public bool isstuned;
    public bool isattacking;
    public float scale;
    public Transform attackPivot;
    public bool isKnight;
    private float idleRotateTimer;
    public float idleRotateInterval = 2f;  // 隔2秒換方向
    private int idleRotateDirection = 1;   // 1 向右 -1 向左


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
        if (isattacking || isstuned)
        {
            return;
        }
        float distance = Vector2.Distance(player.position, attackPivot.position);

        if (distance < property.attack_range)
        {
            rb.linearVelocity = Vector2.zero;
            FaceToPlayer();
            animator.PlayAttack((player.position - attackPivot.position).normalized, ani);
            attack.Attack(attackPivot, player, property.atk, scale);
            isattacking = true;
        }
        else
        {
            if (isKnight)
            {
                if (distance < property.detect_range)  // 只有在偵測範圍內才移動
                {
                    FaceToPlayer();
                    animator.PlayMove((player.position - transform.position).normalized, ani);
                    move.Move(transform, player, rb, property.speed);
                }
                else
                {
                    // 騎士沒有看到人的話原地左右轉
                    KingRotate();
                }
            }
            else
            {
                // 非騎士就普通移動
                animator.PlayMove((player.position - transform.position).normalized, ani);
                move.Move(transform, player, rb, property.speed);
            }
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
    IEnumerator Stun(float cd)
    {
        Debug.Log("stuned");
        yield return new WaitForSecondsRealtime(cd);
        Debug.Log("stuned");
        isstuned = false;
    }
    void KingRotate()
    {
        rb.linearVelocity = Vector2.zero;  // 停止移動

        idleRotateTimer += Time.deltaTime;
        if (idleRotateTimer >= idleRotateInterval)
        {
            idleRotateDirection *= -1;
            transform.localScale = new Vector3(scale * idleRotateDirection, scale, 1);
            idleRotateTimer = 0;
        }

        animator.PlayIdle(ani);  // 播放待機動畫
    }
    void FaceToPlayer()
    {
        if (attackPivot.position.x < player.position.x)
        {
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.localScale = new Vector2(-scale, scale);
        }
    }
}
