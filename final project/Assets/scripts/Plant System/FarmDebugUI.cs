using UnityEngine;
using UnityEngine.UI;

public class FarmDebugUI : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;

    private void Start()
    {
        if (saveButton == null || loadButton == null)
        {
            Debug.LogError("❌ [FarmDebugUI] 請指定兩個按鈕欄位！");
            return;
        }

        saveButton.onClick.AddListener(() =>
        {
            FarmManager.instance.SaveFarmTilesToFile();
            Debug.Log("💾 已手動儲存農地資料");
        });

        loadButton.onClick.AddListener(() =>
        {
            FarmManager.instance.LoadFarmTilesFromFile();
            Debug.Log("📂 已手動載入農地資料");
        });
    }
}
