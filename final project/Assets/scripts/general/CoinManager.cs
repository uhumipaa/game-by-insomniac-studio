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
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;       
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryFindCoinText(); // 每次進新場景自動找一次
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //尋找場景上顯示金額的物件
    private void TryFindCoinText()
    {
        GameObject textObj = GameObject.Find("CoinText");
        if (SceneManager.GetActiveScene().name == "shop")
        {
            textObj = GameObject.Find("CoinTextShop");
        }
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
    public void Start()
    {
        //將runtime金額設為初始金額
        coins = initialCoins;

        UpdateCoinText();
    }

    //取得金額數
    public float GetCoins()
    {
        return coins;
    }

    //增加金額
    public void AddCoins(float amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    //花錢時減少金額
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

    //金額刷新
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

    //金額存檔
    public void SaveData(ref SaveData saveData)
    {
        saveData.coin = coins;
    }

    //金額讀檔
    public void LoadData(SaveData saveData)
    {
        coins = saveData.coin;
    }

}
