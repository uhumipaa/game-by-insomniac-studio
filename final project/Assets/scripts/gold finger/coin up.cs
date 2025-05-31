using UnityEngine;

public class coinup : MonoBehaviour
{
    public void coingoldfinger()
    {
        if (CoinManager.instance != null)
        {
            CoinManager.instance.AddCoins(1000);
        }
    }
}
