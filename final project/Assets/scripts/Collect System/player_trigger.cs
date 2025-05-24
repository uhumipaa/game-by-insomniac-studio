using UnityEngine;

public class player_trigger : MonoBehaviour
{
    //public InventoryManager inventory; //刪

    public TileManager tileManager;
    private float footOffsetY = 0f;
    private GameManager gm;

    private void Start()
    {
        /*//同步資料
        inventory = GameManager.instance.inventoryManager;*/
        gm = FindFirstObjectByType<GameManager>();
        tileManager = gm.tileManager;
        footOffsetY = tileManager.InteractableMap.cellSize.y / 2f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (tileManager != null)
            {
                //取位順便解決偏移
                Vector3 footworldPos = transform.position + new Vector3(0, -footOffsetY, 0);

                //實際種田位置
                Vector3Int gridPos = tileManager.InteractableMap.WorldToCell(footworldPos);

                //抓格子中心座標->讓種田效果居中
                Vector3 gridCenterPos = tileManager.InteractableMap.GetCellCenterWorld(gridPos);

                string tileName = tileManager.GetTileName(gridPos);
                var selectedSlot = InventoryManager.Instance.toolbar.selectedSlot;

                //判斷這塊地能不能種田
                if (PlantManager.instance.TryPlant(gridPos, tileName, selectedSlot))
                {
                    Debug.Log($"✔ 種植成功 at 格子座標 {gridPos} 世界中心 {gridCenterPos}");
                    gm.uiManager.RefreshInventoryUI("Toolbar"); // 更新UI
                }
            }
        }

        // 左鍵點田地
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileManager.cropTilemap.WorldToCell(mouseWorldPos);

            if (FarmManager.instance.HasFarmTile(gridPos))
            {
                var farmTileData = FarmManager.instance.GetFarmTileData(gridPos);

                if (farmTileData.state >= 3)
                {
                    if (FarmManager.instance.TryHarvestTile(gridPos))
                    {
                        Debug.Log("收成完成！");
                    }
                }
                else
                {
                    if (FarmManager.instance.TryGrowTile(gridPos))
                    {
                        Debug.Log("田地狀態推進！");
                    }

                }
            }
        }
    }
    //掉落一件物品
    public void DropItem(ItemType type)
    {

        Vector3 playerPos = transform.position; // 主角位置
        Vector3 finalPosition = playerPos;

        float dropRadius = 5f;
        int maxAttempts = 10;
        LayerMask playerMask = LayerMask.GetMask("Player"); // 確保主角在這個 Layer

        for (int i = 0; i < maxAttempts; i++)
        {
            float randX = Random.Range(-dropRadius, dropRadius);
            float randY = Random.Range(-dropRadius, dropRadius);
            Vector3 offset = new Vector3(randX, randY, 0f);
            Vector3 tryPos = playerPos + offset;

            //避免離自己太近且不會生成在主角腳下
            if (Vector3.Distance(tryPos, playerPos) > 2f && Physics2D.OverlapCircle(tryPos, 0.2f, playerMask) == null)
            {
                finalPosition = tryPos;
                break;
            }
        }

        Collectable dropped = gm.itemManager.SpawnCollectable(type, finalPosition);

        if (dropped != null)
        {
            Debug.Log($"在主角附近丟出物品：{type} at {finalPosition}");
        }
        else
        {
            Debug.LogWarning("找不到對應 prefab：" + type);
        }
    }

    //掉落多件物品
    public void DropItem(ItemType type, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(type);
        }
    }

    /*private void OnDrawGizmos()
    {
        if (tileManager == null) return;

        Vector3 footWorldPos = transform.position + new Vector3(0, -footOffsetY, 0);
        Vector3Int gridPos = tileManager.InteractableMap.WorldToCell(footWorldPos);
        Vector3 gridCenterPos = tileManager.InteractableMap.GetCellCenterWorld(gridPos);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(footWorldPos, 0.1f); // 腳下

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(gridCenterPos, 0.1f); // 格子中心
    }*/
}
