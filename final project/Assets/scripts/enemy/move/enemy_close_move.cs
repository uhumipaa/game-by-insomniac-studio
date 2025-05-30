using UnityEngine;
using System.Collections;

public class enemy_close_move : MonoBehaviour,IEnemyMoveBehavior
{
    private Transform player;
    public float turnSpeed = 5f;
    public float stuned_time=1f;
    private int offsetSign;//���V��V
    public float offsetStrength = 0.5f;//���V�O��
    public float jitterStrength = 0.1f;//�n�̤O��
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsetSign = Random.value > 0.5f ? 1 : -1;
    }

    // Update is called once per frame
    public void Move(Transform self, Transform player, Rigidbody2D rb, float speed)
    {
        // 1. �򥻤�V�]�ª��a�^
        Vector2 direction = (player.position - self.position).normalized;

        // 2. �]��������V�]������ direction�^
        Vector2 sideOffset = Vector2.Perpendicular(direction) * offsetSign; // offsetSign �O -1 �� 1

        // 3. �H���ݰʡ]���W�h�P�^
        Vector2 jitter = Random.insideUnitCircle * jitterStrength;

        // 4. �̲פ�V�[�
        Vector2 desiredDirection = (direction + sideOffset * offsetStrength + jitter).normalized;

        // 5. ������V
        Vector2 currentDirection = rb.linearVelocity.magnitude > 0.1f ? rb.linearVelocity.normalized : direction;
        Vector2 newDirection = Vector2.Lerp(currentDirection, desiredDirection, turnSpeed * Time.fixedDeltaTime);
        rb.linearVelocity = speed * newDirection;
    }
}
