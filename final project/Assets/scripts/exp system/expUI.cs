using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class expUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI levelText;
    public Slider expSlider;
    
    //經驗條與等級文字更新
    void Update()
    {
        levelText.text = "" + playerStats.level;
        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
}