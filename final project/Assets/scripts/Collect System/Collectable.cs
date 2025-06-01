using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    /*player walks into collectable*/
    /*add collectable to player*/
    /*delete collectable from the screen*/
    public Rigidbody2D rb2D;
    public Item item;

    public int amount = 1; // 新增數量屬性
    private bool isCollected = false;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        item = GetComponent<Item>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return; // 防止重複

        player_trigger player = collision.GetComponent<player_trigger>();
        DropManager dropManager = FindFirstObjectByType<DropManager>();

        if (player)
        {
            Debug.Log("【OnTriggerEnter2D】撞到物件：" + player.gameObject.name);

            var ui = FindFirstObjectByType<Inventory_UI>();
            if (ui == null)
            {
                Debug.LogError("⚠ 找不到 Inventory_UI");
            }

            if (item == null)
            {
                Debug.LogError("【錯誤】Collectable 上沒有 Item 元件！");
            }

            if (item != null)
            {
                if (InventoryManager.Instance == null)
                {
                    Debug.LogError("【錯誤】player.inventory 還是 null！");
                }
                isCollected = true;

                //東西加進背包
                InventoryManager.Instance.Add("Backpack", item.data, amount);
                Debug.Log($"實際撿起 {item.data.itemName}，數量: {amount}");

                DropManager.instance.SaveDroppedItemsNextFrame();
                Destroy(this.gameObject);
            }
        }
    }

    public void SetItemData(ItemData data, int amt = 1)
    {
        if (item == null)
            item = GetComponent<Item>();

        item.data = data;
        amount = amt;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && data != null && data.icon != null)
        {
            spriteRenderer.sprite = data.icon;
        }
        else
        {
            Debug.LogWarning("❌ 無法設定圖示：ItemData 或 icon 為 null");
        }
    }

    
}
