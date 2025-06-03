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
        attack = 30;
        defense = 10;
        maxHP = 500;
        magic_power = 30;
        speed = 4.5f;
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
        property = FindAnyObjectByType<Player_Property>()?.GetComponent<Player_Property>();
        Initialize();
        if(property!=null)property.update_property();
        
        
    }
    private void Initialize() 
    {
        playerStatusData.attack = 40;
        playerStatusData.defense = 10;
        playerStatusData.maxHP = 500;
        playerStatusData.magic_power = 30;
        playerStatusData.speed = 4.5f;
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
