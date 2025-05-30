using UnityEngine;

public class StartTakoCD : MonoBehaviour
{
    public Animator cdAnimator;     // 👉 冷卻動畫用的 Animator（掛在 SkillCD 圖示上）
    public string triggerName = "StartCD"; // 👉 觸發冷卻動畫的參數名稱

    // 呼叫這個方法就能播放動畫
    public void PlayCooldownAnimation()
    {
        if (cdAnimator != null)
        {
            cdAnimator.SetTrigger(triggerName); // 👉 播放動畫
        }
        else
        {
            Debug.LogWarning("cdAnimator 未設定！");
        }
    }
}
