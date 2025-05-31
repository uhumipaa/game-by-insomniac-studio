using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemType itemType; // ✅ 用 enum 取代字串 ID
    public int amount = 1;

    [HideInInspector]
    public ItemData itemData;

    private void Awake()
    {
        // 透過 ItemType 找到對應的 ScriptableObject
        /*itemData = ItemDatabase.GetItemData(itemType);

        if (itemData != null)
        {
            var renderer = GetComponent<SpriteRenderer>();
            if (renderer)
                renderer.sprite = itemData.icon;
        }
        else
        {
            Debug.LogWarning($"❌ 找不到對應的 ItemData: {itemType}");
        }*/
    }

    //初始化用（建議在掉落時用）
    public void Init(ItemType type, int amt = 1)
    {
        itemType = type;
        amount = amt;
        itemData = ItemDatabase.GetItemData(type);

        var renderer = GetComponent<SpriteRenderer>();
        if (renderer && itemData != null)
            renderer.sprite = itemData.icon;
    }
}