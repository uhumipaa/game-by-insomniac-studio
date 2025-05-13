using UnityEngine;

public class player_trigger : MonoBehaviour
{
    public InventoryManager inventory;
    private TileManager tileManager;
    private void Awake()
    {
        //Debug.Log("【Awake】初始化 Inventory 成功！物件：" + this.gameObject.name);
        inventory = GetComponent<InventoryManager>();
        GameManager.instance.player = this;
    }

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if(tileManager != null)
            {
                Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
                string tileName = tileManager.GetTileName(position);
                var selectedSlot = inventory.toolbar.selectedSlot;

                //判斷這塊地能不能種田
                if (PlantManager.instance.TryPlant(position, tileName, selectedSlot))
                {
                    Debug.Log("種植成功！");
                    GameManager.instance.uiManager.RefreshInventoryUI("Toolbar"); // 更新UI
                }  
            }
        }

        // 左鍵點田地
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileManager.cropTilemap.WorldToCell(mouseWorldPos);

            if (FarmManager.instance.TryGrowTile(gridPos))
            {
                Debug.Log("田地狀態推進！");
            }
        }
    }

    //掉落一件物品
    public void DropItem(ItemType type)
    {
        var prefab = GameManager.instance.itemManager.GetCollectablePrefab(type);

        if (prefab != null)
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

            Collectable dropped = Instantiate(prefab, finalPosition, Quaternion.identity);
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
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(type);
        }
    }
}
