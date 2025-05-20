using UnityEngine;
using UnityEngine.UI;
public class playerhealthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Player_Property property;

    // 更新更新血量數值
    public void UpdateUI()
    {
        healthBar.value = property.current_health;
        Debug.Log("haha");
    }

    // 初始化血量數值
    public void initial()
    {
        property = FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>();
        healthBar.maxValue = property.current_health;
        healthBar.value = property.current_health;
    }
}
