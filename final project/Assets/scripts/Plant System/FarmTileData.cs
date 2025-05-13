using UnityEngine;

public class FarmTileData
{
    public Vector3Int position;
    public int state; // 1=發芽 2=成長 3=準備收成

    public FarmTileData(Vector3Int pos, int initialState)
    {
        position = pos;
        state = initialState;
    }
}
