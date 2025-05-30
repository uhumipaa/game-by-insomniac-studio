using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class playerhealthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Player_Property property;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            property = player.GetComponent<Player_Property>();
            if (property != null)
            {
                initial(); // <-- 場景載入後自動初始化
            }
        }
    }

    // 更新更新血量數值
    public void UpdateUI()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            property = playerGO.GetComponent<Player_Property>();
        }
        healthBar.maxValue = property.max_health;
        healthBar.value = property.current_health;
        Debug.Log("haha");
    }
    void Update()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            property = playerGO.GetComponent<Player_Property>();
        }
        healthBar.maxValue = property.max_health;
        healthBar.value = property.current_health;
    }

    // 初始化血量數值
    public void initial()
    {
        property = FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>();
        healthBar.maxValue = property.current_health;
        healthBar.value = property.current_health;
    }
}
