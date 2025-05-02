using UnityEngine;

public class KillEnemyStats : MonoBehaviour
{
    public int expReward = 30; // 擊殺經驗

    public void kill()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            player.GainExp(expReward);
        }

        Destroy(gameObject);
    }
}
