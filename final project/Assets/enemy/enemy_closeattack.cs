using UnityEngine;

public class enemy_closeattack : MonoBehaviour
{
    private Transform player;
    private Player_Property player_Property;

    public float attack_range;
    public float attack_cd;
    private float lastattacktime;
    public float attack_time;
    public bool attacking { get; private set; }
    public int damage;
    private float lastAttackTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null){
            player_Property = player.GetComponent<Player_Property>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attack_range&&attacking==false&&(Time.time-lastattacktime)>attack_cd)
        {
            int actual_damage = UnityEngine.Random.Range(damage - 10, damage + 10);
            player_Property.takedamage(actual_damage , transform.position);
            attacking = true;
            lastattacktime = Time.time;
        }
        else if((Time.time - lastattacktime)> attack_time)
        {
            attacking = false;
        }
    }
}
