using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class expUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI current_exp_Text;
    public TextMeshProUGUI next_exp_Text;
    public TextMeshProUGUI currethptext;
    public TextMeshProUGUI maxhptext;
    public Slider expSlider;
    public Player_Property player_Property;


    //經驗條與等級文字更新
    void Update()
    {
        levelText.text = "" + playerStats.level;
        current_exp_Text.text = "" + playerStats.currentExp;
        next_exp_Text.text = "" + playerStats.expToNextLevel;

        currethptext.text = "" + player_Property.current_health;
        maxhptext.text = "" + player_Property.max_health;

        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
    
    void Start()
    {
        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
}