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

    public string GetTileName(Vector3Int position)
    {
        if(InteractableMap != null)
        {
            TileBase tile = InteractableMap.GetTile(position);

            if(tile != null)
            {
                return tile.name;
            }
        }
        return "";
    }
}
