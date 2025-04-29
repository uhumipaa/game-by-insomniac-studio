using UnityEngine;

public class bubbletracker : MonoBehaviour
{
    public float speed = 3f;                 // 正常速度
    public float accelerateSpeed = 6f;       // 靠近時加速
    public float accelerateDistance = 2f;    // 距離小於這個就加速
    public float lifetime = 5f;              // 泡泡壽命（秒）
    public GameObject explosionEffect;       // 爆炸特效 Prefab

    private Transform target;
    private float lifeTimer;
    public int damage;
    private Vector2 actual_direction;
    private Player_Property player;

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
    }

    void Start()
    {
        lifeTimer = lifetime;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Property>(); 
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        float currentSpeed = (distance <= accelerateDistance) ? accelerateSpeed : speed;

        // 計算方向
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        actual_direction = direction;

        // 移動泡泡
        transform.position += (Vector3)(direction * currentSpeed * Time.deltaTime);

        // 追蹤玩家
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 壽命倒數
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //判斷是否碰到玩家
        {
            Explode();
            player.takedamage(damage, actual_direction);
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // 一秒後銷毀特效
        }
        Destroy(gameObject); // 銷毀泡泡
    }
}
