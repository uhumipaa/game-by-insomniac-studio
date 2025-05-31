using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();
    [SerializeField] private CanvasGroup canvasGroup; // ✅ 用來控制顯示/隱藏

    private Slot_UI selectedSlot;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Debug.Log($"🧩 抓到 CanvasGroup 嗎？{canvasGroup != null}");
        }
           

    }

    private void Start()
    {
        SelectSlot(0); // 開始時選第 0 格
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"🟢 Toolbar_UI 啟用於場景：{currentScene}");

        UpdateVisibility(currentScene);
    }

    private void OnDisable()
    {
        Debug.Log("🔴 Toolbar_UI 被關閉！");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"🧭 場景載入：{scene.name}");
        UpdateVisibility(scene.name);
    }

    private void UpdateVisibility(string sceneName)
    {
        Debug.Log($"🧪 檢查場景：{sceneName}");
        bool shouldShow = sceneName != "shop";

        Debug.Log($"🔧 是否顯示 Toolbar：{shouldShow}");
        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
        canvasGroup.interactable = shouldShow;
    }

    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 10)
        {
            if (selectedSlot != null)
                selectedSlot.SetHighlight(false);

            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);

            InventoryManager.Instance.toolbar.SelectSlot(index);
        }
    }

    private void CheckAlphaNumericKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SelectSlot(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SelectSlot(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SelectSlot(9);
    }

    public void EnsureToolbarVisible()
    {
        UpdateVisibility(SceneManager.GetActiveScene().name);
    }
}
