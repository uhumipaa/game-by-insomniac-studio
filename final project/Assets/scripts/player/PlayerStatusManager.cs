using UnityEngine;
[System.Serializable]
public class PlayerStatusData
{
    public int attack;
    public int defense;
    public int maxHP;
    public int magic_power;
    public float speed;
    public int Luck;
    public PlayerStatusData ()
    {
        attack = 15;
        defense = 15;
        maxHP = 50;
        magic_power = 15;
        speed = 3;
        Luck = 10;
    }
}
public class PlayerStatusManager : MonoBehaviour,ISaveData  
{
    public static PlayerStatusManager instance;
    public PlayerStatusData playerStatusData;
    private Player_Property property;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        property = FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>();
        Initialize();
        property.update_property();
        
        
    }
    private void Initialize()
    {
        playerStatusData.attack = 15;
        playerStatusData.defense = 15;
        playerStatusData.maxHP = 50;
        playerStatusData.magic_power = 15;
        playerStatusData.speed = 3;
        playerStatusData.Luck = 10;
    }

    // Update is called once per frame
    public void add_status(ItemData data)
    {
        playerStatusData.attack += data.buffATK;
        playerStatusData.defense += data.buffDEF;
        playerStatusData.maxHP += data.maxHP;
        if (playerStatusData.maxHP <= 0)
        {
            playerStatusData.maxHP = 1;
        }
        playerStatusData.magic_power += data.buffMP;
        playerStatusData.speed += data.buffSP;
        if (property != null)
            property.update_property();
    }
    public void diff_status(ItemData data)
    {
        playerStatusData.attack -= data.buffATK;
        playerStatusData.defense -= data.buffDEF;
        playerStatusData.maxHP -= data.maxHP;
        if (playerStatusData.maxHP <= 0)
        {
            playerStatusData.maxHP = 1;
        }
        playerStatusData.magic_power -= data.buffMP;
        playerStatusData.speed -= data.buffSP;
        if (property != null) 
            property.update_property();
    }

    public void SaveData(ref SaveData saveData)
    {
        saveData.playerStatusData = this.playerStatusData;
    }

    public void LoadData(SaveData saveData)
    {
        this.playerStatusData = saveData.playerStatusData;
    }

}
