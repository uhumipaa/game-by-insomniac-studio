using UnityEngine;
using UnityEngine.UI;

public class GrowthProgressBar : MonoBehaviour
{
    public Image fillBar; // 指向 Fill 圖片
    private FarmTileData tileData;

    public void Setup(FarmTileData data)
    {
        tileData = data;
        UpdateProgress(); // 初始化時馬上更新一次
    }

    public void UpdateProgress()
    {
        if (tileData == null || tileData.cropData == null) return;


        float progress = tileData.state /  4f; //判斷總進度
        fillBar.fillAmount = Mathf.Clamp01(progress);
    }
}
