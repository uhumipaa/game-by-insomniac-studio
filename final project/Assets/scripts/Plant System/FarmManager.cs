using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager instance;
    public TileManager tileManager;

    private Dictionary<Vector3Int, FarmTileData> farmTiles = new Dictionary<Vector3Int, FarmTileData>();

    private void Awake()
    {
        instance = this;
        tileManager = GameManager.instance.tileManager;
    }

    public void AddFarmTile(Vector3Int pos)
    {
        if (!farmTiles.ContainsKey(pos))
        {
            farmTiles.Add(pos, new FarmTileData(pos, 1)); // 初始狀態是1(發芽)
            UpdateTileVisual(farmTiles[pos]);
        }
    }

    public bool TryGrowTile(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
        {
            var tileData = farmTiles[pos];
            tileData.state++;

            UpdateTileVisual(tileData);

            if (tileData.state >= 3) // 收成狀態
            {
                Debug.Log("這塊田可以收成了！");
                farmTiles.Remove(pos);//移除成長資料，不再推進狀態
            }

            return true;
        }

        return false;
    }

    private void UpdateTileVisual(FarmTileData tileData)
    {
        switch (tileData.state)
        {
            case 1:
                tileManager.SetCropTile(tileData.position, tileManager.sproutTile); // 發芽Tile
                break;
            case 2:
                tileManager.SetCropTile(tileData.position, tileManager.matureTile); // 成長Tile
                break;
            case 3:
                tileManager.SetCropTile(tileData.position, tileManager.readyToHarvestTile); // 準備收成Tile
                break;
        }
    }

    public bool HasFarmTile(Vector3Int pos)
    {
        return farmTiles.ContainsKey(pos);
    }
}
