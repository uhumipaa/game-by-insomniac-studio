using UnityEngine;

public class enemy_property : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

        public int max_health;
        public int current_health;
        public int def;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Start()
        {
            current_health = max_health;
        }

        public void takedamage(int damage)
        {
            int actual_def = UnityEngine.Random.Range(def - 5, def + 6);
            int actual_damage = Mathf.Max(damage - actual_def, 0);
            current_health -= actual_damage;
            Debug.Log($"�Ǫ����� {actual_damage} �ˮ`�A�ثe��q {current_health}");
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
