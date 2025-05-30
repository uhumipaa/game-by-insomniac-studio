using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /*void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "farm") // ⚠️請改成你農場場景的名字
        {
            Debug.Log("🌱 FarmScene 載入完成，準備刷新農地");

            StartCoroutine(DelayedFarmRefresh());
        }
    }

    private System.Collections.IEnumerator DelayedFarmRefresh()
    {
        // 等待一幀或多幀，確保 Tilemap 與其他依賴初始化完成
        yield return null;

        if (FarmManager.instance != null)
        {
            FarmManager.instance.LoadFarmTilesFromFile();
            Debug.Log("✅ FarmManager 已刷新田地狀態");
        }
        else
        {
            Debug.LogWarning("❌ FarmManager.instance 為 null，無法刷新農地");
        }
    }*/
}
