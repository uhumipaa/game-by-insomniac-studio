using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour,ISaveData
{
    public static CoinManager instance;

    [Header("Initial Setup")]
    [SerializeField] private float initialCoins = 1000f;

    [Header("Runtime Data")]
    [SerializeField] private float coins;
    [SerializeField] private Text coinsText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);//跨場景保存

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryFindCoinText(); // 每次進新場景自動找一次
    }

    private void TryFindCoinText()
    {
        // 嘗試找場景中名為 CoinText 的物件
        GameObject textObj = GameObject.Find("CoinText");
        if (textObj != null)
        {
            coinsText = textObj.GetComponent<Text>();
            UpdateCoinText();
        }
        else
        {
            Debug.LogWarning("找不到場景中的 CoinText UI！");
        }
    }
    private void Start()
    {
        //將runtime金額設為初始金額
        coins = initialCoins;

        UpdateCoinText();
    }

    public float GetCoins()
    {
        return coins;
    }

    public void AddCoins(float amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    public bool SpendCoins(float amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCoinText();
            return true;
        }
        return false;
    }

    private void UpdateCoinText()
    {
        if (coinsText != null)
            coinsText.text = "      " + coins.ToString();
    }

    //設定text
    public void RegisterCoinText(Text newText)
    {
        coinsText = newText;
        UpdateCoinText();
    }

    public void SaveData(ref SaveData saveData)
    {
        saveData.coin = coins;
    }

    public void LoadData(SaveData saveData)
    {
        coins = saveData.coin;
    }

}
