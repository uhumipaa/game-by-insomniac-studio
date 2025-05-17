using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Tile sproutTile;
    public Tile matureTile;
    public Tile harvestTile;
    public GameObject harvestPrefab;
    public ItemData harvestItemData;
    //public int daysToMature;
    //public int daysToHarvest;
}
