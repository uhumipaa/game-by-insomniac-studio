using UnityEngine;
using UnityEngine.SceneManagement;
public class WaterTool : MonoBehaviour
{
    private GameManager gm;
    public static WaterTool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject); // 如果需要
    }


    public bool TryWater(Vector3Int position, Inventory.Slot selectedSlot)
    {
        if (selectedSlot.itemData == null) return false;
        Debug.Log($"🎯 當前物品: {selectedSlot.itemData.itemName}, 類型: {selectedSlot.itemData.type}, 數量: {selectedSlot.count}");
        // 檢查該格是否已經種田
        if (!FarmManager.instance.HasFarmTile(position)) return false;

        var tileData = FarmManager.instance.GetFarmTileData(position);

        // 檢查是否已經澆水
        if (tileData.isWatered) return false;

        // 檢查是否選到物品
        if (selectedSlot == null || string.IsNullOrEmpty(selectedSlot.itemData.itemName)) return false;
        if (selectedSlot.count <= 0) return false;

        // 檢查是否為水
        if (selectedSlot.itemData.type != ItemType.Water) return false;

        // 扣除水的數量
        selectedSlot.count--;

        // 執行澆水
        FarmManager.instance.WaterTile(position);

        return true;
    }
}
