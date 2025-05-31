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
    

    [Header("每個階段所需通關層數")]
    public int sproutFloor;  // 發芽(0 -> 1)
    public int growFloor;    // 成長（狀態1 → 2）
    public int matureFloor;  // 成熟（狀態2 → 3）
    public int harvestFloor;
    //public int daysToMature;
    //public int daysToHarvest;
}
