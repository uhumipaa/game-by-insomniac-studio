using UnityEngine;

public class BossHealthBarFollow : MonoBehaviour
{
    [SerializeField] private Transform bossRoot;

    void LateUpdate()
    {
        if (bossRoot != null)
        {
            // 讓血條永遠正面朝右
            Vector3 worldScale = transform.lossyScale;

            // 把世界縮放轉成正X軸（反轉回來）
            if (bossRoot.localScale.x < 0 && worldScale.x < 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = -Mathf.Abs(localScale.x);  // 把localScale.x反回去
                transform.localScale = localScale;
            }
            else if (bossRoot.localScale.x > 0 && worldScale.x < 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);   // 保持正面
                transform.localScale = localScale;
            }
        }
    }
}