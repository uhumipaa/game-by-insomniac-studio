using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject coinManagerPrefab;

    void Awake()
    {
        if (CoinManager.instance == null)
        {
            Instantiate(coinManagerPrefab);
        }
    }
}

