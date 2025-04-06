using UnityEngine;

public class enemy_move : MonoBehaviour
{
    public float Speed;
    private Transform player;
    public Vector2 direction { get; private set; }
    private enemy_closeattack CloseAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseAttack = GetComponent<enemy_closeattack>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || CloseAttack == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance >= CloseAttack.attack_range&&CloseAttack.attacking==false)
        {
            direction = (player.position - transform.position).normalized;
            transform.position = (Vector2)transform.position + (direction * Speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
        else
        {
            direction = Vector2.zero;
        }
    }
}
