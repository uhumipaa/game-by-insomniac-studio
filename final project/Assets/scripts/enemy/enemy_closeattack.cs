using UnityEngine;

public class enemy_closeattack : MonoBehaviour
{
    private Transform player;
    public float attack_range;
    public float attack_cd;
    public bool attacking { get; private set; }
    private float lastattacktime;
    public float attack_time;
    public int damage;
    private Player_Property player_Property;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        player_Property = player.GetComponent<Player_Property>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attack_range&&attacking==false&&(Time.time-lastattacktime)>attack_cd)
        {
            int actual_damage = UnityEngine.Random.Range(damage - 10, damage + 10);
            player_Property.takedamage(actual_damage);
            attacking = true;
            lastattacktime = Time.time;
        }
        else if((Time.time - lastattacktime)> attack_time)
        {
            attacking = false;
        }
    }
}
