using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemID;
    public int amount;

    [HideInInspector]
    public ItemData itemData;

     private void Awake()
    {
        itemData = ItemDatabase.GetItemData(itemID);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer && itemData != null)
            spriteRenderer.sprite = itemData.icon;
    }
}