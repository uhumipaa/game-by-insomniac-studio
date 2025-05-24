using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 8f;
    public int damage = 1;

    private Player_Property player_Property;
    private enemy_property owner;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody2D NOT FOUND on Arrow prefab!");
        }
    }

    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player_Property = playerGO.GetComponent<Player_Property>();
        }
        else
        {
            Debug.LogError("❌ 找不到 Player，請確認有設定 Tag 且場上有 Player！");
        }
        if (owner == null)
        {
            owner = GetComponentInParent<enemy_property>();
        }

        Destroy(gameObject, lifeTime);
    }
    void Update()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.offset = Vector2.zero; // ✅ 永遠強制歸零
        }
    }

    public void SetOwner(enemy_property ownerProperty)
    {
        owner = ownerProperty;
    }

    public void SetDirection(Vector2 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>(); // 安全保險

        rb.linearVelocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player_Property = other.GetComponent<Player_Property>();
            if (player_Property != null)
            {
                player_Property.takedamage(owner.atk, transform.position);
            }

            Destroy(gameObject); // 命中後消失
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
