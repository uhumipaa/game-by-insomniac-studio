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
        var drops = new List<DropItemData>();
        foreach (var item in GameObject.FindGameObjectsWithTag("DropItem"))
        {
            var pickup = item.GetComponent<PickupItem>();
            if (pickup != null)
            {
                drops.Add(new DropItemData
                {
                    itemID = pickup.itemID,
                    position = item.transform.position,
                    amount = pickup.amount
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
        Debug.Log($"🔁 嘗試載入掉落物：{savePath}");

        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        var saveData = JsonUtility.FromJson<DropItemSaveData>(json);
        foreach (var data in saveData.dropItems)
        {
            GameObject item = Instantiate(dropItemPrefab, data.position, Quaternion.identity);
            var pickup = item.GetComponent<PickupItem>();
            pickup.itemID = data.itemID;
            pickup.amount = data.amount;
            pickup.itemData = ItemDatabase.GetItemData(data.itemID);
        }
    }

    public void ClearSavedDrops()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
