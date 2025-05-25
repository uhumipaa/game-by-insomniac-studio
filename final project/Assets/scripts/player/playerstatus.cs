using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager instance;
    public int attack;
    public int defense;
    public int maxHP;
    public int magic_power;
    public float speed;
    public int Luck;
    private Player_Property property;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        property = FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
        Initialize();
    }
    private void Initialize()
    {
        attack = 15;
        defense = 15;
        maxHP = 50;
        magic_power = 15;
        speed = 3;
        Luck = 10;
    }

    // Update is called once per frame
    public void add_status(ItemData data)
    {
        attack += data.buffATK;
        defense += data.buffDEF;
        maxHP += data.maxHP;
        if (maxHP <= 0)
        {
            maxHP = 1;
        }
        magic_power += data.buffMP;
        speed += data.buffSP;
        if (property != null)
            property.update_property();
    }
    public void diff_status(ItemData data)
    {
        attack -= data.buffATK;
        defense -= data.buffDEF;
        maxHP -= data.maxHP;
        if (maxHP <= 0)
        {
            maxHP = 1;
        }
        magic_power -= data.buffMP;
        speed -= data.buffSP;
        if (property != null) 
            property.update_property();
    }
}
