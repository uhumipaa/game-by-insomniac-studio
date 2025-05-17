using UnityEngine;
using UnityEngine.UI;
public class healthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private enemy_property property;

    // 更新更新血量數值
    public void UpdateUI()
    {
        healthBar.value = property.ReadValue;
    }

    // 初始化血量數值
    public void initial()
    {
        healthBar.maxValue = property.ReadValue;
        healthBar.value = property.ReadValue;
    }
}
