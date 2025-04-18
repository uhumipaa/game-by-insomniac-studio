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

        [SerializeField] private SpriteRenderer spriteRender;
        [SerializeField] private UnityEvent healthChanged;
        [SerializeField] private healthbar healthbar;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Start()
        {
            current_health = max_health;
            healthbar.initial(); //血量條初始化
        }

        // 讓其他程式讀取目前的血量
        public int ReadValue
        {
            get { return current_health; }
        }

        public void takedamage(int damage)
        {
            int actual_def = UnityEngine.Random.Range(def - 5, def + 6);
            int actual_damage = Mathf.Max(damage - actual_def, 0);
            current_health -= actual_damage;
            Debug.Log($"�Ǫ����� {actual_damage} �ˮ`�A�ثe��q {current_health}");
            spriteRender.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo).ChangeStartValue(Color.white);//顏色從白色變成紅色再變為白色
            healthChanged.Invoke();
            if (current_health < 0)
            {
                die();
            }
        }
        void die()
        {
            Debug.Log("���`�G�G");
            gameObject.SetActive(false);//將物體的標籤關掉
        }
    }
