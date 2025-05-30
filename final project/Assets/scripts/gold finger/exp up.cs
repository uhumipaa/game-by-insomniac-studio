using UnityEngine;

public class expup : MonoBehaviour
{
    public int Reward = 500; // 經驗

    public void upexp()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        if (player != null)
        {
            player.GainExp(Reward);
        }
    }
}
