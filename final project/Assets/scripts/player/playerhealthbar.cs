using UnityEngine;
using UnityEngine.UI;
public class playerhealthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Player_Property property;

    // 更新更新血量數值
    public void UpdateUI()
    {
        healthBar.value = property.ReadValue;
        Debug.Log("haha");
    }

    // 初始化血量數值
    public void initial()
    {
        healthBar.maxValue = property.ReadValue;
        healthBar.value = property.ReadValue;
    }
}
