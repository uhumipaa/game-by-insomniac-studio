using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStats playerStats;

    public TextMeshProUGUI levelText;
    public Slider expSlider;

    void Update()
    {
        levelText.text = "Level: " + playerStats.level;
        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
}