using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


public class TileManager : MonoBehaviour
{
    [SerializeField] public Tilemap InteractableMap; //田地層(不會改變)
    [SerializeField] private Tile hiddenInteractableTile;
    public Tilemap cropTilemap;   // 作物層

    public TileBase sproutTile; //發芽
    public TileBase matureTile; //成長
    public TileBase readyToHarvestTile; //準備收成

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "farm")
        {
            var tilemapObj = GameObject.Find("CropTilemap");
            if (tilemapObj != null)
                cropTilemap = tilemapObj.GetComponent<Tilemap>();
            var Interactobj = GameObject.Find("InteractableMap");
            if (Interactobj != null)
                InteractableMap = Interactobj.GetComponent<Tilemap>();
        }
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        foreach (var position in InteractableMap.cellBounds.allPositionsWithin)
        {
            InteractableMap.SetTile(position, hiddenInteractableTile);
        }
    }

    public void SetCropTile(Vector3Int position, TileBase tile)
    {
        cropTilemap.SetTile(position, tile);
    }

    public string GetTileName(Vector3Int position)
    {
        if (InteractableMap != null)
        {
            TileBase tile = InteractableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }
        return "";
    }

    //清除作物並還原成原本的tile
    public void ClearTile(Vector3Int position)
    {
        cropTilemap.SetTile(position, hiddenInteractableTile);
    }

}
