using UnityEngine;

public class Dark_Spark_Controller : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 moveDirection = Vector2.right;

    private Rigidbody2D rb;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // 讓 prefab 轉向運動方向 ✅
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = moveDirection * speed;

        Destroy(gameObject, 5f); // 自動消失
    }

    private bool hasHit = false;

private void OnTriggerEnter2D(Collider2D collision)
{
    if (hasHit) return;

    if (collision.CompareTag("Player"))
    {
        hasHit = true;

        Player_Property player = collision.GetComponent<Player_Property>();
        if (player != null)
        {
            player.takedamage(10, transform.position);
        }

        Destroy(gameObject);
    }

    if (collision.CompareTag("wall"))
    {
        hasHit = true;
        Destroy(gameObject);
    }
}

}
