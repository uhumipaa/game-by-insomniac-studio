using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;
    public TileManager tileManager;
    public FarmManager farmManager;
    public List<CropData> allCrops; // 所有作物資料
    private GameManager gm;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 防止重複產生
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 如果你希望它跨場景存在
        gm = FindFirstObjectByType<GameManager>();
        tileManager = gm.tileManager;

        Debug.Log("🟢 PlantManager 初始化成功：" + gameObject.name);
    }

    public bool TryPlant(Vector3Int position, string tileName, Inventory.Slot selectedSlot)
    {
        //Debug.Log($"檢查格子座標 {position}，Tile 名稱：{tileName}");

        //檢查是不是田地
        if (tileName != "interable_visible") return false;

        //檢查是否有選到種子
        if (selectedSlot == null || string.IsNullOrEmpty(selectedSlot.itemData.itemName)) return false;
        if (selectedSlot.count <= 0) return false;

        // 根據種子名稱找對應的作物資料
        CropData cropToPlant = allCrops.Find(crop => crop.cropName == selectedSlot.itemData.itemName);
        //Debug.Log($"選擇的是{selectedSlot.itemData.itemName}");

        if (cropToPlant == null) //如果找不到
        {
            //Debug.Log("找不到對應作物資料，無法種植");
            return false;
        }

        //判斷有沒有種作物
        if (FarmManager.instance.HasFarmTile(position))
        {
            //Debug.Log("這格已經種過了，不能重複種植！");
            return false;
        }

        //格子裡的種子數減一
        selectedSlot.count--;

        // ⏳ 延遲一幀再種田，避免使用時欄位尚未注入
        StartCoroutine(DelayedPlant(position, cropToPlant));

        //種下種子後變成發芽狀態
        //tileManager.SetCropTile(position, cropToPlant.sproutTile);

        /*// 記錄田地狀態
        FarmManager.instance.AddFarmTile(position, cropToPlant);*/
        return true;
    }

    private IEnumerator DelayedPlant(Vector3Int pos, CropData crop)
    {
        yield return null; // 等一幀，確保 FarmManager 裡的 prefab 欄位已經準備好
        if (FarmManager.instance.farmTileProgressBarPrefab == null)
        {
            Debug.LogError("❌ ProgressBar Prefab 尚未設定！");
        }

        FarmManager.instance.AddFarmTile(pos, crop);
    }
}