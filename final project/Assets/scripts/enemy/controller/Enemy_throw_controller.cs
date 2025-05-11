using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Enemy_throw_controller : MonoBehaviour
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
    private bool isinattackerange;
    private bool isattacking;
    private float lastattackattacktime;
    private bool canattack=true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = scripts[0] as IEnemyAttackBehavior;
        animator = scripts[1] as IEnemyAnimatorBehavior;
        move = scripts[2] as IEnemyMoveBehavior;
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = FindAnyObjectByType<player_controler>().transform;
        property = GetComponent<enemy_property>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isattacking)
        {
            return;
        }
        float distance = Vector2.Distance(player.position, transform.position);
        Vector2 direaction = (transform.position - player.position).normalized;
        isinattackerange = distance < property.attack_range ? true : false;
        move.Move(transform, player, rb, property.speed);
        animator.PlayMove(direaction, ani);
        if (!canattack)
        {
           
            if (isstuned)
            {
                if (Time.time - lastattackattacktime > property.stuncd)
                {
                    isstuned = false;
                    canattack = true;
                }
                else
                {
                    isstuned = true;
                }
            }
        }
        else
        {
            if (isinattackerange&&canattack)
            {
                attack.Attack(transform, player, property.atk);
                animator.PlayAttack(direaction, ani);
                isattacking = true;
                canattack = false;
            }
        }
    }
    /*
        IEnumerator Stun(float cd)
        {
            Debug.Log("stunning");
            yield return new WaitForSeconds(cd);
            isstuned = false;
        
        }
    */
        public void FinishAttack()
        {
            Debug.Log("finishattack");
            animator.PlayIdle(ani);
            isattacking = false;
            isstuned = true;
            lastattackattacktime = Time.time;
            canattack = false;
            //StartCoroutine(Stun(property.stuncd));
    }
}
    
