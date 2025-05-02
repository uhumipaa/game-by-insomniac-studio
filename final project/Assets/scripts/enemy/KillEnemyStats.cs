using UnityEngine;

public class KillEnemyStats : MonoBehaviour
{
    public int expReward = 30;

    public void kill()
    {
        // 取得場景中的 PlayerStats
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            player.GainExp(expReward);
        }

        Destroy(gameObject);
    }
}
