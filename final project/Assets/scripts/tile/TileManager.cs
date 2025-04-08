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
}
