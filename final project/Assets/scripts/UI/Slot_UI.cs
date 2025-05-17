using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public int slotID;
    public Inventory inventory;
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    [SerializeField] private GameObject highlight;

    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null && slot.count > 0)
        {
            if (slot.itemData.icon != null)
            {
                itemIcon.sprite = slot.itemData.icon;
                itemIcon.color = new Color(1, 1, 1, 1);
                quantityText.text = slot.count.ToString();
                Debug.Log($"SetItem() → 設定 icon：{slot.itemData.icon.name}，數量：{slot.count}");
            }
            else
            {
                Debug.LogWarning("❌ Slot 的 icon 為 null，無法顯示圖示。");
                itemIcon.color = new Color(1, 1, 1, 0); // 隱藏
                quantityText.text = "";
            }
        }
        else
        {
            Debug.LogWarning("❌ SetItem() 收到空 slot 或 count <= 0。");
            SetEmpty();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);

    }
}
