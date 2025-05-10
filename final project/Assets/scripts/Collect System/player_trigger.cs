using UnityEngine;

public class player_trigger : MonoBehaviour
{
    public Inventory inventory;
    private void Awake()
    {
        //Debug.Log("【Awake】初始化 Inventory 成功！物件：" + this.gameObject.name);
        inventory = new Inventory(28);   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if(GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is interactable");
            }
        }
    }

    public void DropItem(ItemType type, Vector3 position)
    {
        /*Vector3 spawnLocation = transform.position; //物品掉落位置

        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);

        Vector3 spawnOffset = new Vector3(randX, randY, 0f).normalized;

        GameObject droppedGO = Instantiate(item.gameObject, dropPosition, Quaternion.identity);
        Item droppedItem = droppedGO.GetComponent<Item>();

        //droppedItem.rb2D.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
        Debug.Log("掉落物品：" + droppedGO.name + " at " + dropPosition);*/

        var prefab = GameManager.instance.itemManager.GetCollectablePrefab(type);

        if (prefab != null)
        {
            Vector3 finalPosition = position;
            const float avoidRadius = 0.8f; // 主角附近不要生成
            int maxAttempts = 10;//最多嘗試10次

            for (int i = 0; i < maxAttempts; i++)
            {
                float randX = Random.Range(-1f, 1f);
                float randY = Random.Range(-1f, 1f);
                Vector3 offset = new Vector3(randX, randY, 0f);
                Vector3 tryPos = position + offset;

                if (Vector3.Distance(tryPos, position) > avoidRadius)
                {
                    finalPosition = tryPos;
                    break;
                }
            }
            //實例化物品
            Collectable dropped = Instantiate(prefab, finalPosition, Quaternion.identity);

            Debug.Log($"丟出物品：{type} 在 {finalPosition}");
        }
        else
        {
            Debug.LogWarning("找不到對應 prefab：" + type);
        }
    }
}
