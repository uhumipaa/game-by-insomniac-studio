using UnityEngine;

public class playerstatus : MonoBehaviour
{

    public int attack;
    public int defense;
    public int maxHP;
    public int magic_power;
    public float cooldownMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        attack =  2;
        defense = 3;
        maxHP = 50;
        magic_power = 10;
        cooldownMultiplier = 1f;
    }

    // Update is called once per frame
    public void add_status(ArmorRewardData data)
    {
        attack += data.add_atk;
        defense += data.add_def;
        maxHP += data.add_HP;
        magic_power += data.add_magic;
        cooldownMultiplier *= data.diff_cd;
        if (cooldownMultiplier < 0.4f)
        {
            cooldownMultiplier = 0.4f;
        }
    } 
}
