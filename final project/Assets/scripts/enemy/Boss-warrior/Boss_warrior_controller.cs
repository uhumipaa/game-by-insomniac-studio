using System.Collections;
using UnityEngine;

public class Boss_warrior_controller : MonoBehaviour
{
    public enum EnemyState { Idle, Run, Attack1, Attack2, Attack3, Jump }
    public EnemyState currentState = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public float jumpRange = 4f;
    public float idleDelay = 1f;

    public Transform player;
    public Animator animator;

    public WarriorHitbox attack1Hitbox;
    public WarriorHitbox attack2Hitbox;
    public WarriorHitbox attack3Hitbox;

    private Vector3 originalPosition;
    private bool isJumping = false;
    private float stateTimer = 0f;

    void Start()
    {
        originalPosition = transform.position;
        DisableAllHitboxes();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.Play("Idle");
                if (distanceToPlayer < attackRange)
                {
                    int attackType = Random.Range(0, 2);
                    currentState = (attackType == 0) ? EnemyState.Attack2 : EnemyState.Attack3;
                    stateTimer = 0f;
                }
                else if (distanceToPlayer < jumpRange)
                {
                    currentState = EnemyState.Jump;
                    stateTimer = 0f;
                }
                else if (distanceToPlayer < 8f)
                {
                    currentState = EnemyState.Run;
                    stateTimer = 0f;
                }
                break;

            case EnemyState.Run:
                animator.Play("Run");
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                if (distanceToPlayer < attackRange)
                {
                    currentState = EnemyState.Attack1;
                    stateTimer = 0f;
                }
                else if (stateTimer > idleDelay)
                {
                    currentState = EnemyState.Idle;
                    stateTimer = 0f;
                }
                break;

            case EnemyState.Attack1:
                animator.Play("Attack1");
                break;

            case EnemyState.Attack2:
                animator.Play("Attack2");
                break;

            case EnemyState.Attack3:
                animator.Play("Attack3");
                break;

            case EnemyState.Jump:
                if (!isJumping)
                {
                    StartCoroutine(JumpTowardsPlayer());
                }
                break;
        }
    }

    IEnumerator JumpTowardsPlayer()
    {
        isJumping = true;
        Vector3 jumpTarget = player.position;
        animator.Play("Jump");
        float jumpDuration = 0.4f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        while (elapsed < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPos, jumpTarget, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        isJumping = false;
        currentState = EnemyState.Idle;
    }

    // Animation Event Functions
    public void EnableHitbox1() { attack1Hitbox.EnableHitbox(); }
    public void DisableHitbox1() { attack1Hitbox.DisableHitbox(); }

    public void EnableHitbox2() { attack2Hitbox.EnableHitbox(); }
    public void DisableHitbox2() { attack2Hitbox.DisableHitbox(); }

    public void EnableHitbox3() { attack3Hitbox.EnableHitbox(); }
    public void DisableHitbox3() { attack3Hitbox.DisableHitbox(); }

    void DisableAllHitboxes()
    {
        attack1Hitbox.DisableHitbox();
        attack2Hitbox.DisableHitbox();
        attack3Hitbox.DisableHitbox();
    }
}
