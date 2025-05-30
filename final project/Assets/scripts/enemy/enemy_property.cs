using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;
public class enemy_property : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int max_health;
    public int current_health;
    public int def;
    public int atk;
    public float attack_range;
    public float speed;
    public float stuncd;

    public System.Action Boss_King_Death;

    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] public UnityEvent healthChanged;
    [SerializeField] public UnityEvent healthbarinitial;

    private Knockback knockback;
    private NanoMachine_Son nanoMachine; //加入無敵系統
    public KillEnemyStats stats;
    public bool isBossKingPhase2 = false;
    public bool isnightdemon = false;
    private bool isBossKingDead = false;
    private bool isDarkMagicion = false;
    public float detect_range = 5f;
    private SmartShieldEnemy killershield;

    [Header("是nightdemon才需要掛")]
    public nightdemoncontroller nightdemon;

    [Header("是Dark Magicion才需要掛")]
    public Dark_Magicion_Controller Dark_Magicion;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.Addenemy(gameObject);
            Debug.Log("ya");
        }

    }

    public void generaterandomstatus(EnemyData enemyData, int level)
    {
        max_health = enemyData.baseHP + (int)(enemyData.HPpara * level);
        atk = enemyData.baseatk + (int)(enemyData.atkpara * level);
        def = enemyData.basedef + (int)(enemyData.defpara * level);
        Debug.Log($"生成了 {enemyData.enemyname}，等級 {level}，HP {max_health}，攻擊 {atk}");
    }
    void Start()
    {
        current_health = max_health;
        knockback = GetComponent<Knockback>();
        healthbarinitial.Invoke(); //血量條初始化
        nanoMachine = GetComponent<NanoMachine_Son>(); //抓無敵系統
    }

    // 讓其他程式讀取目前的血量
    public int ReadValue
    {
        get { return current_health; }
    }

    public void takedamage(int damage, Vector2 attackerPos)
    {
        Debug.Log("12345464");
        // 如果正在無敵，就不受傷
        if (nanoMachine != null && nanoMachine.IsInvincible())
        {
            return;
        }
        var bossController = GetComponent<BossController>();
        if (bossController != null && bossController.IsTransforming())
        {
            return;  //  變身中 → 不吃傷害
        }
        var killershield = GetComponent<SmartShieldEnemy>();
        if (killershield != null)
        {
            if (killershield.isShielded)
            {
                return;  //  護盾
            }
        }
        int actual_def = UnityEngine.Random.Range(def - 5, def + 6);
        int actual_damage = Mathf.Max(damage - actual_def, 0);
        current_health -= actual_damage;
        Debug.Log($"takedamage {actual_damage} now health; {current_health}");
        //特效與事件
        spriteRender.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo).ChangeStartValue(Color.white);//顏色從白色變成紅色再變為白色
        healthChanged.Invoke();

        // 使用 Knockback 組件
        if (knockback != null)
        {
            knockback.ApplyKnockback(attackerPos);
        }

        //扣血後啟動無敵
        if (nanoMachine != null)
        {
            nanoMachine.StartInvincibility();
        }

        if (current_health < 0)
        {
            if (isnightdemon)
            {
                nightdemon.nightdemondie();
                stats.kill();
                return;
            }
            if (!isBossKingPhase2 && Boss_King_Death != null)
            {
                Boss_King_Death.Invoke();
                isBossKingPhase2 = true;
            }
            else
            {
                if (bossController != null && bossController.currentState == BossController.BossState.Phase2)
                {
                    stats.kill();
                    bossController.StartPhase2Death(); //  呼叫Boss死亡動畫流程
                }
                if (Dark_Magicion != null)
                {
                    stats.kill();
                    // 交給 Dark Magicion 自己決定什麼時候死亡（播放動畫等等）
                    Debug.Log("由 Dark Magicion 自行處理死亡流程");
                    Dark_Magicion.Die(); // ✅ 呼叫 Dark Magicion 自訂死亡流程（播放動畫 + 延遲死亡）
                }
                else
                {
                    die();
                    stats.kill();
                }
            }
        }
    }
    void die()
    {
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.removeenemy(gameObject);
        }
        else
        {
            Debug.Log("lol");
        }
        var bossController = GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.ClearSummonedKnights();  // Boss死時清掉所有召喚物
        }
        Debug.Log("GG");
        StartCoroutine(DelayedDestroy(0.5f));


        // Debug.Log("GG");
        // gameObject.SetActive(false);
        // Destroy(gameObject);
    }
    private IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void ForceDie()
    {
        if (isBossKingDead) return;
        isBossKingDead = true;

        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.removeenemy(gameObject);
        }

        gameObject.SetActive(false);
    }
 }
