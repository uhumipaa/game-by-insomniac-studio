using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TilemapTileCleaner : MonoBehaviour
{
    public Tilemap targetTilemap;
    public string targetTileName = "interable_visible";
    public Vector2Int scanMin = new Vector2Int(-20, -20);
    public Vector2Int scanMax = new Vector2Int(20, 20);

    [ContextMenu("一鍵清除 interable_visible")]
    public void CleanTiles()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("請先指定要清除的 Tilemap！");
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
                    targetTilemap.SetTile(pos, null);
                    Debug.Log($"🗑 已清除 {targetTileName} at {pos}");
                    count++;
                }
            }
        }

        if (count == 0)
        {
            Debug.Log($"✔ 沒發現 {targetTileName}，不需要清理！");
        }
        else
        {
            Debug.Log($"⚠ 已清理 {count} 個 {targetTileName}，完成！");
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(targetTilemap);
#endif
    }
}
