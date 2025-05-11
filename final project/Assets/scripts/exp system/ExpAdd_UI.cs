using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpAddUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    private bool isInventoryVisible = false;
    private int point = 10;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI DefText;
    public TextMeshProUGUI HpText;
    public Player_Property player_Property;

    void Start()
    {
        inventoryPanel.SetActive(isInventoryVisible);
    }

    // 按p鍵開啟加點介面
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isInventoryVisible = !isInventoryVisible;
            inventoryPanel.SetActive(isInventoryVisible);
        }
        // 更新文字
        pointText.text = "point : " + point;
        AtkText.text = "ATK : " + player_Property.atk;
        DefText.text = "DEF : " + player_Property.def;
        HpText.text = "HP : " + player_Property.max_health;
    }
    public void addpoint(){
        point++;
    }

    public int minuspoint(){
        if (point > 0){
            point--;
            return point;
        }
        return -1;
    }
}