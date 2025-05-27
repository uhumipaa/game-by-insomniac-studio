using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Boss_warrior_controller : MonoBehaviour
{
    public enum enemystate { Idle, Moving, Attack1, Attack2, Attack3, Stunning, stunafterattack }
    private enemystate currentstate;

    [Header("模組")]
    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private enemy_property property;

    [Header("腳本")]
    private IEnemyAttackBehavior attack;
    private IEnemyAnimatorBehavior animator;

    [Header("攻擊設定")]
    public float attackDistance = 3.8f;
    private float attackCooldown = 2f;
    private float lastAttackTime = -999f;
    public float stunAfterAttackTime = 1f;



    [Header("數值設定")]
    public float moveSpeed = 8f;
    public float jumpTriggerDistance = 5f;
    public float detect_distence = 10f;


    private float patrolTimer = 0f;
    private float patrolInterval = 5f;
    private float patrolMoveDuration = 2f;
    private float patrolMoveTimer = 0f;
    private bool isPatrolling = false;
    private Vector2 patrolDirection;
    private float previousDistance;
    private bool isAttacking = false;


    // Hitbox 物件（建議直接 assign）
    public GameObject hitbox_attack1;
    public GameObject hitbox_attack2;
    public GameObject hitbox_attack3;

    // 控制函式（供動畫事件呼叫）
    public void EnableHitbox1()
    {
        hitbox_attack1.SetActive(true);
        hitbox_attack1.GetComponent<WarriorHitbox>().EnableHitbox();
    }
    public void DisableHitbox1()
    {
        hitbox_attack1.GetComponent<WarriorHitbox>().DisableHitbox();
        hitbox_attack1.SetActive(false);
    }

    public void EnableHitbox2()
    {
        hitbox_attack2.SetActive(true);
        hitbox_attack2.GetComponent<WarriorHitbox>().EnableHitbox();
    }
    public void DisableHitbox2()
    {
        hitbox_attack2.GetComponent<WarriorHitbox>().DisableHitbox();
        hitbox_attack2.SetActive(false);
    }
    public void EnableHitbox3()
    {
        hitbox_attack3.SetActive(true);
        hitbox_attack3.GetComponent<WarriorHitbox>().EnableHitbox();
    }
    public void DisableHitbox3()
    {
        hitbox_attack3.GetComponent<WarriorHitbox>().DisableHitbox();
        hitbox_attack3.SetActive(false);
    }


    private void Start()
    {
        hitbox_attack1.SetActive(false);
        hitbox_attack2.SetActive(false);
        hitbox_attack3.SetActive(false);

        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentstate = enemystate.Idle;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool playerIsApproaching = distance < previousDistance;

        FlipToFacePlayer();

        // 如果正在僵直，什麼都不做（避免移動或再次攻擊）
        if (currentstate == enemystate.stunafterattack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (distance <= detect_distence)
        {
            // 若目前不是攻擊/僵直狀態才允許移動
            if (currentstate != enemystate.Attack1 &&
                currentstate != enemystate.Attack2 &&
                currentstate != enemystate.Attack3 &&
                currentstate != enemystate.stunafterattack)
            {
                currentstate = enemystate.Moving;
                MoveTowardsPlayer();
            }


            // 如果靠近且冷卻結束，就執行攻擊
            if (distance <= attackDistance && Time.time - lastAttackTime >= attackCooldown)
            {
                if (!isAttacking)
                {
                    StartCoroutine(PerformAttack());
                }

            }
        }
        else
        {
            PatrolBehavior();
        }

        UpdateAnimator();

        previousDistance = distance;

    }


    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    void PatrolBehavior()
    {
        Debug.Log("77777");
        patrolTimer += Time.deltaTime;

        if (isPatrolling)
        {
            patrolMoveTimer += Time.deltaTime;

            rb.linearVelocity = patrolDirection * moveSpeed;

            if (patrolMoveTimer >= patrolMoveDuration)
            {
                rb.linearVelocity = Vector2.zero;
                isPatrolling = false;
                patrolMoveTimer = 0f;
            }
        }
        else if (patrolTimer >= patrolInterval)
        {
            // 開始新一輪巡邏
            patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            isPatrolling = true;
            patrolTimer = 0f;
        }

        currentstate = enemystate.Idle;
    }

    void UpdateAnimator()
    {
        anim.SetBool("running", currentstate == enemystate.Moving);
        anim.SetBool("waiting", currentstate == enemystate.Idle);
    }

    public void OnAttackAnimationEnd()
    {
        currentstate = enemystate.Idle;
    }

    IEnumerator PerformAttack()
    {
        // 停止移動
        rb.linearVelocity = Vector2.zero;

        // 隨機選一個攻擊
        int rand = Random.Range(1, 4); // 1 ~ 3
        lastAttackTime = Time.time;
        switch (rand)
        {
            case 1:
                currentstate = enemystate.Attack1;
                EnableHitbox1();
                anim.SetTrigger("Attack1");
                break;
            case 2:
                currentstate = enemystate.Attack2;
                EnableHitbox2();
                anim.SetTrigger("Attack2");
                break;
            case 3:
                currentstate = enemystate.Attack3;
                EnableHitbox3();
                anim.SetTrigger("Attack3");
                break;
        }

        // 進入僵直狀態（如果你有這個列舉值可視覺用）
        currentstate = enemystate.stunafterattack;

        // 等待攻擊後停頓 1.0~2.0 秒，可調整
        yield return new WaitForSeconds(3f);

        // 回到待機狀態
        currentstate = enemystate.Idle;

        // 結束攻擊時關閉所有 Hitbox
        DisableHitbox1();
        DisableHitbox2();
        DisableHitbox3();

    }



    private void FlipToFacePlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        // 如果玩家在左邊但角色朝右（scale.x 為正），就反轉
        if (player.position.x < transform.position.x && scale.x > 0)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        // 如果玩家在右邊但角色朝左（scale.x 為負），就反轉
        else if (player.position.x > transform.position.x && scale.x < 0)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
    }


}
