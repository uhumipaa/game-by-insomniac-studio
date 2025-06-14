using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class Player_Property : MonoBehaviour,ISaveData
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

    static public Player_Property instance;
    private Knockback knockback;
    private SuperStarEffect SuperStarEffect; 
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private UnityEvent healthChanged;
    [SerializeField] private playerhealthbar healthbar;
    public ExpAddUI expAddUI;
    [SerializeField] private player_controler playerController;
    [SerializeField] private CanvasGroup blackScreenPanel;
    [SerializeField] private List<Animator> deathAnimators; // 可拖入多個 Animator




    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        healthbar = FindAnyObjectByType<playerhealthbar>();
        expAddUI = FindAnyObjectByType<ExpAddUI>();
        GameObject UI = GameObject.FindGameObjectWithTag("exp");
        if (UI != null)
        {
            expAddUI = UI.GetComponent<ExpAddUI>();
            healthbar = UI.GetComponent<playerhealthbar>();
        }
        else
        {
            Debug.Log("jaja");
        }
        current_health = max_health;
        knockback = GetComponent<Knockback>();
        SuperStarEffect = GetComponent<SuperStarEffect>(); //無敵和閃爍功能
        healthbar = FindAnyObjectByType<playerhealthbar>()?.GetComponent<playerhealthbar>();
        if (healthbar != null)
        {
            healthbar.initial(); //血量條初始化
        }
        update_property();
        //StartCoroutine(die());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "tower")
        {
            blackScreenPanel = GameObject.Find("blackscreen").GetComponent<CanvasGroup>();
            deathAnimators.Add(GameObject.Find("GameOver1").GetComponent<Animator>());
            deathAnimators.Add(GameObject.Find("GameOver2").GetComponent<Animator>());
            deathAnimators.Add(GameObject.Find("GameOver3").GetComponent<Animator>());
            deathAnimators.Add(GameObject.Find("GameOver4").GetComponent<Animator>());
            deathAnimators.Add(GameObject.Find("GameOver5").GetComponent<Animator>());
            deathAnimators.Add(GameObject.Find("GameOver6").GetComponent<Animator>());
            deathAnimators.Add(GameObject.Find("GameOver7").GetComponent<Animator>());
        }

        if (scene.name == "playerHome")
        {
            transform.position = SceneLoadPosition.spawnPosition; //進入新場景後定位
            Debug.Log($"玩家已定位到 {SceneLoadPosition.spawnPosition}");
        }
    }

    void Update()
    {
        healthbar = FindAnyObjectByType<playerhealthbar>();
        expAddUI = FindAnyObjectByType<ExpAddUI>();
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
            PlayerStatusManager.instance.playerStatusData.attack += 5;
            PlayerStatusManager.instance.playerStatusData.magic_power += 5;
            update_property();
        }
    }
    public void DefAdd()
    {
        if (expAddUI.minuspoint() >= 0)
        {
            PlayerStatusManager.instance.playerStatusData.defense += 1;
            update_property();
        }
    }
    public void HPAdd()
    {
        if(expAddUI.minuspoint() >= 0) {
            PlayerStatusManager.instance.playerStatusData.maxHP += 10;
            update_property();
        }
    }
    public void update_property()
    {
        if (PlayerStatusManager.instance == null)
        {
            Debug.LogError("aaaaaa");
            return;
        }
        if (PlayerStatusManager.instance.playerStatusData.maxHP - max_health > 0)
        {
            current_health += PlayerStatusManager.instance.playerStatusData.maxHP - max_health;
        }
        max_health = PlayerStatusManager.instance.playerStatusData.maxHP;
        if (max_health < current_health)
        {
            current_health = max_health;
        }
        atk = PlayerStatusManager.instance.playerStatusData.attack;
        def = PlayerStatusManager.instance.playerStatusData.defense;
        magic_atk = PlayerStatusManager.instance.playerStatusData.magic_power;
        speed = PlayerStatusManager.instance.playerStatusData.speed;
        Luck = PlayerStatusManager.instance.playerStatusData.Luck;
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
        if(Audio_manager.Instance!=null)
            Audio_manager.Instance.Play(14, "player_take_damaged", false, 0);
        healthChanged.Invoke();
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
            StartCoroutine(die());
        }
    }
    public void heal(int amount)
    {
        current_health = Mathf.Min(max_health, current_health + amount);
        Debug.Log("玩家恢復了 HP，目前：" + current_health);
    }


    private IEnumerator die()
    {
        Debug.Log("玩家死亡！開始死亡流程");
        if (blackScreenPanel != null)
            blackScreenPanel.alpha = 1f; // 立即變黑，不淡入

        foreach (Animator anim in deathAnimators)
        {
            if (anim != null)
            {
                anim.SetTrigger("GameOver"); // 或 "Pop"、"FadeIn"，視你的動畫命名
            }
        }// 播放死亡動畫

        //等待10秒
         yield return new WaitForSeconds(5f);
        //轉場景
        SceneLoadPosition.spawnPosition = new Vector3(-3f, -2f, 0f); // ✅ 你想要在新場景的出生座標
        SceneManager.LoadScene("playerHome");

    }


    public void SaveData(ref SaveData saveData)
    {
        saveData.playerposition = transform.position;
        saveData.currentscene = SceneManager.GetActiveScene().name;
    }

    public void LoadData(SaveData saveData)
    {
        transform.position = saveData.playerposition;
    }

}
