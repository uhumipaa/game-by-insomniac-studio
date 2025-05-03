using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
public class enemy_property : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

        public int max_health;
        public int current_health;
        public int def;
        public int atk;
        public float attack_range;
        public float speed;

        [SerializeField] private SpriteRenderer spriteRender;
        [SerializeField] private UnityEvent healthChanged;
        [SerializeField] private healthbar healthbar;

        private Knockback knockback;
        private NanoMachine_Son nanoMachine; //加入無敵系統
        public PlayerStats playerstats;

        public KillEnemyStats stats;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.Addenemy(gameObject);
            Debug.Log("ya");
        }
        
    }

    public void generaterandomstatus(EnemyData enemyData,int level)
    {
        max_health = enemyData.baseHP + (enemyData.HPpara * level);
        atk = enemyData.baseatk + (enemyData.atkpara * level);
        def = enemyData.basedef + (enemyData.defpara * level);
        Debug.Log($"生成了 {enemyData.enemyname}，等級 {level}，HP {max_health}，攻擊 {atk}");
    }
    void Start()
        {
            current_health = max_health;
             knockback = GetComponent<Knockback>();
            healthbar.initial(); //血量條初始化
            nanoMachine = GetComponent<NanoMachine_Son>(); //抓無敵系統
        }

        // 讓其他程式讀取目前的血量
        public int ReadValue
        {
            get { return current_health; }
        }

        public void takedamage(int damage , Vector2 attackerPos)
        {
            // ⭐ 如果正在無敵，就不受傷
            if (nanoMachine != null && nanoMachine.IsInvincible())
            {
                return;
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
                stats.kill();
                die();
            }
        }
        void die()
        {
            if (EnemyManager.instance != null)
                {
                    EnemyManager.instance.removeenemy(gameObject);
                }
            else {
                Debug.Log("lol");
        }            
            Debug.Log("GG");
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
    }
