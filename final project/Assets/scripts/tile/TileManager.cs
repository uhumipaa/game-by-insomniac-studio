using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap InteractableMap;
    [SerializeField] private Tile hiddenInteractableTile;

    void Start()
    {
        foreach(var position in InteractableMap.cellBounds.allPositionsWithin)
        {
            InteractableMap.SetTile(position, hiddenInteractableTile);
        }   
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = InteractableMap.GetTile(position);

        if(tile != null)
        {
            Debug.Log("Tile Name: " + tile.name);  // 輸出 Tile 的名稱來確認
            if(tile.name == "interable_visible")
            {
                return true;
            }
        }

        return false;
    }
}
