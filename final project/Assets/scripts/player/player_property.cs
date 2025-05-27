using System;
using UnityEngine;
using UnityEngine.Events;
public class Player_Property : MonoBehaviour
{
    
    public int max_health;
    public int current_health;
    public int atk;
    public int magic_atk;
    public int def;
    public float attack_range;
    public float attack_time;
    public float speed;
    public int Luck;


    private Knockback knockback;
    private SuperStarEffect SuperStarEffect; 
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private UnityEvent healthChanged;
    [SerializeField] private playerhealthbar healthbar;
    public ExpAddUI expAddUI;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        current_health = max_health;
        knockback = GetComponent<Knockback>();
        SuperStarEffect = GetComponent<SuperStarEffect>(); //無敵和閃爍功能
        healthbar = FindAnyObjectByType<playerhealthbar>()?.GetComponent<playerhealthbar>();
        if (healthbar != null)
        {
            healthbar.initial(); //血量條初始化
        }
    }

    // 讓其他程式讀取目前的血量
    public int ReadValue()
    {
        return current_health; 
    }
    

    // 經驗點數屬性加成
    public void AtkAdd()
    {
        if (expAddUI.minuspoint() >= 0)
        {
            PlayerStatusManager.instance.attack += 1;
            PlayerStatusManager.instance.magic_power += 1;
            update_property();
        }
    }
    public void DefAdd()
    {
        if (expAddUI.minuspoint() >= 0)
        {
            PlayerStatusManager.instance.defense += 1;
            update_property();
        }
    }
    public void HPAdd()
    {
        if(expAddUI.minuspoint() >= 0) {
            PlayerStatusManager.instance.maxHP += 1;
            update_property();
        }
    }
    public void update_property()
    {
        if (PlayerStatusManager.instance.maxHP - max_health > 0)
        {
            current_health += PlayerStatusManager.instance.maxHP - max_health;
        }
        max_health = PlayerStatusManager.instance.maxHP;
        if (max_health < current_health)
        {
            current_health = max_health;
        }
        atk = PlayerStatusManager.instance.attack;
        def = PlayerStatusManager.instance.defense;
        magic_atk = PlayerStatusManager.instance.magic_power;
        speed = PlayerStatusManager.instance.speed;
        Luck = PlayerStatusManager.instance.Luck;
    }
    // 受傷
    public void takedamage(int damage, Vector2 attackerPos)
    {
        // 判斷還是不是無敵
        if (SuperStarEffect != null && SuperStarEffect.IsInvincible())
        {
            return;
        }

        int actual_def = UnityEngine.Random.Range(def - 5, def + 6);
        int actual_damage = Mathf.Max(damage - actual_def, 0);
        current_health -= actual_damage;
        // healthChanged.Invoke();
        if (healthbar != null)
        {
            healthbar.UpdateUI();
        }
        Debug.Log($"takedamage; {actual_damage} now health: {current_health}");

        // 擊退效果
        if (knockback != null)
        {
            knockback.ApplyKnockback(attackerPos);
            Debug.Log("knockback");
        }

        // 在受傷後啟動無敵
        if (SuperStarEffect != null)
        {
            SuperStarEffect.StartSuperstar();
        }

        if (current_health < 0)
        {
            die();
        }
    }
    void die()
    {
        Debug.Log("���`�G�G");
    }
}
