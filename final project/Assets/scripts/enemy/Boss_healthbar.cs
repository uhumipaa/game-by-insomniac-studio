using UnityEngine;
using UnityEngine.UI;

public class DualHealthBarImage : MonoBehaviour
{
    [Header("UI References")]
    public Image foregroundBar;  // 紅色當前血條
    public Image backgroundBar;  // 橘色延遲血條

    [Header("Health Settings")]
    public enemy_property health;

    [Header("Effect Settings")]
    public float backgroundLerpSpeed = 2f;  // 橘色血條回落速度
    public float healLerpSpeed = 4f;        // 回血時橘色上升速度

    private void Update()
    {
        UpdateBackgroundBar();
    }

    public void Initial()
    {
        UpdateForegroundBar(true);
    }

    public void UpdateUI()
    {
        UpdateForegroundBar();
    }

    private void UpdateForegroundBar(bool instant = false)
    {
        float healthPercent = (float)health.current_health / health.max_health;
        foregroundBar.fillAmount = healthPercent;

        if (instant)
        {
            backgroundBar.fillAmount = healthPercent;
        }
        // 橘色的平滑效果交給 Update() 執行
    }

    private void UpdateBackgroundBar()
    {
        float foregroundFill = foregroundBar.fillAmount;
        float backgroundFill = backgroundBar.fillAmount;

        if (Mathf.Approximately(backgroundFill, foregroundFill)) return;

        if (backgroundFill > foregroundFill)
        {
            // 受傷：橘色慢慢降
            backgroundBar.fillAmount = Mathf.Lerp(backgroundFill, foregroundFill, Time.deltaTime * backgroundLerpSpeed);
        }
        else if (backgroundFill < foregroundFill)
        {
            // 回血：橘色快速追上
            backgroundBar.fillAmount = Mathf.Lerp(backgroundFill, foregroundFill, Time.deltaTime * healLerpSpeed);
        }
    }
}
