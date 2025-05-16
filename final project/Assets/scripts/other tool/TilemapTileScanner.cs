using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TilemapTileScanner : MonoBehaviour
{
    public Tilemap targetTilemap;
    public string targetTileName = "interable_visible";
    public Vector2Int scanMin = new Vector2Int(-20, -20);
    public Vector2Int scanMax = new Vector2Int(20, 20);

    [ContextMenu("掃描 Tilemap")]
    public void ScanTilemap()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("請先指定要掃描的 Tilemap！");
            return;
        }

        int count = 0;
        for (int x = scanMin.x; x <= scanMax.x; x++)
        {
            for (int y = scanMin.y; y <= scanMax.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = targetTilemap.GetTile(pos);

                if (tile != null && tile.name == targetTileName)
                {
                    Debug.Log($"❗ 發現目標 Tile：{targetTileName} at 格子 {pos}");
                    count++;
                }
            }
        }

        if (count == 0)
        {
            Debug.Log($"✔ 沒有發現 {targetTileName}，地圖乾淨！");
        }
        else
        {
            Debug.Log($"⚠ 發現 {count} 個 {targetTileName} Tile，請檢查！");
        }
    }
}