using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FarmTileData
{
    public Vector3Int position;
    public int state; // 1=發芽 2=成長 3=準備收成
    public CropData cropData;
    public GrowthProgressBar progressUI;
    public bool isWatered = false;//澆水狀態
    public GameObject waterIcon; // 顯示水滴的圖示

    public FarmTileData(Vector3Int pos, int initialState, CropData crop)
    {
        position = pos;
        state = initialState;
        cropData = crop;

    }
}

[System.Serializable]
public class FarmTileSaveData
{
    public int x;
    public int y;
    public int z;
    public int state;
    public string cropName;
    
}

[System.Serializable]
public class FarmSaveData
{
    public List<FarmTileSaveData> allTiles = new List<FarmTileSaveData>();
}
