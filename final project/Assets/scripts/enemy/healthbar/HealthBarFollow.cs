using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    [SerializeField] private Transform enemyRoot;

    void LateUpdate()
    {
        if (enemyRoot != null)
        {
            // 讓血條永遠正面朝右
            Vector3 worldScale = transform.lossyScale;

            // 把世界縮放轉成正X軸（反轉回來）
            if (enemyRoot.localScale.x < 0 && worldScale.x < 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = -Mathf.Abs(localScale.x);  // 把localScale.x反回去
                transform.localScale = localScale;
            }
            else if (enemyRoot.localScale.x > 0 && worldScale.x < 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);   // 保持正面
                transform.localScale = localScale;
            }
        }
    }
}