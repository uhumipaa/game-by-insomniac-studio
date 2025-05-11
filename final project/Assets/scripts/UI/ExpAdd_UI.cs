using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    // public Button button1;
    // public Button button2;
    // public Button button3;

    private bool isInventoryVisible = false;

    void Start()
    {
        inventoryPanel.SetActive(isInventoryVisible);

        // 加入按鈕的事件處理器
        // button1.onClick.AddListener(() => OnButtonClicked("物品1"));
        // button2.onClick.AddListener(() => OnButtonClicked("物品2"));
        // button3.onClick.AddListener(() => OnButtonClicked("物品3"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isInventoryVisible = !isInventoryVisible;
            inventoryPanel.SetActive(isInventoryVisible);
        }
    }

    // void OnButtonClicked(string itemName)
    // {
    //     Debug.Log(itemName + " 被點擊");
    // }
}