using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FloorJump : MonoBehaviour
{
    public GameObject jumpPanel; // 控制輸入欄位和確認按鈕的區塊
    public TMP_InputField floorInputField;
    public int maxTowerFloor = 50;

    void Start()
    {
        jumpPanel.SetActive(false); // 預設隱藏
    }

    // 第一步：點「跳層」按鈕後執行
    public void ShowInputPanel()
    {
        jumpPanel.SetActive(true);
    }

    // 第二步：點「確認」按鈕後執行
    public void GoToFloor()
    {
        string input = floorInputField.text.Trim();

        if (int.TryParse(input, out int targetFloor))
        {
            if (targetFloor >= 1 && targetFloor <= maxTowerFloor)
            {
                TowerManager.Instance.currentTowerFloor = targetFloor;
                // TowerManager.Instance.backtotower = true;
                // SceneManager.LoadScene("tower");
            }
            else
            {
                Debug.LogWarning("樓層超出範圍！");
            }
        }
        else
        {
            Debug.LogWarning("請輸入有效的樓層數！");
        }
        jumpPanel.SetActive(false);
    }
}
