using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DropManager : MonoBehaviour
{
    public static DropManager instance;
    public GameObject dropItemPrefab;
    private string savePath;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 確保只保留一個
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath,
                                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_drop_items.json"
                                );
        LoadDroppedItems();
    }

    void OnApplicationQuit()
    {
        SaveDroppedItems();
    }

    public void SaveDroppedItems()
    {
        //抓取路徑
        savePath = Path.Combine(Application.persistentDataPath,
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_drop_items.json"
        );
        
        var drops = new List<DropItemData>();
        foreach (var item in GameObject.FindGameObjectsWithTag("DropItem"))
        {
            var collectable = item.GetComponent<Collectable>();
            if (collectable != null && collectable.item.data != null)
            {
                drops.Add(new DropItemData
                {
                    itemType = collectable.item.data.type,
                    position = item.transform.position,
                    amount = 1
                });
            }
        }

        var saveData = new DropItemSaveData { dropItems = drops };
        File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
        Debug.Log($"🔁 掉落物儲存路徑：{savePath}");
        Debug.Log("場上物件寫入成功");
    }

    public void LoadDroppedItems()
    {
        //抓取路徑
        savePath = Path.Combine(Application.persistentDataPath,
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_drop_items.json"
        );

        Debug.Log($"🔁 嘗試載入掉落物：{savePath}");
        foreach (var obj in GameObject.FindGameObjectsWithTag("DropItem"))
        {
            Destroy(obj);
        }

        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        var saveData = JsonUtility.FromJson<DropItemSaveData>(json);
        foreach (var data in saveData.dropItems)
        {
            // 避免生成預設 itemType（ItemType.None 或 0）
            if ((int)data.itemType == 0)
            {
                Debug.LogWarning("⛔ 忽略無效掉落物（itemType 為 0）");
                continue;
            }

            GameObject item = Instantiate(dropItemPrefab, data.position, Quaternion.identity);
            var collectable = item.GetComponent<Collectable>();
            if (collectable != null)
            {
                var itemData = ItemDatabase.GetItemData(data.itemType);
                if (itemData != null)
                {
                    collectable.SetItemData(itemData, data.amount);
                }
                else
                {
                    Debug.LogWarning($"❌ 無法找到 ItemData：{data.itemType}");
                }
            }
            else
            {
                Debug.LogWarning("❌ 掉落物 prefab 上缺少 Collectable 腳本");
            }
        }
    }

    public void ClearSavedDrops()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
