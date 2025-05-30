using System.Collections.Generic;
using System.Collections;

using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager instance;
    public TileManager tileManager;
    private Dictionary<Vector3Int, FarmTileData> farmTiles = new Dictionary<Vector3Int, FarmTileData>();
    private GameManager gm;
    private void Awake()
    {
        instance = this;
        gm = FindFirstObjectByType<GameManager>();
        tileManager = gm.tileManager;
    }

    private void Start()
    {
        StartCoroutine(CheckGrowthPeriodically());
    }

    private IEnumerator CheckGrowthPeriodically()
    {
        while (true)
        {
            AutoGrowAllTiles(); // 檢查是否成長
            yield return new WaitForSeconds(10f); // 每 10 秒跑一次
        }
    }

    public void AddFarmTile(Vector3Int pos, CropData cropData)
    {
        if (!farmTiles.ContainsKey(pos))
        {
            farmTiles.Add(pos, new FarmTileData(pos, 0, cropData)); // 初始狀態是0(剛播種)
            UpdateTileVisual(farmTiles[pos]);
        }
    }

    public bool TryGrowTile(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
        {
            var tileData = farmTiles[pos];
            var crop = tileData.cropData;
            int floor = TowerManager.Instance.finishfloorthistime;

            switch (tileData.state)
            {
                case 0: // 播種 -> 發芽
                    if (floor >= crop.sproutFloor)
                    {
                        tileData.state = 1;
                        UpdateTileVisual(tileData);
                        return true;
                    }
                    break;
                case 1: // 發芽 → 成長
                    if (floor >= crop.growFloor)
                    {
                        tileData.state = 2;
                        UpdateTileVisual(tileData);
                        return true;
                    }
                    break;
                case 2: // 成長 → 成熟
                    if (floor >= crop.matureFloor)
                    {
                        tileData.state = 3;
                        UpdateTileVisual(tileData);
                        return true;
                    }
                    break;
                case 3: //成熟 -> 收成
                    if (floor >= crop.harvestFloor)
                    {
                        TryHarvestTile(pos);
                        return true;
                    }
                    break;
                default: //state == 0
                    tileManager.ClearTile(tileData.position); // 播種時清除圖像
                    break;
            }

        }

        return false;
    }

    private void UpdateTileVisual(FarmTileData tileData)
    {
        switch (tileData.state)
        {
            case 1:
                tileManager.SetCropTile(tileData.position, tileData.cropData.sproutTile); // 發芽Tile
                break;
            case 2:
                tileManager.SetCropTile(tileData.position, tileData.cropData.matureTile); // 成長Tile
                break;
            case 3:
                tileManager.SetCropTile(tileData.position, tileData.cropData.harvestTile); // 準備收成Tile
                break;
        }
    }

    public bool HasFarmTile(Vector3Int pos)
    {
        return farmTiles.ContainsKey(pos);
    }

    //田地收成
    public bool TryHarvestTile(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
        {
            var tileData = farmTiles[pos];

            //生成收成物到地圖
            Vector3 worldPos = tileManager.cropTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f); // 讓物品出現在 tile 中央
            GameObject harvestItem = GameObject.Instantiate(tileData.cropData.harvestPrefab, worldPos, Quaternion.identity);

            Collectable collectable = harvestItem.GetComponent<Collectable>();
            if (collectable != null)
            {
                collectable.SetItemData(tileData.cropData.harvestItemData);
            }

            //清除 tile
            tileManager.ClearTile(pos);

            //移除田地資料
            farmTiles.Remove(pos);

            // Debug.Log($"收成 {tileData.cropData.cropName} 作物完成！");
            return true;

        }

        //Debug.Log("這塊田還不能收成！");
        return false;
    }

    //取得田地資料
    public FarmTileData GetFarmTileData(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
            return farmTiles[pos];
        else
            return null;
    }

    public void AutoGrowAllTiles()
    {
        List<Vector3Int> keys = new List<Vector3Int>(farmTiles.Keys);

        foreach (var pos in keys)
        {
            TryGrowTile(pos);
        }
    }


}
